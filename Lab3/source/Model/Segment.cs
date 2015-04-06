using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics_Lab3
{
    public class Segment
    {
        public Point2D FromPoint { get; set; }
        public Point2D ToPoint { get; set; }

        public Segment() : this(0, 0, 0, 0) { }

        public Segment(int fromX, int fromY, int toX, int toY) : this(new Point2D(fromX, fromY), new Point2D(toX, toY)) { }

        public Segment(Point2D from, Point2D to)
        {
            FromPoint = from;
            ToPoint = to;
        }
    }
}
