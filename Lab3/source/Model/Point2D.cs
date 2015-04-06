using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics_Lab3
{
    public class Point2D
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point2D() : this(0, 0) { }

        public Point2D(int x, int y)
        {
            X = x;
            Y = y;
        }

    }
}
