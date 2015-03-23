using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Graphics_Lab2
{
    class Filter
    {
        public static Bitmap changeBrightness(Image image, int value)
        {
            Bitmap newImage = null;
            if (image != null)
            {
                int R, G, B;
                newImage = new Bitmap(image);
                LockBitmap lbmp = new LockBitmap(newImage);
                Color color;

                lbmp.LockBits();

                for (int i = 0; i < lbmp.Width; ++i)
                    for (int j = 0; j < lbmp.Height; ++j)
                    {
                        color = lbmp.GetPixel(i, j);
                        R = addValueToRGB(color.R, value);
                        G = addValueToRGB(color.G, value);
                        B = addValueToRGB(color.B, value);
                        lbmp.SetPixel(i, j, Color.FromArgb(R, G, B));
                    }

                lbmp.UnlockBits();
            }

            return newImage;
        }
        
        public static Bitmap changeContrast(Image image, double correction)
        {
            Bitmap newImage = null;
            if (image != null)
            {
                double avgBrightness = 0;
                int delta = 0, temp = 0;
                double k = 1.0 + correction / 100.0;
                int[] palette = new int[256];

                newImage = new Bitmap(image);
                LockBitmap lbmp = new LockBitmap(newImage);
                Color color;

                lbmp.LockBits();

                for (int i = 0; i < lbmp.Width; ++i)
                    for (int j = 0; j < lbmp.Height; ++j)
                    {
                        color = lbmp.GetPixel(i, j);
                        avgBrightness += color.R * 0.299 + color.G * 0.587 + color.B * 0.114;
                    }

                avgBrightness /= image.Height * image.Width;

                //Заполнение палитры
                for (int i = 0; i < 256; i++)
                {
                    delta = i - (int)avgBrightness;
                    temp = 0;

                    palette[i] = addValueToRGB(temp, (int)(avgBrightness + k * delta)); ;
                }

                for (int i = 0; i < lbmp.Width; ++i)
                    for (int j = 0; j < lbmp.Height; ++j)
                    {
                        color = lbmp.GetPixel(i, j);
                        lbmp.SetPixel(i, j, Color.FromArgb(palette[color.R], palette[color.G], palette[color.B]));
                    }

                lbmp.UnlockBits();
            }
            return newImage;
        }

        private static int addValueToRGB(int current, int value)
        {
            int tmp = current + value;
            return tmp = (tmp > 255) ? 255 : (tmp < 0) ? 0 : tmp;
        }
    }
}
