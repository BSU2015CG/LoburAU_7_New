using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace Graphics_Lab6
{
    public partial class TransformationForm : Form
    {
        private Segment[] OriginalObject;
        private Segment[] TransformedObject;
        private double[,] ResultMatrix;
        private enum Projection { OXY, OXZ, OYZ };

        private Color ColorAllSegments = Color.RoyalBlue;

        private double[,] MatrixProjectionOxy = { {1, 0, 0, 0}, {0, 1, 0, 0}, {0, 0, 0, 0}, {0, 0, 0, 1} };
        private double[,] MatrixProjectionOxz = { { 1, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, 1 } };
        private double[,] MatrixProjectionOyz = { { 0, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, 1 } };


        public TransformationForm()
        {
            InitializeComponent();
        }

        private void TransformationForm_Load(object sender, EventArgs e)
        {
            InitZedGraph(zedGraphXY);
            InitZedGraph(zedGraphXZ);
            InitZedGraph(zedGraphYZ);

            SetTextBoxInfo();
        }

        private void InitZedGraph(ZedGraphControl zedGraph)
        {
            GraphPane pane = zedGraph.GraphPane;

            pane.Title.Text = "";
            /*pane.XAxis.MajorGrid.IsVisible = true;
            pane.YAxis.MajorGrid.IsVisible = true;*/

            pane.XAxis.Cross = 0.0;
            pane.YAxis.Cross = 0.0;

            pane.XAxis.Scale.IsSkipFirstLabel = true;
            pane.XAxis.Scale.IsSkipLastLabel = true;
            pane.XAxis.Scale.IsSkipCrossLabel = true;

            pane.YAxis.Scale.IsSkipFirstLabel = true;
            pane.YAxis.Scale.IsSkipLastLabel = true;
            pane.YAxis.Scale.IsSkipCrossLabel = true;

            pane.XAxis.Title.IsVisible = false;
            pane.YAxis.Title.IsVisible = false;

            pane.XAxis.Scale.Min = -6;
            pane.XAxis.Scale.Max = 6;
            pane.XAxis.Scale.MaxAuto = false;
            pane.XAxis.Scale.MinAuto = false;

            pane.YAxis.Scale.Min = -6;
            pane.YAxis.Scale.Max = 6;
            pane.YAxis.Scale.MaxAuto = false;  
            pane.YAxis.Scale.MinAuto = false;

            zedGraph.AxisChange();
        }

        private void ClearZedGraph(ZedGraphControl zedGraph)
        {
            zedGraph.GraphPane.CurveList.Clear();
            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }

        private void buttonSelectFile_Click(object sender, EventArgs e)
        {
            openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                OriginalObject = ReadObjectFromFile(openFileDialog.FileName);
                RefreshForm(new object(), null);    
            }
        }

        private void RefreshForm(object sender, EventArgs e)
        {
            if ((sender.Equals(trackBarRotationX) || sender.Equals(trackBarRotationY) || sender.Equals(trackBarRotationZ)) && checkBoxRotationLock.Checked)
            {
                trackBarRotationX.Value = trackBarRotationY.Value = trackBarRotationZ.Value = (sender as TrackBar).Value;
                HandleAngleTrackbar(trackBarRotationX, true);
                HandleAngleTrackbar(trackBarRotationY, false);
                HandleAngleTrackbar(trackBarRotationZ, false);
            }
            else
            {
                HandleAngleTrackbar(trackBarRotationX, true);
                HandleAngleTrackbar(trackBarRotationY, true);
                HandleAngleTrackbar(trackBarRotationZ, true);
            }

            if ((sender.Equals(trackBarScalingX) || sender.Equals(trackBarScalingY) || sender.Equals(trackBarScalingZ)) && checkBoxScalingLock.Checked)
                trackBarScalingX.Value = trackBarScalingY.Value = trackBarScalingZ.Value = (sender as TrackBar).Value;
  
            if ((sender.Equals(trackBarShiftingX) || sender.Equals(trackBarShiftingY) || sender.Equals(trackBarShiftingZ)) && checkBoxShiftingLock.Checked)
                trackBarShiftingX.Value = trackBarShiftingY.Value = trackBarShiftingZ.Value = (sender as TrackBar).Value;

            
            SetTextBoxInfo();
            ProcessObject();
            PrintMatrix(ResultMatrix);
        }

        private void HandleAngleTrackbar(TrackBar trackBar, bool withCursorShift)
        {
            if (trackBar.Value == 360)
            {
                if(withCursorShift)
                    Cursor.Position = new Point(Cursor.Position.X - 106, Cursor.Position.Y);
                trackBar.Value = 0;
            }
            if (trackBar.Value == -1)
            {
                if (withCursorShift)
                    Cursor.Position = new Point(Cursor.Position.X + 106, Cursor.Position.Y);
                trackBar.Value = 359;
            }
        }

        private void SetTextBoxInfo()
        {
            textBoxScalingX.Text = (trackBarScalingX.Value / 10.0).ToString();
            textBoxScalingY.Text = (trackBarScalingY.Value / 10.0).ToString();
            textBoxScalingZ.Text = (trackBarScalingZ.Value / 10.0).ToString();

            textBoxShiftingX.Text = (trackBarShiftingX.Value / 10.0).ToString();
            textBoxShiftingY.Text = (trackBarShiftingY.Value / 10.0).ToString();
            textBoxShiftingZ.Text = (trackBarShiftingZ.Value / 10.0).ToString();

            textBoxRotationX.Text = trackBarRotationX.Value.ToString();
            textBoxRotationY.Text = trackBarRotationY.Value.ToString();
            textBoxRotationZ.Text = trackBarRotationZ.Value.ToString();
        }

        private void PrintMatrix(double[,] matrix)
        {
            if (matrix == null)
                return;

            String strMatrix = "";
            int paddingWidth = 6;
            
            for(int i = 0; i < matrix.GetLength(0); i++)
            {
                for(int j = 0; j < matrix.GetLength(0); j++)
                {
                    strMatrix += String.Format("{0:0.##} ", matrix[i, j]).PadRight(paddingWidth);
                }
                strMatrix += Environment.NewLine + Environment.NewLine;
            }

            richTextBoxMatrix.Text = strMatrix;
            
        }

        private void ProcessObject()
        {
            if (OriginalObject == null || OriginalObject.Length == 0)
                return;

            ResultMatrix = GetResultMatrix();
            TransformedObject = ProcessSegments(OriginalObject, ResultMatrix);

            ClearZedGraph(zedGraphXY);
            ClearZedGraph(zedGraphXZ);
            ClearZedGraph(zedGraphYZ);

            DrawSegments(zedGraphXY, ProcessSegments(TransformedObject, MatrixProjectionOxy), Projection.OXY, ColorAllSegments);
            DrawSegments(zedGraphXZ, ProcessSegments(TransformedObject, MatrixProjectionOxz), Projection.OXZ, ColorAllSegments);
            DrawSegments(zedGraphYZ, ProcessSegments(TransformedObject, MatrixProjectionOyz), Projection.OYZ, ColorAllSegments);
        }

        private Segment[] ProcessSegments(Segment[] segments, double[,] matrix)
        {
            if (segments == null || segments.Length == 0)
                return null;

            Segment[] result = new Segment[segments.Length];

            for(int i = 0; i < segments.Length; i++)
            {
                double[,] newPointA = MatrixMultiply(matrix, segments[i].PointA.GetVector4D());
                double[,] newPointB = MatrixMultiply(matrix, segments[i].PointB.GetVector4D());
                result[i] = new Segment(newPointA[0, 0], newPointA[1, 0], newPointA[2, 0], newPointB[0, 0], newPointB[1, 0], newPointB[2, 0]);
            }

            return result;
        }

        double[,] GetResultMatrix()
        {
            return MatrixMultiply(GetScalingMatrix(), MatrixMultiply(GetShiftingMatrix(), GetRotationMatrix()));
        }

        double[,] GetScalingMatrix()
        {
            double[,] result = new double[4, 4];

            double a = trackBarScalingX.Value / 10.0;
            double b = trackBarScalingY.Value / 10.0;
            double c = trackBarScalingZ.Value / 10.0;

            result[0, 0] = a;
            result[1, 1] = b;
            result[2, 2] = c;
            result[3, 3] = 1;

            return result;
        }

        double[,] GetShiftingMatrix()
        {
            double[,] result = new double[4, 4];

            double tx = trackBarShiftingX.Value / 10.0;
            double ty = trackBarShiftingY.Value / 10.0;
            double tz = trackBarShiftingZ.Value / 10.0;

            result[0, 0] = 1;
            result[1, 1] = 1;
            result[2, 2] = 1;
            result[3, 3] = 1;
            result[0, 3] = tx;
            result[1, 3] = ty;
            result[2, 3] = tz;

            return result;
        }

        double[,] GetRotationMatrix()
        {
            double[,] rotationX = new double[4, 4];
            double[,] rotationY = new double[4, 4];
            double[,] rotationZ = new double[4, 4];

            double angleX = (2 * Math.PI * trackBarRotationX.Value) / 360.0;
            double angleY = (2 * Math.PI * trackBarRotationY.Value) / 360.0;
            double angleZ = (2 * Math.PI * trackBarRotationZ.Value) / 360.0;

            rotationX[0, 0] = 1;
            rotationX[1, 1] = rotationX[2, 2] = Math.Cos(angleX);
            rotationX[1, 2] = -Math.Sin(angleX);
            rotationX[2, 1] = -rotationX[1, 2];

            rotationY[1, 1] = 1;
            rotationY[0, 0] = rotationY[2, 2] = Math.Cos(angleY);
            rotationY[2, 0] = -Math.Sin(angleY);
            rotationY[0, 2] = -rotationY[2, 0];

            rotationZ[2, 2] = 1;
            rotationZ[0, 0] = rotationZ[1, 1] = Math.Cos(angleZ);
            rotationZ[0, 1] = -Math.Sin(angleZ);
            rotationZ[1, 0] = -rotationZ[0, 1];

            rotationX[3, 3] = rotationY[3, 3] = rotationZ[3, 3] = 1;

            return MatrixMultiply(rotationX, MatrixMultiply(rotationY, rotationZ));
        }

        double[,] MatrixMultiply(double[,] matrixA, double[,] matrixB)
        {
            if (matrixA.GetLength(1) != matrixB.GetLength(0))
                return null;

            int rows = matrixA.GetLength(0);
            int columns = matrixB.GetLength(1);
            int middle = matrixA.GetLength(1);
            double[,] result = new double[rows, columns];

            for (int i = 0; i < rows; i++ )
            {
                for(int j = 0; j < columns; j++ )
                {
                    double element = 0;
                    for (int k = 0; k < middle; k++)
                        element += matrixA[i, k] * matrixB[k, j];
                    result[i, j] = element;
                }
            }

            return result;
        }

        private void DrawSegments(ZedGraphControl zedGraph, Segment[] segments, Projection projection, Color color)
        {
            if (segments == null || segments.Length == 0)
                return;

            GraphPane pane = zedGraph.GraphPane;

            for (int i = 0; i < segments.Length; i++)
            {
                PointPairList list = new PointPairList();

                if (projection == Projection.OYZ)
                {
                    list.Add(segments[i].PointA.Z, segments[i].PointA.Y);
                    list.Add(segments[i].PointB.Z, segments[i].PointB.Y);
                }
                else if (projection == Projection.OXZ)
                {
                    list.Add(segments[i].PointA.X, segments[i].PointA.Z);
                    list.Add(segments[i].PointB.X, segments[i].PointB.Z);
                }
                else if (projection == Projection.OXY)
                {
                    list.Add(segments[i].PointA.X, segments[i].PointA.Y);
                    list.Add(segments[i].PointB.X, segments[i].PointB.Y);
                }

                LineItem curve = pane.AddCurve("", list, color, SymbolType.None);
            }

            pane.XAxis.Scale.Min = -6;
            pane.XAxis.Scale.Max = 6;
            pane.YAxis.Scale.Min = -6;
            pane.YAxis.Scale.Max = 6;
            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }

        private Segment[] ReadObjectFromFile(String filename)
        {
            int segmentCount;
            double x1, y1, z1, x2, y2, z2;
            Segment[] result;
            StreamReader file;

            file = new StreamReader(filename);

            segmentCount = Int32.Parse(file.ReadLine());
            result = new Segment[segmentCount];

            for (int i = 0; i < segmentCount; i++)
            {
                String[] elements = file.ReadLine().Split(new char[] { ' ' });

                x1 = Double.Parse(elements[0]);
                y1 = Double.Parse(elements[1]);
                z1 = Double.Parse(elements[2]);
                x2 = Double.Parse(elements[3]);
                y2 = Double.Parse(elements[4]);
                z2 = Double.Parse(elements[5]);

                result[i] = new Segment(x1, y1, z1, x2, y2, z2);
            }

            file.Close();
            return result;
        }
    }
}
