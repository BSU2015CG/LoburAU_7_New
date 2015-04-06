using System;
using System.Collections.Generic;

namespace Graphics_Lab3
{
    public class BresenhamAlgo : RasterizationAlgorithm
    {
        public override String GetDescription()
        {
            return "На каждом шаге X увеличивается на 1 и высчитывается ошибка - расстояние между истинным отрезком и ближай-\nшими координатами сетки. В зависи-\nмости от ошибки выбирается коорди-\nната Y.";
        }

        public override Point2D[] GetArrayPresentation(Segment segment)
        {
            List<Point2D> result = new List<Point2D>();
            Point2D point;
            int x1, y1, x2, y2, dx, dy, octet;
            int Dx, Dy, E;
            x1 = segment.FromPoint.X;
            y1 = segment.FromPoint.Y;
            x2 = segment.ToPoint.X;
            y2 = segment.ToPoint.Y;

            dx = x1;
            dy = y1;
            x1 = y1 = 0;
            x2 -= dx;
            y2 -= dy;

            octet = GetOctet(x2, y2);
            point = ToZeroOctet(x2, y2, octet);
            x2 = point.X;
            y2 = point.Y;

            Dx = x2 - x1;
            Dy = y2 - y1;
            E = 2 * Dy - Dx;

            result.Add(new Point2D(x1, y1));
            while(x1 < x2)
            {
                if(E > 0)
                {
                    x1++;
                    y1++;
                    E += 2 * (Dy - Dx);
                }
                else
                {
                    x1++;
                    E += 2 * Dy;
                }
                result.Add(new Point2D(x1, y1));
            }


            for (int i = 0; i < result.Count; i++)
            {
                result[i] = FromZeroOctet(result[i].X, result[i].Y, octet);
                result[i].X += dx;
                result[i].Y += dy;
            }

            return result.ToArray();
        }
    }
}
