using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics_Lab5
{
    public class MidPointAlgorithm : ClippingAlgorithm
    {
        List<Segment> VisibleSegments = new List<Segment>();

        public override Segment[] CutSegmentsWithWindow(Segment[] segments, Window window)
        {
            VisibleSegments.Clear();

            for (int i = 0; i < segments.Length; i++)
                CutSegment(segments[i], window);

            return VisibleSegments.ToArray();
        }

        public void CutSegment(Segment segment, Window window)
        {
            double epsilon = 0.001;
            uint code1, code2;

            code1 = GetCode(segment.X1, segment.Y1, window);
            code2 = GetCode(segment.X2, segment.Y2, window);

            if (code1 == 0 && code2 == 0)
            {
                VisibleSegments.Add(new Segment(segment));
            }
            else if ((code1 & code2) > 0 || GetSegmentLength(segment) < epsilon)
            {
                return;
            }
            else
            {
                double centerX, centerY;

                centerX = (segment.X1 + segment.X2) / 2;
                centerY = (segment.Y1 + segment.Y2) / 2;

                CutSegment(new Segment(segment.X1, segment.Y1, centerX, centerY), window);
                CutSegment(new Segment(centerX, centerY, segment.X2, segment.Y2), window);
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

        private double GetSegmentLength(Segment segment)
        {
            return Math.Sqrt(Math.Pow(segment.X2 - segment.X1, 2) + Math.Pow(segment.Y2 - segment.Y1, 2));
        }
    }
}
