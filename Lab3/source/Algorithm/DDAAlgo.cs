using System;
using System.Collections.Generic;

namespace Graphics_Lab3
{
    public class DDAAlgo : RasterizationAlgorithm
    {
        public override String GetDescription()
        {
            return "Растеризует отрезок между двумя за-\nданными точками, используя вычисле-\nния в числах с плавающей точкой. За-\nключается в постепенном увеличении\nX на 1, а Y на Dy/Dx.";
        }

        public override Point2D[] GetArrayPresentation(Segment segment)
        {
            List<Point2D> result = new List<Point2D>();
            Point2D point;
            int x1, y1, x2, y2, dx, dy, octet;
            int Dx, Dy;
            double tmp = 0;
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

            Dx = Math.Abs(x2 - x1);
            Dy = Math.Abs(y2 - y1);

            
            result.Add(new Point2D(x1, y1));
            if(Dx >= Dy)
            {
                while(x1 < x2)
                {
                    x1++;
                    tmp += (double)Dy/Dx;
                    y1 = (int)Math.Ceiling(tmp);
                    result.Add(new Point2D(x1, y1));
                }
            }
            else
            {
                while (y1 < y2)
                {
                    y1++;
                    tmp += (double)Dx / Dy;
                    x1 = (int)Math.Ceiling(tmp);
                    result.Add(new Point2D(x1, y1));
                }
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




//result.Add(new Point2D(x1, y1));
//            if (x1 == x2)
//            {
//                while (y1 < y2)
//                {
//                    y1++;
//                    result.Add(new Point2D(x1, y1));
//                }
//            }
//            else if (y1 == y2)
//            {
//                while (x1 < x2)
//                {
//                    x1++;
//                    result.Add(new Point2D(x1, y1));
//                }
//            }
//            else if(Dx >= Dy)
//            {
//                while(x1 < x2)
//                {
//                    x1++;
//                    tmp += (double)Dy/Dx;
//                    y1 = (int)Math.Floor(tmp);
//                    result.Add(new Point2D(x1, y1));
//                }
//            }
//            else
//            {
//                while (y1 < y2)
//                {
//                    y1++;
//                    tmp += (double)Dx / Dy;
//                    x1 = (int)Math.Floor(tmp);
//                    result.Add(new Point2D(x1, y1));
//                }
//            }