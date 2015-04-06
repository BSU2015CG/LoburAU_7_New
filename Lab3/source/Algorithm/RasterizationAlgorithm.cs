using System;

namespace Graphics_Lab3
{
    public abstract class RasterizationAlgorithm
    {
        private int[,] transitions = new int[8,4]{
        {1, 0, 0, 1},
        {0, 1, 1, 0},
        {0, -1, 1, 0},
        {-1, 0, 0, 1},
        {-1, 0, 0, -1},
        {0, -1, -1, 0},
        {0, 1, -1, 0},
        {1, 0, 0, -1}};

        public abstract Point2D[] GetArrayPresentation(Segment segment);
        public abstract String GetDescription();

        protected int GetOctet(int x, int y)
        {
            if (y <= x && y >= 0)
                return 0;
            if (x >= 0 && y > x)
                return 1;
            if (y >= -x && x < 0)
                return 2;
            if (y >= 0 && y < -x)
                return 3;
            if (y >= x && y < 0)
                return 4;
            if (x <= 0 && y < x)
                return 5;
            if (y <= -x && x > 0)
                return 6;
            if (y < 0 && y > -x)
                return 7;

            return -1;
        }

        protected Point2D FromZeroOctet(int x, int y, int octet)
        {
            int newX = x * transitions[octet, 0] + y * transitions[octet, 1];
            int newY = x * transitions[octet, 2] + y * transitions[octet, 3];

            return new Point2D(newX, newY);
        }

        protected Point2D ToZeroOctet(int x, int y, int octet)
        {
            int newX = x * transitions[octet, 0] + y * transitions[octet, 2];
            int newY = x * transitions[octet, 1] + y * transitions[octet, 3];

            return new Point2D(newX, newY);
        }
    }
}
