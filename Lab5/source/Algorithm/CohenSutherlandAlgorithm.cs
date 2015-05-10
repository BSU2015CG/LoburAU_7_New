using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics_Lab5
{
    public class CohenSutherlandAlgorithm : ClippingAlgorithm
    {
        public override Segment[] CutSegmentsWithWindow(Segment[] segments, Window window)
        {
            List<Segment> result = new List<Segment>();

            for (int i = 0; i < segments.Length; i++)
            {
                Segment cutted = CutSegment(segments[i], window);

                if (cutted != null)
                    result.Add(cutted);
            }

            return result.ToArray();
        }

        public Segment CutSegment(Segment segment, Window window)
        {
            uint code1, code2;

            code1 = GetCode(segment.X1, segment.Y1, window);
            code2 = GetCode(segment.X2, segment.Y2, window);

            if (code1 == 0 && code2 == 0)
            {
                return new Segment(segment);
            }
            else if ((code1 & code2) > 0)
            {
                return null;
            }
            else
            {
                Segment newSegment = GetIntersection(segment, window);
                return CutSegment(newSegment, window);
            }
        }

        private uint GetCode(double x, double y, Window window)
        {
            uint code = 0;

            if (x < window.left.X1)
                code += 8;
            if (x > window.right.X1)
                code += 4;
            if (y < window.bottom.Y1)
                code += 2;
            if (y > window.top.Y1)
                code += 1;

            return code;
        }

        private Segment GetIntersection(Segment segment, Window window)
        {
            uint code1, code2, code;
            double outX, outY, inX, inY, k;

            code1 = GetCode(segment.X1, segment.Y1, window);
            code2 = GetCode(segment.X2, segment.Y2, window);

            if(code1 != 0)
            {
                code = code1;
                outX = segment.X1;
                outY = segment.Y1;
                inX = segment.X2;
                inY = segment.Y2;
            }
            else
            {
                code = code2;
                outX = segment.X2;
                outY = segment.Y2;
                inX = segment.X1;
                inY = segment.Y1;
            }

            if (outX - inX != 0)
                k = (outY - inY) / (outX - inX);
            else
                k = 0;

            if(code == 9 || code == 1)
            {
                if (k != 0)
                    outX = outX + (1 / k) * (window.top.Y1 - outY);
                outY = window.top.Y1;
            }
            else if(code == 5 || code == 4)
            {
                outY = outY + k * (window.right.X1 - outX);
                outX = window.right.X1;
            }
            else if(code == 6 || code == 2)
            {
                if (k != 0)
                    outX = outX + (1 / k) * (window.bottom.Y1 - outY);
                outY = window.bottom.Y1;
            }
            else if(code == 10 || code == 8)
            {
                outY = outY + k * (window.left.X1 - outX);
                outX = window.left.X1;
            }

            return new Segment(outX, outY, inX, inY);
        }
    }
}
