using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics_Lab6
{
    public class Segment
    {
        public Point3D PointA { get; set; }
        public Point3D PointB { get; set; }

        public Segment() : this(0, 0, 0, 0, 0, 0) { }

        public Segment(double x1, double y1, double z1, double x2, double y2, double z2)
        {
            PointA = new Point3D(x1, y1, z1);
            PointB = new Point3D(x2, y2, z2);
        }

        public Segment(Segment segment) : this(segment.PointA.X, segment.PointA.Y, segment.PointA.Z, segment.PointB.X, segment.PointB.Y, segment.PointB.Z) { }
    }
}
