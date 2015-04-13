using System;
using System.Collections.Generic;

namespace Graphics_Lab3
{
    public class StepByStepAlgo : RasterizationAlgorithm
    {
        public override String GetDescription()
        {
            return "Заключается в постепенном увеличении координаты x, вычислении координаты y=kx+b и последующем округлении полу-\nченного результата до целого числа.";
        }

        public override Point2D[] GetArrayPresentation(Segment segment)
        {
            List<Point2D> result = new List<Point2D>();
            Point2D point;
            int x1, y1, x2, y2, dx, dy, octet;
            double k, b;
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

            if (x2 != x1)
                k = (double)(y2 - y1) / (x2 - x1);
            else
                k = 0;

            b = y1 - k * x1;

            result.Add(new Point2D(x1, y1));
            while (x1 < x2)
            {
                x1++;
                result.Add(new Point2D(x1, (int)Math.Floor(k * x1 + b)));
            }


            for (int i = 0; i < result.Count; i++ )
            {
                result[i] = FromZeroOctet(result[i].X, result[i].Y, octet);
                result[i].X += dx;
                result[i].Y += dy;
            }

            return result.ToArray();
        }
    }
}



/*result.Add(new Point2D(x1, y1));
            if (x1 == x2)
            {
                while (y1 < y2)
                {
                    y1++;
                    result.Add(new Point2D(x1, y1));
                }
            }
            else if (y1 == y2)
            {
                while (x1 < x2)
                {
                    x1++;
                    result.Add(new Point2D(x1, y1));
                }
            }
            else if (Math.Abs(x2 - x1) >= Math.Abs(y2 - y1))
            {
                int newY;
                while (x1 < x2)
                {
                    x1++;
                    newY = (int)Math.Ceiling(k * x1 + b);

                    result.Add(new Point2D(x1, newY));

                }
            }
            else
            {
                int newX;
                while (y1 < y2)
                {
                    y1++;
                    newX = (int)Math.Ceiling((double)(y1 - b) / k);

                    result.Add(new Point2D(newX, y1));

                }
            }*/