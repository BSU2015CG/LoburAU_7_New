using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using ZedGraph;

namespace Graphics_Lab5
{
    public partial class SegmentCutForm : Form
    {
        private Window ClippingWindow;
        private Segment[] AllSegments;
        private Segment[] VisibleSegments;
        private ClippingAlgorithm CurrentAlgorithm;

        private Color ColorAllSegments = Color.RoyalBlue;
        private Color ColorVisibleSegments = Color.FromArgb(64, 204, 14);
        private Color ColorWindow = Color.Black;


        public SegmentCutForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBoxAlgorithm.SelectedIndex = 0;

            GraphPane pane = zedGraph.GraphPane;

            pane.Title.Text = "";
            pane.XAxis.MajorGrid.IsVisible = true;
            pane.YAxis.MajorGrid.IsVisible = true;

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

            zedGraph.AxisChange();
        }

        private void buttonSelectFile_Click(object sender, EventArgs e)
        {
            if(openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                AllSegments = ReadSegmentsFromFile(openFileDialog.FileName, out ClippingWindow);
                VisibleSegments = null;

                zedGraph.GraphPane.CurveList.Clear();
                zedGraph.AxisChange();
                zedGraph.Invalidate();

                DrawSegments(AllSegments, ColorAllSegments, SymbolType.Circle, 1.0f);
                DrawSegments(ClippingWindow.GetSegments(), ColorWindow, SymbolType.None, 2.0f);
                
                textBoxFilepath.Text = openFileDialog.FileName;
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            RemoveVisibleSegments();
            VisibleSegments = null;
        }

        private void buttonCut_Click(object sender, EventArgs e)
        {
            if (VisibleSegments != null || AllSegments == null)
                return;

            VisibleSegments = CurrentAlgorithm.CutSegmentsWithWindow(AllSegments, ClippingWindow);
            DrawSegments(VisibleSegments, ColorVisibleSegments, SymbolType.Circle, 2.0f);
        }

        private void comboBoxAlgorithm_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxAlgorithm.SelectedIndex == 0)
                CurrentAlgorithm = new CohenSutherlandAlgorithm();
            else if(comboBoxAlgorithm.SelectedIndex == 1)
                CurrentAlgorithm = new MidPointAlgorithm();

            buttonClear_Click(null, null);
        }

        private Segment[] ReadSegmentsFromFile(String filename, out Window window)
        {
            int segmentCount, x1, y1, x2, y2;
            Segment[] result;
            StreamReader file;
            
            file = new StreamReader(filename);

            window = new Window();
            segmentCount = Int32.Parse(file.ReadLine());
            result = new Segment[segmentCount];

            for(int i = 0; i < segmentCount + 1; i++)
            {
                String[] elements = file.ReadLine().Split(new char[] {' '});
                x1 = Int32.Parse(elements[0]);
                y1 = Int32.Parse(elements[1]);
                x2 = Int32.Parse(elements[2]);
                y2 = Int32.Parse(elements[3]);

                if (i != segmentCount)
                    result[i] = new Segment(x1, y1, x2, y2);
                else
                    window = new Window(x1, y1, x2, y2);
            }

            file.Close();
            return result;
        }

        private void DrawSegments(Segment[] segments, Color color, SymbolType symbols, float lineWidth)
        {
            if (segments == null || segments.Length == 0)
                return;

            GraphPane pane = zedGraph.GraphPane;

            for (int i = 0; i < segments.Length; i++ )
            {
                PointPairList list = new PointPairList();

                list.Add(segments[i].X1, segments[i].Y1);
                list.Add(segments[i].X2, segments[i].Y2);

                LineItem curve = pane.AddCurve("", list, color, symbols);
                curve.Line.Width = lineWidth;
                curve.Symbol.Size = 8.0f;
                curve.Symbol.Fill.Type = FillType.Solid;
                pane.CurveList.Move(pane.CurveList.Count - 1, -pane.CurveList.Count);
            }

            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }

        private void RemoveVisibleSegments()
        {
            if (VisibleSegments == null || VisibleSegments.Length == 0)
                return;

            int segmentCount = VisibleSegments.Length;
            GraphPane pane = zedGraph.GraphPane;

            pane.CurveList.RemoveRange(0, segmentCount);

            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }
    }

}
