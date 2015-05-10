using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics_Lab5
{
    public abstract class ClippingAlgorithm
    {
        public abstract Segment[] CutSegmentsWithWindow(Segment[] segments, Window window);
    }
}
