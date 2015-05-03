using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Graphics_Lab4
{
    public partial class Form1 : Form
    {
        private Image currentImage, currentResult;
        private List<ImageFilter> filters;
        private ImageFilter currentFilter;

        private int localWindowSize = 3;
        private int localWindowSizeMin = 3;
        private double localCoef = -0.02;
        private double adaptiveCoef = 0.3;
        private double adaptiveCoefMin = 0.3;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            filters = new List<ImageFilter>();
            filters.Add(new ImageFilter(new int[3, 3] { { 0, -1, 0 }, { -1, 4, -1 }, { 0, -1, 0 } }));
            filters.Add(new ImageFilter(new int[3, 3] { { 0, -1, 0 }, { -1, 5, -1 }, { 0, -1, 0 } }));
            filters.Add(new ImageFilter(new int[3, 3] { { 1, -2, 1 }, { -2, 5, -2 }, { 1, -2, 1 } }));
            filters.Add(new ImageFilter(new int[3, 3] { { -1, -1, -1 }, { -1, 9, -1 }, { -1, -1, -1 } }));

            comboBoxFilter.SelectedIndex = 0;
            radioButtonHighFrequency.Checked = true;
            textBoxLocal.Text = localCoef.ToString();
            textBoxAdaptive.Text = adaptiveCoef.ToString();
            textBoxLocalWindow.Text = localWindowSize.ToString();

            String imagePath = "Sample.png";
            textBoxImagePath.Text = imagePath;
            currentImage = Image.FromFile(imagePath);
            pictureBoxSource.Image = currentImage;
        }

        private void buttonChooseImage_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                String filepath = openFileDialog1.FileName;
                currentImage = Image.FromFile(filepath);
                textBoxImagePath.Text = filepath;
                pictureBoxSource.Image = currentImage;
                pictureBoxResult.Image = null;
            }
        }

        private void comboBoxFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentFilter = filters[comboBoxFilter.SelectedIndex];
            richTextBoxFilter.Text = currentFilter.ToString();
        }

        private void radioButtonHighFrequency_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonHighFrequency.Checked == true)
            {
                radioButtonLocalProc.Checked = false;
                radioButtonAdaptive.Checked = false;
            }

            pictureBoxResult.Image = null;
        }

        private void radioButtonLocalProc_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonLocalProc.Checked == true)
            { 
                radioButtonHighFrequency.Checked = false;
                radioButtonAdaptive.Checked = false;
            }

            pictureBoxResult.Image = null;
        }

        private void radioButtonAdaptive_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonAdaptive.Checked == true)
            {
                radioButtonHighFrequency.Checked = false;
                radioButtonLocalProc.Checked = false;
            }

            pictureBoxResult.Image = null;
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            pictureBoxResult.Image = null;

            if (currentImage == null)
                return;

            if (radioButtonHighFrequency.Checked)
            {
                currentResult = highFrequenceFilter((Bitmap)currentImage, currentFilter);
                pictureBoxResult.Image = currentResult;
            }

            if (radioButtonLocalProc.Checked)
            {
                try
                {
                    localCoef = Convert.ToDouble(textBoxLocal.Text);
                }
                catch (FormatException ex)
                {
                    textBoxLocal.Text = localCoef.ToString();
                }

                currentResult = localProcessingFilter((Bitmap)currentImage, localWindowSize, localCoef);
                pictureBoxResult.Image = currentResult;   
            }

            if (radioButtonAdaptive.Checked)
            {
                currentResult = adaptiveProcessingFilter((Bitmap)currentImage, adaptiveCoef);
                pictureBoxResult.Image = currentResult;
            }
        }

        private void trackBarLocal_Scroll(object sender, EventArgs e)
        {
            localWindowSize = localWindowSizeMin + trackBarLocal.Value * 2;
            textBoxLocalWindow.Text = localWindowSize.ToString();
        }

        private void trackBarAdaptive_Scroll(object sender, EventArgs e)
        {
            adaptiveCoef = adaptiveCoefMin + trackBarAdaptive.Value * 0.1;
            textBoxAdaptive.Text = adaptiveCoef.ToString();
        }

        private Bitmap highFrequenceFilter(Bitmap originalBitmap, ImageFilter filter)
        {
            Bitmap newBitmap = new Bitmap(originalBitmap.Width, originalBitmap.Height);

            LockBitmap original = new LockBitmap(originalBitmap);
            LockBitmap filtered = new LockBitmap(newBitmap);

            int width = originalBitmap.Width;
            int height = originalBitmap.Height;

            original.LockBits();
            filtered.LockBits();

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    int[] result = new int[3];

                    if (y > 0 && x > 0)
                        SumArrays(result, GetPixelWithMultiply(original, x - 1, y - 1, filter.Matrix[0, 0]));
                    if (y > 0)
                        SumArrays(result, GetPixelWithMultiply(original, x, y - 1, filter.Matrix[0, 1]));
                    if (y > 0 && x < width - 1)
                        SumArrays(result, GetPixelWithMultiply(original, x + 1, y - 1, filter.Matrix[0, 2]));
                    if (x > 0)
                        SumArrays(result, GetPixelWithMultiply(original, x - 1, y, filter.Matrix[1, 0]));

                    SumArrays(result, GetPixelWithMultiply(original, x, y, filter.Matrix[1, 1]));

                    if (x < width - 1)
                        SumArrays(result, GetPixelWithMultiply(original, x + 1, y, filter.Matrix[1, 2]));
                    if (y < height - 1 && x > 0)
                        SumArrays(result, GetPixelWithMultiply(original, x - 1, y + 1, filter.Matrix[2, 0]));
                    if (y < height - 1)
                        SumArrays(result, GetPixelWithMultiply(original, x, y + 1, filter.Matrix[2, 1]));
                    if (y < height - 1 && x < width - 1)
                        SumArrays(result, GetPixelWithMultiply(original, x + 1, y + 1, filter.Matrix[2, 2]));

                    for (int i = 0; i < 3; i++)
                    {
                        if (result[i] < 0)
                            result[i] = 0;
                        if (result[i] > 255)
                            result[i] = 255;
                    }

                    filtered.SetPixel(x, y, Color.FromArgb(result[0], result[1], result[2]));
                }

            filtered.UnlockBits();
            original.UnlockBits();

            return newBitmap;
        }

        private Bitmap localProcessingFilter(Bitmap originalBitmap, int windowSize, double coef)
        {
            Bitmap newBitmap = new Bitmap(originalBitmap.Width, originalBitmap.Height);

            LockBitmap original = new LockBitmap(originalBitmap);
            LockBitmap filtered = new LockBitmap(newBitmap);

            int width = originalBitmap.Width;
            int height = originalBitmap.Height;

            original.LockBits();
            filtered.LockBits();

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    Color pixel = original.GetPixel(x, y);
                    int[] result = new int[3] { pixel.R, pixel.G, pixel.B };

                    Square indexes = GetSquare(x, y, windowSize, height, width);
                    double[] average = GetLocalAverage(original, indexes);
                    double[] square = GetLocalSquare(original, indexes);

                    for (int i = 0; i < 3; i++)
                    {
                        int limit = (int)(average[i] + coef * square[i] + 0.5);

                        if (result[i] >= limit)
                            result[i] = 0;
                        else
                            result[i] = 255;
                    }

                    filtered.SetPixel(x, y, Color.FromArgb(result[0], result[1], result[2]));
                }

            filtered.UnlockBits();
            original.UnlockBits();

            return newBitmap;
        }

        private Bitmap adaptiveProcessingFilter(Bitmap originalBitmap, double coef)
        {
            Bitmap newBitmap = new Bitmap(originalBitmap.Width, originalBitmap.Height);

            LockBitmap original = new LockBitmap(originalBitmap);
            LockBitmap filtered = new LockBitmap(newBitmap);

            int width = originalBitmap.Width;
            int height = originalBitmap.Height;

            original.LockBits();
            filtered.LockBits();

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    Color pixel = original.GetPixel(x, y);
                    int[] result = new int[3] { pixel.R, pixel.G, pixel.B };

                    int[] limits = GetAdaptiveLimits(original, x, y);

                    for (int i = 0; i < 3; i++)
                    {
                        if (result[i] > limits[i])
                            result[i] = 255;
                        else
                            result[i] = 0;
                    }

                    filtered.SetPixel(x, y, Color.FromArgb(result[0], result[1], result[2]));
                }

            filtered.UnlockBits();
            original.UnlockBits();

            return newBitmap;
        }

        private int[] GetPixelWithMultiply(LockBitmap bmp, int x, int y, int coef)
        {
            int[] result = new int[3];
            Color color = bmp.GetPixel(x, y);

            result[0] = color.R * coef;
            result[1] = color.G * coef;
            result[2] = color.B * coef;

            return result;
        }

        private void SumArrays(int[] destination, int[] source)
        {
            for (int i = 0; i < source.Length; i++)
                destination[i] += source[i];
        }

        private double[] GetLocalAverage(LockBitmap bmp, Square indexes)
        {
            int elementCount = (indexes.X2 - indexes.X1 + 1) * (indexes.Y2 - indexes.Y1 + 1);
            double[] result = new double[3];

            for(int x = indexes.X1; x < indexes.X2; x++)
                for(int y = indexes.Y1; y < indexes.Y2; y++)
                {
                    Color color = bmp.GetPixel(x, y);
                    result[0] += color.R;
                    result[1] += color.G;
                    result[2] += color.B;
                }

            for (int i = 0; i < 3; i++)
                result[i] /= elementCount;

            return result;
        }

        private double[] GetLocalSquare(LockBitmap bmp, Square indexes)
        {
            double[] result = new double[3];

            for (int x = indexes.X1; x < indexes.X2; x++)
                for (int y = indexes.Y1; y < indexes.Y2; y++)
                {
                    Color color = bmp.GetPixel(x, y);
                    result[0] += Math.Pow(color.R, 2);
                    result[1] += Math.Pow(color.G, 2);
                    result[2] += Math.Pow(color.B, 2);
                }

            for (int i = 0; i < 3; i++)
                result[i] = Math.Sqrt(result[i]);

            return result;
        }

        private int[] GetLocalMax(LockBitmap bmp, Square indexes)
        {
            int[] result = new int[3];

            for (int x = indexes.X1; x < indexes.X2; x++)
                for (int y = indexes.Y1; y < indexes.Y2; y++)
                {
                    Color color = bmp.GetPixel(x, y);

                    if (color.R > result[0])
                        result[0] = color.R;
                    if (color.G > result[1])
                        result[1] = color.G;
                    if (color.B > result[2])
                        result[2] = color.B;
                }

            return result;
        }

        private int[] GetLocalMin(LockBitmap bmp, Square indexes)
        {
            int[] result = new int[3] { 256, 256, 256 };

            for (int x = indexes.X1; x < indexes.X2; x++)
                for (int y = indexes.Y1; y < indexes.Y2; y++)
                {
                    Color color = bmp.GetPixel(x, y);

                    if (color.R < result[0])
                        result[0] = color.R;
                    if (color.G < result[1])
                        result[1] = color.G;
                    if (color.B < result[2])
                        result[2] = color.B;
                }

            return result;
        }

        private int[] GetAdaptiveLimits(LockBitmap bmp, int x, int y)
        {
            int[] result = new int[3] { -1, -1, -1 };

            int windowSize = 1;

            for (int i = 0; i < 2; i++ )
            {
                Square indexes = GetSquare(x, y, windowSize, bmp.Height, bmp.Width);
                int[] fmax = GetLocalMax(bmp, indexes);
                int[] fmin = GetLocalMin(bmp, indexes);
                double[] P = GetLocalAverage(bmp, indexes);
                double[] dfmax = new double[3];
                double[] dfmin = new double[3];

                for (int j = 0; j < 3; j++ )
                {
                    dfmax[j] = Math.Abs(fmax[j] - P[j]);
                    dfmin[j] = Math.Abs(fmin[j] - P[j]); 
                }

                 for (int j = 0; j < 3; j++ )
                 {
                     if(result[j] != -1)
                         continue;

                     if (dfmax[j] > dfmin[j])
                     {
                         result[j] = (int)(adaptiveCoef * (2.0/3 * fmin[j] + 1.0/3 * P[j]) + 0.5);
                     }
                     else if (dfmax[j] < dfmin[j])
                     {
                         result[j] = (int)(adaptiveCoef * (1.0 / 3 * fmin[j] + 2.0 / 3 * P[j]) + 0.5); //fmax??
                     }
                     else if (dfmax[j] == dfmin[j])
                     {
                         if (i == 1)
                         {
                             result[j] = (int)(adaptiveCoef * P[j] + 0.5);
                         }
                         if (i == 0)
                         {
                             windowSize += 2;
                         }
                     }
                 }
            }

            return result;
        }

        private Square GetSquare(int centerX, int centerY, int squareSize, int matrixHeight, int matrixWidth)
        {
            int left = 0, right = 0, top = 0, bottom = 0, width = (squareSize - 1)/2;

            left = centerX - width;
            right = centerX + width;
            top = centerY - width;
            bottom = centerY + width;

            left = left < 0 ? 0 : left;
            right = right >= matrixWidth ? matrixWidth - 1 : right;
            top = top < 0 ? 0 : top;
            bottom = bottom >= matrixHeight ? matrixHeight - 1 : bottom;

            return new Square(left, right, top, bottom);
        }

    }
}





/*public static Bitmap ImageToMonotone(Bitmap original)
       {
           Bitmap newBitmap = new Bitmap(original.Width, original.Height);
           Graphics g = Graphics.FromImage(newBitmap);

           ColorMatrix colorMatrix = new ColorMatrix(
              new float[][] 
              {
                 new float[] {.3f, .3f, .3f, 0, 0},
                 new float[] {.59f, .59f, .59f, 0, 0},
                 new float[] {.11f, .11f, .11f, 0, 0},
                 new float[] {0, 0, 0, 1, 0},
                 new float[] {0, 0, 0, 0, 1}
              });

           ImageAttributes attributes = new ImageAttributes();

           attributes.SetColorMatrix(colorMatrix);

           g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
              0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

           g.Dispose();
           return newBitmap;
       }*/
