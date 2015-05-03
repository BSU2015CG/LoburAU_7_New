using System;

namespace Graphics_Lab4
{
    class ImageFilter
    {
        public int[,] Matrix { get; set; }

        public ImageFilter(int[,] matrix)
        {
            Matrix = new int[matrix.GetLength(0), matrix.GetLength(1)];

            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                    Matrix[i, j] = matrix[i, j];
        }

        public override String ToString()
        {
            String result = "";

            if(Matrix == null)
                return "";

            for (int i = 0; i < Matrix.GetLength(0); i++)
            {
                for (int j = 0; j < Matrix.GetLength(1); j++)
                    result += String.Format("{0, 4} ", Matrix[i, j]);
                result += "\n";
            }

            return result;
        }
    }
}
