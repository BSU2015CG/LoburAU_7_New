using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grapics_Lab1
{
    public class Converter : Form1
    {
        public static ColorRGB HLStoRGB(int H, double L, double S)
        {
            int[] result = new int[3];
            double[] T = new double[3];
            double Q = (L < 0.5) ? (L * (1.0 + S)) : (L + S - L * S);
            double P = 2.0 * L - Q;

            T[0] = H / 360.0 + 1.0 / 3;
            T[1] = H / 360.0;
            T[2] = H / 360.0 - 1.0 / 3;

            for (int i = 0; i < 3; i++)
            {
                if (T[i] < 0)
                    T[i] += 1.0;
                if (T[i] > 1)
                    T[i] -= 1.0;

                if (T[i] < 1.0 / 6)
                    result[i] = (int)((P + ((Q - P) * 6.0 * T[i])) * 255.0);
                else
                    if (T[i] >= 1.0 / 6 && T[i] < 1.0 / 2)
                        result[i] = (int)(Q * 255.0);
                    else
                        if (T[i] >= 1.0 / 2 && T[i] < 2.0 / 3)
                            result[i] = (int)((P + ((Q - P) * 6.0 * (2.0 / 3 - T[i]))) * 255.0);
                        else
                            result[i] = (int)(P * 255.0);
            }

            return new ColorRGB(result[0], result[1], result[2]);
        }

        public static ColorHLS RGBtoHLS(int R, int G, int B)
        {
            int H = 0; double S, L;
            double r = R / 255.0;
            double g = G / 255.0;
            double b = B / 255.0;
            double max = Math.Max(r, Math.Max(g, b));
            double min = Math.Min(r, Math.Min(g, b));
            double delta = max - min;

            L = (max + min) / 2.0;

            if (delta == 0)
            {
                H = 0;
                S = 0.0;
                return new ColorHLS(H, L, S);
            }
            else
                S = delta / (1 - Math.Abs(2 * L - 1));

            if (max == r)
                if (g >= b)
                    H = (int)(60 * ((g - b) / delta));
                else
                    H = (int)(60 * (((g - b) / delta) + 6.0));
            if (max == g)
                H = (int)(60 * (((b - r) / delta) + 2.0));
            if (max == b)
                H = (int)(60 * (((r - g) / delta) + 4.0));

            return new ColorHLS(H, L, S);
        }

        public static ColorCMYK RGBtoCMYK(int R, int G, int B)
        {
            double C, M, Y, K;
            double r = R / 255.0;
            double g = G / 255.0;
            double b = B / 255.0;

            K = 1 - Math.Max(r, Math.Max(g, b));
            if (1 - K == 0)
                return new ColorCMYK(0, 0, 0, 0);

            C = (1 - r - K) / (1 - K);
            M = (1 - g - K) / (1 - K);
            Y = (1 - b - K) / (1 - K);

            return new ColorCMYK(C, M, Y, K);
        }

        public static ColorRGB CMYKtoRGB(double C, double M, double Y, double K)
        {
            int R, G, B;

            R = Convert.ToInt32(255.0 * (1 - C) * (1 - K));
            G = Convert.ToInt32(255.0 * (1 - M) * (1 - K));
            B = Convert.ToInt32(255.0 * (1 - Y) * (1 - K));

            return new ColorRGB(R, G, B);
        }

        public static ColorLuv RGBtoLuv(int R, int G, int B)
        {
            double X, Y, Z, L, u, v, _u, _v;
            double Yn = 1.0, xn = 0.312713, yn = 0.329016, Un, Vn; //D65
            ColorXYZ xyz = RGBtoXYZ(R, G, B);

            X = xyz.X;
            Y = xyz.Y;
            Z = xyz.Z;

            if (X + 15 * Y + 3 * Z == 0)
                return new ColorLuv(0, 0, 0);

            Un = 4 * xn / (-2 * xn + 12 * yn + 3);
            Vn = 9 * yn / (-2 * xn + 12 * yn + 3);
            _u = 4 * X / (X + 15 * Y + 3 * Z);
            _v = 9 * Y / (X + 15 * Y + 3 * Z);

            if (Y / Yn <= Math.Pow(6 / 29.0, 3))
                L = Math.Pow(29 / 3.0, 3) * Y / Yn;
            else
                L = 116 * Math.Pow(Y / Yn, 1 / 3.0) - 16;

            u = 13 * L * (_u - Un);
            v = 13 * L * (_v - Vn);

            return new ColorLuv(L, u, v);
        }

        public static ColorRGB LuvtoRGB(double L, double u, double v)
        {
            int R, G, B;
            double X, Y, Z, a, b, c, d, r, g, bB;
            double eps = 0.008856, k = 903.3, xn = 95.047, yn = 100.0, zn = 108.883, Un, Vn; //D65
            ColorRGB rgb;

            if (L == 0)
                return new ColorRGB(0, 0, 0);

            if (L > k * eps)
                Y = Math.Pow((L + 16) / 116, 3);
            else
                Y = L / k;

            Un = 4 * xn / (xn + 15 * yn + 3 * zn);
            Vn = 9 * yn / (xn + 15 * yn + 3 * zn);

            a = (1.0 / 3.0) * ((52.0 * L) / (u + 13 * L * Un) - 1.0);
            b = -5 * Y;
            c = -1.0 / 3.0;
            d = Y * ((39.0 * L) / (v + 13.0 * L * Vn) - 5.0);

            X = (d - b) / (a - c);
            Z = X * a + b;

            rgb = XYZtoRGB(X, Y, Z);

            R = rgb.R;
            G = rgb.G;
            B = rgb.B;

            return new ColorRGB(R, G, B);
        }

        public static ColorXYZ RGBtoXYZ(int R, int G, int B)
        {
            double X, Y, Z;
            double r = R / 255.0;
            double g = G / 255.0;
            double b = B / 255.0;

            X = 0.412453 * r + 0.357580 * g + 0.180423 * b;
            Y = 0.212671 * r + 0.715160 * g + 0.072169 * b;
            Z = 0.019334 * r + 0.119193 * g + 0.950227 * b;

            X = (X > 1.0) ? 1.0 : X;
            Y = (Y > 1.0) ? 1.0 : Y;
            Z = (Z > 1.0) ? 1.0 : Z;

            return new ColorXYZ(X, Y, Z);
        }

        public static ColorRGB XYZtoRGB(double X, double Y, double Z)
        {
            int R, G, B;
            double r, g, b;

            r = 3.24045 * X + (-1.53714) * Y + (-0.498531) * Z;
            g = (-0.969258) * X + 1.87599 * Y + 0.0415557 * Z;
            b = 0.0556352 * X + (-0.203996) * Y + 1.05707 * Z;

            r = (r <= 0.0031308) ? (12.92 * r) : (1.055 * Math.Pow(r, 1 / 2.4) - 0.055);
            g = (g <= 0.0031308) ? (12.92 * g) : (1.055 * Math.Pow(g, 1 / 2.4) - 0.055);
            b = (b <= 0.0031308) ? (12.92 * b) : (1.055 * Math.Pow(b, 1 / 2.4) - 0.055);

            R = Convert.ToInt32(r * 255.0);
            G = Convert.ToInt32(g * 255.0);
            B = Convert.ToInt32(b * 255.0);

            for (int i = 0; i < 3; i++)
                cuttedComponents[i] = false;

            if (R < 0)
            {
                R = 0;
                cuttedComponents[0] = true;
                colorCut = true;
            }
            if (G < 0)
            {
                G = 0;
                cuttedComponents[1] = true;
                colorCut = true;
            }
            if (B < 0)
            {
                B = 0;
                cuttedComponents[2] = true;
                colorCut = true;
            }

            if (R > 255)
            {
                R = 255;
                cuttedComponents[0] = true;
                colorCut = true;
            }
            if (G > 255)
            {
                G = 255;
                cuttedComponents[1] = true;
                colorCut = true;
            }
            if (B > 255)
            {
                B = 255;
                cuttedComponents[2] = true;
                colorCut = true;
            }

            return new ColorRGB(R, G, B);
        }
    }
}
