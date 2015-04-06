using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using ZedGraph;

namespace Graphics_Lab3
{
    public partial class Form1 : Form
    {
        private Segment[] segments;
        private RasterizationAlgorithm currentAlgo;
        private Stopwatch timer;

        public Form1()
        {
            InitializeComponent();
            timer = new Stopwatch();
            GenerateHugeArrayOfSegments("crosses.txt", 100);
        }

        private void GenerateHugeArrayOfSegments(String filename, int segmentCount)
        {
            int crossSize = 3, distance = 8;
            int startX = -(int)Math.Sqrt(segmentCount);
            int currentX = startX;
            int currentY = (int)Math.Sqrt(segmentCount);
            StreamWriter file = new StreamWriter(filename);

            for(int i = 0; i < (int)Math.Sqrt(segmentCount); i++)
            {
                for (int j = 0; j < (int)Math.Sqrt(segmentCount); j++)
                {
                    file.WriteLine("{0} {1} {2} {3}", currentX, currentY, currentX - crossSize, currentY + crossSize);
                    file.WriteLine("{0} {1} {2} {3}", currentX, currentY, currentX - crossSize, currentY - crossSize);
                    file.WriteLine("{0} {1} {2} {3}", currentX, currentY, currentX + crossSize, currentY - crossSize);
                    file.WriteLine("{0} {1} {2} {3}", currentX, currentY, currentX + crossSize, currentY + crossSize);
                    currentX += distance;
                }
                currentY -= distance;
                currentX = startX;
            }

            file.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GraphPane pane = zedGraphControl1.GraphPane;

            pane.Title.Text = "";
            pane.XAxis.MajorGrid.IsVisible = true;
            pane.YAxis.MajorGrid.IsVisible = true;

            pane.XAxis.Scale.MajorStep = 1.0;
            pane.YAxis.Scale.MajorStep = 1.0;
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

            zedGraphControl1.AxisChange();

            comboBox1.Items.Add("Step-by-step");
            comboBox1.Items.Add("DDA");
            comboBox1.Items.Add("Bresenham");
            comboBox1.SelectedIndex = 0; 
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            GraphPane pane = zedGraphControl1.GraphPane;
            pane.CurveList.Clear();
            pane.GraphObjList.Clear();
            zedGraphControl1.Invalidate();

            if(checkBox1.Checked)
            {
                textBoxX1.ReadOnly = true;
                textBoxX2.ReadOnly = true;
                textBoxY1.ReadOnly = true;
                textBoxY2.ReadOnly = true;

                textBoxFilePath.Visible = true;
                buttonBrowse.Visible = true;
            }
            else
            {
                textBoxX1.ReadOnly = false;
                textBoxX2.ReadOnly = false;
                textBoxY1.ReadOnly = false;
                textBoxY2.ReadOnly = false;

                textBoxFilePath.Visible = false;
                buttonBrowse.Visible = false;
                textBoxFilePath.Text = "";
                textBoxTime.Text = "";
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox1.SelectedIndex == 0)
                currentAlgo = new StepByStepAlgo();  

            if(comboBox1.SelectedIndex == 1)
                currentAlgo = new DDAAlgo();

            if(comboBox1.SelectedIndex == 2)
                currentAlgo = new BresenhamAlgo();

            richTextBox1.Text = currentAlgo.GetDescription();

            if (checkBox1.Checked == true)
                textBoxTime.Text = DrawSegments(segments) + " ms";
            else
                buttonDraw_Click(null, null);
        }

        private void buttonDraw_Click(object sender, EventArgs e)
        {
            GraphPane pane = zedGraphControl1.GraphPane;
            pane.CurveList.Clear();
            pane.GraphObjList.Clear();
            zedGraphControl1.Invalidate();

            if (checkBox1.Checked == true)
            {
                textBoxTime.Text = DrawSegments(segments) + " ms";
                return;
            }

            if (textBoxX1.Text == "" || textBoxX2.Text == "" || textBoxY1.Text == "" || textBoxY2.Text == "")
                return;

            int x1 = Convert.ToInt32(textBoxX1.Text);
            int y1 = Convert.ToInt32(textBoxY1.Text);
            int x2 = Convert.ToInt32(textBoxX2.Text);
            int y2 = Convert.ToInt32(textBoxY2.Text);

            textBoxTime.Text = DrawSegments(new Segment[]{new Segment(x1, y1, x2, y2)}) + " ms";
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBoxFilePath.Text = openFileDialog1.FileName;

                string[] strSegments = File.ReadAllLines(openFileDialog1.FileName);
                segments = new Segment[strSegments.Length];

                for (int i = 0; i < strSegments.Length; i++)
                {
                    string[] segment = strSegments[i].Split(new Char[] { ' ' });
                    segments[i] = new Segment(Convert.ToInt32(segment[0]), Convert.ToInt32(segment[1]), Convert.ToInt32(segment[2]), Convert.ToInt32(segment[3]));
                }

                textBoxTime.Text = DrawSegments(segments) + " ms";
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            GraphPane pane = zedGraphControl1.GraphPane;
            pane.GraphObjList.Clear();
            pane.CurveList.Clear();
            zedGraphControl1.Invalidate();
            textBoxX1.Text = "";
            textBoxX2.Text = "";
            textBoxY1.Text = "";
            textBoxY2.Text = "";
            textBoxTime.Text = "";
        }

        private void DrawSegment(int x1, int y1, int x2, int y2)
        {
            GraphPane pane = zedGraphControl1.GraphPane;
            Point2D[] points;

            points = currentAlgo.GetArrayPresentation(new Segment(x1, y1, x2, y2));

            if (points == null || points.Length == 0)
                return;

            foreach (Point2D p in points)
            {
                BoxObj pixel = new BoxObj(p.X, p.Y + 1, 1, 1, Color.Black, Color.Gray);
                pixel.ZOrder = ZOrder.E_BehindCurves;
                pane.GraphObjList.Add(pixel);
            }

            PointPairList list = new PointPairList();
            list.Add(x1, y1);
            list.Add(x2, y2);

            pane.AddCurve("", list, Color.Blue, SymbolType.Circle);

            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
        }

        private long DrawSegments(Segment[] segments)
        {
            long time = 0;
            if (segments == null)
                return time;

            timer.Start();
            for (int i = 0; i < segments.Length; i++)
                DrawSegment(segments[i].FromPoint.X, segments[i].FromPoint.Y, segments[i].ToPoint.X, segments[i].ToPoint.Y);

            timer.Stop();
            time = timer.ElapsedMilliseconds;
            timer.Reset();

            return time;
        }

        
    }
}
