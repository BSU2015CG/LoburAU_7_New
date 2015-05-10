using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics_Lab5
{
    public class Window
    {
        public Segment left { get; set; }
        public Segment top { get; set; }
        public Segment right { get; set; }
        public Segment bottom { get; set; }

        public Window() : this(0, 0, 0, 0) { }

        public Window(int x1, int y1, int x2, int y2)
        {
            left = new Segment(x1, y1, x1, y2);
            top = new Segment(x1, y2, x2, y2);
            right = new Segment(x2, y2, x2, y1);
            bottom = new Segment(x2, y1, x1, y1);
        }

        public Segment[] GetSegments()
        {
            Segment[] result = new Segment[4];
            result[0] = left;
            result[1] = top;
            result[2] = right;
            result[3] = bottom;

            return result;
        }
    }
}
