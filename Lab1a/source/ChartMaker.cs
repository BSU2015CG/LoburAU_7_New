using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Graphics_Lab2
{
    class ChartMaker
    {
        public static double[] CalculateAverageRGB(Image image)
        {
            double[] result = null;
            if (image != null)
            {
                int totalR = 0, totalG = 0, totalB = 0;
                Bitmap bmp = new Bitmap(image);
                int pixelCount = bmp.Width * bmp.Height;
                result = new double[3];

                Color color;
                for (int i = 0; i < bmp.Width; ++i)
                    for (int j = 0; j < bmp.Height; ++j)
                    {
                        color = bmp.GetPixel(i, j);
                        totalR += color.R;
                        totalG += color.G;
                        totalB += color.B;
                    }

                result[0] = (double)totalR / pixelCount;
                result[1] = (double)totalG / pixelCount;
                result[2] = (double)totalB / pixelCount;
            }

            return result;
        }

        public static Bitmap[] CalculateBarChart(Image image, int height, int width)
        {
            Bitmap[] barCharts = null;
            if (image != null)
            {
                int max = 0, totalR = 0, totalG = 0, totalB = 0;
                int avrR, avrG, avrB;
                double scale = 0.0;
                Bitmap bmp = new Bitmap(image);
                int pixelCount = bmp.Width * bmp.Height;
                barCharts = new Bitmap[4];

                for (int k = 0; k < 4; k++)
                    barCharts[k] = new Bitmap(width, height);

                int[] A = new int[256];
                int[] R = new int[256];
                int[] G = new int[256];
                int[] B = new int[256];

                Color color;
                for (int i = 0; i < bmp.Width; ++i)
                    for (int j = 0; j < bmp.Height; ++j)
                    {
                        color = bmp.GetPixel(i, j);
                        ++R[color.R];
                        ++G[color.G];
                        ++B[color.B];
                        totalR += color.R;
                        totalG += color.G;
                        totalB += color.B;
                    }

                avrR = totalR / pixelCount;
                avrG = totalG / pixelCount;
                avrB = totalB / pixelCount;

                for (int i = 0; i < 256; ++i)
                {
                    A[i] = (R[i] + G[i] + B[i]) / 3;
                    if (R[i] > max)
                        max = R[i];
                    if (G[i] > max)
                        max = G[i];
                    if (B[i] > max)
                        max = B[i];
                }

                scale = (double)max / height;
                for (int i = 0; i < width; ++i)
                {
                    for (int j = height - 1; j > height - A[i] / scale; --j)
                    {
                        barCharts[0].SetPixel(i, j, Color.Black);
                    }

                    for (int j = height - 1; j > height - R[i] / scale; --j)
                    {
                        barCharts[1].SetPixel(i, j, Color.Red);
                    }

                    for (int j = height - 1; j > height - G[i] / scale; --j)
                    {
                        barCharts[2].SetPixel(i, j, Color.Green);
                    }

                    for (int j = height - 1; j > height - B[i] / scale; --j)
                    {
                        barCharts[3].SetPixel(i, j, Color.Blue);
                    }
                }

                for (int j = height - 1; j >= 0; --j)
                    barCharts[1].SetPixel(avrR, j, Color.Black);

                for (int j = height - 1; j >= 0; --j)
                    barCharts[2].SetPixel(avrG, j, Color.Black);

                for (int j = height - 1; j >= 0; --j)
                    barCharts[3].SetPixel(avrB, j, Color.Black);

            }

            return barCharts;
        }
    }
}
