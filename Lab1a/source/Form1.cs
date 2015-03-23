using System;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Graphics_Lab2
{
    public partial class Form1 : Form
    {
        private int barChartHeight = 120;
        private int barChartWidth = 256;
        private Image currentImage;
        private Image modifiedImage;
        private bool fromTrackbar1 = false;
        private bool fromTrackbar2 = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            String imagePath = "Sample.jpg";
            currentImage = new Bitmap(Image.FromFile(imagePath), pictureBox1.Width, pictureBox1.Height);
            Bitmap br = new Bitmap(Image.FromFile("brightness.png"), pictureBoxBrightness.Width, pictureBoxBrightness.Height);
            Bitmap cont = new Bitmap(Image.FromFile("contrast.png"), pictureBoxContrast.Width, pictureBoxContrast.Height);
            pictureBox1.Image = currentImage;
            pictureBoxBrightness.Image = br;
            pictureBoxContrast.Image = cont;

            pictureBoxR.Height = pictureBoxG.Height = pictureBoxB.Height = barChartHeight;
            pictureBoxR.Width = pictureBoxG.Width = pictureBoxB.Width = barChartWidth;
            textBox1.Text = imagePath;
            buttonInfo.PerformClick();
        }

        private void cleanAll()
        {
            textBoxR.Text = textBoxG.Text = textBoxB.Text = "";
            pictureBoxA.Image = new Bitmap(1, 1);
            pictureBoxR.Image = new Bitmap(1, 1);
            pictureBoxG.Image = new Bitmap(1, 1);
            pictureBoxB.Image = new Bitmap(1, 1);
            trackBar1.Value = 0;
            trackBar2.Value = 0;
        }

        private void buttonChangeImage_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                currentImage = new Bitmap(Image.FromFile(openFileDialog1.FileName), pictureBox1.Width, pictureBox1.Height);
                pictureBox1.Image = currentImage;
                textBox1.Text = openFileDialog1.FileName;
                cleanAll();
                buttonInfo.PerformClick();
            }
        }

        private void buttonInfo_Click(object sender, EventArgs e)
        {
            Bitmap[] barCharts = ChartMaker.CalculateBarChart(pictureBox1.Image, barChartHeight, barChartWidth);
            double[] avg = ChartMaker.CalculateAverageRGB(pictureBox1.Image);
            if (barCharts != null)
            {
                pictureBoxA.Image = barCharts[0];
                pictureBoxR.Image = barCharts[1];
                pictureBoxG.Image = barCharts[2];
                pictureBoxB.Image = barCharts[3];
            }

            if(avg != null)
            {
                textBoxR.Text = "Average: " + avg[0];
                textBoxG.Text = "Average: " + avg[1];
                textBoxB.Text = "Average: " + avg[2];
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            Image image;

            if (trackBar2.Value != 0)
                image = Filter.changeBrightness(modifiedImage, trackBar1.Value);
            else
                modifiedImage = image = Filter.changeBrightness(currentImage, trackBar1.Value);

            if(sender != null)
                fromTrackbar1 = true;
           
            numericUpDown1.Value = trackBar1.Value;
            pictureBox1.Image = image;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            Image image;
            if (trackBar1.Value != 0)
                image = Filter.changeContrast(modifiedImage, trackBar2.Value);
            else
                modifiedImage = image = Filter.changeContrast(currentImage, trackBar2.Value);

            if (sender != null)
                fromTrackbar2 = true;

            numericUpDown2.Value = trackBar2.Value;
            pictureBox1.Image = image;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (fromTrackbar1)
                fromTrackbar1 = false;
            else
            {
                trackBar1.Value = Convert.ToInt32(numericUpDown1.Value);
                trackBar1_Scroll(null, null);
            }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (fromTrackbar2)
                fromTrackbar2 = false;
            else
            {
                trackBar2.Value = Convert.ToInt32(numericUpDown2.Value);
                trackBar2_Scroll(null, null);
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("RGB Charts\nVersion 1.0.0.0\n© 2015 Lobur Alexander.\nAll rights reserved.", "About RGB Charts", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


    }
}
