using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;


namespace Grapics_Lab1
{

    public partial class Form1 : Form
    {
        private Color currentColor;
        private int R, G, B, H, L, S, C, M, Y, K, L_, u, v, X, Y_, Z;
        protected static bool colorCut = false;
        protected static bool[] cuttedComponents = { false, false, false };

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Bitmap image = new Bitmap(Image.FromFile("bird.jpg"), pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = image;
            currentColor = Color.FromArgb(0, 0, 0);
            label14.Visible = false; 
        }

        private void refreshAll(ColorRGB color)
        {
            R = trackBarR.Value = color.R;
            G = trackBarG.Value = color.G;
            B = trackBarB.Value = color.B;
            ColorHLS hls = Converter.RGBtoHLS(R, G, B);         //!!!!!
            trackBarH.Value = H = hls.H;
            trackBarL.Value = L = Convert.ToInt32(hls.L * 100.0);
            trackBarS.Value = S = Convert.ToInt32(hls.S * 100.0);
            ColorCMYK cmyk = Converter.RGBtoCMYK(R, G, B);      //!!!!!
            trackBarC.Value = C = Convert.ToInt32(cmyk.C * 100.0);
            trackBarM.Value = M = Convert.ToInt32(cmyk.M * 100.0);
            trackBarY.Value = Y = Convert.ToInt32(cmyk.Y * 100.0);
            trackBarK.Value = K = Convert.ToInt32(cmyk.K * 100.0);
            ColorXYZ xyz = Converter.RGBtoXYZ(R, G, B);         //!!!!!
            trackBarX.Value = X = Convert.ToInt32(xyz.X * 100.0);
            trackBarY_.Value = Y_ = Convert.ToInt32(xyz.Y * 100.0);
            trackBarZ.Value = Z = Convert.ToInt32(xyz.Z * 100.0);
            ColorLuv luv = Converter.RGBtoLuv(R, G, B);           //!!!!
            trackBarL_.Value = L_ = Convert.ToInt32(luv.L);
            trackBarU.Value = u = Convert.ToInt32(luv.u);
            trackBarV.Value = v = Convert.ToInt32(luv.v);

            numericUpDownR.Value = R;
            numericUpDownG.Value = G;
            numericUpDownB.Value = B;
            numericUpDownH.Value = H;
            numericUpDownL.Value = L;
            numericUpDownS.Value = S;
            numericUpDownC.Value = C;
            numericUpDownM.Value = M;
            numericUpDownY.Value = Y;
            numericUpDownK.Value = K;
            numericUpDownL_.Value = L_;
            numericUpDownU.Value = u;
            numericUpDownV.Value = v;
            numericUpDownX.Value = X;
            numericUpDownY_.Value = Y_;
            numericUpDownZ.Value = Z;

            if(colorCut)
            {
                label14.Visible = true;

                if (cuttedComponents[0])
                    numericUpDownR.BackColor = Color.Red;
                else
                    numericUpDownR.BackColor = Color.White;
                if (cuttedComponents[1])
                    numericUpDownG.BackColor = Color.Red;
                else
                    numericUpDownG.BackColor = Color.White;
                if (cuttedComponents[2])
                    numericUpDownB.BackColor = Color.Red;
                else
                    numericUpDownB.BackColor = Color.White;

                colorCut = false;
            }
            else
            {
                label14.Visible = false;

                numericUpDownR.BackColor = Color.White;
                numericUpDownG.BackColor = Color.White;
                numericUpDownB.BackColor = Color.White;
            }

            currentColor = Color.FromArgb(R, G, B);
            panel1_Paint(null, null);
        }

        private void refreshAll(ColorHLS color)
        {
            trackBarH.Value = H = color.H;
            trackBarL.Value = L = Convert.ToInt32(color.L * 100.0);
            trackBarS.Value = S = Convert.ToInt32(color.S * 100.0);
            ColorRGB rgb = Converter.HLStoRGB(color.H, color.L, color.S);     //!!!
            R = trackBarR.Value = rgb.R;
            G = trackBarG.Value = rgb.G;
            B = trackBarB.Value = rgb.B;
            ColorCMYK cmyk = Converter.RGBtoCMYK(R, G, B);           //!!!!!
            trackBarC.Value = C = Convert.ToInt32(cmyk.C * 100.0);
            trackBarM.Value = M = Convert.ToInt32(cmyk.M * 100.0);
            trackBarY.Value = Y = Convert.ToInt32(cmyk.Y * 100.0);
            trackBarK.Value = K = Convert.ToInt32(cmyk.K * 100.0);
            ColorXYZ xyz = Converter.RGBtoXYZ(R, G, B);         //!!!!!
            trackBarX.Value = X = Convert.ToInt32(xyz.X * 100.0);
            trackBarY_.Value = Y_ = Convert.ToInt32(xyz.Y * 100.0);
            trackBarZ.Value = Z = Convert.ToInt32(xyz.Z * 100.0);
            ColorLuv luv = Converter.RGBtoLuv(R, G, B);           //!!!!
            trackBarL_.Value = L_ = Convert.ToInt32(luv.L);
            trackBarU.Value = u = Convert.ToInt32(luv.u);
            trackBarV.Value = v = Convert.ToInt32(luv.v);

            numericUpDownR.Value = R;
            numericUpDownG.Value = G;
            numericUpDownB.Value = B;
            numericUpDownH.Value = H;
            numericUpDownL.Value = L;
            numericUpDownS.Value = S;
            numericUpDownC.Value = C;
            numericUpDownM.Value = M;
            numericUpDownY.Value = Y;
            numericUpDownK.Value = K;
            numericUpDownL_.Value = L_;
            numericUpDownU.Value = u;
            numericUpDownV.Value = v;
            numericUpDownX.Value = X;
            numericUpDownY_.Value = Y_;
            numericUpDownZ.Value = Z;

            if (colorCut)
            {
                label14.Visible = true;

                if (cuttedComponents[0])
                    numericUpDownR.BackColor = Color.Red;
                else
                    numericUpDownR.BackColor = Color.White;
                if (cuttedComponents[1])
                    numericUpDownG.BackColor = Color.Red;
                else
                    numericUpDownG.BackColor = Color.White;
                if (cuttedComponents[2])
                    numericUpDownB.BackColor = Color.Red;
                else
                    numericUpDownB.BackColor = Color.White;

                colorCut = false;
            }
            else
            {
                label14.Visible = false;

                numericUpDownR.BackColor = Color.White;
                numericUpDownG.BackColor = Color.White;
                numericUpDownB.BackColor = Color.White;
            }

            currentColor = Color.FromArgb(R, G, B);
            panel1_Paint(null, null);
        }

        private void refreshAll(ColorCMYK color)
        {
            trackBarC.Value = C = Convert.ToInt32(color.C * 100.0);
            trackBarM.Value = M = Convert.ToInt32(color.M * 100.0);
            trackBarY.Value = Y = Convert.ToInt32(color.Y * 100.0);
            trackBarK.Value = K = Convert.ToInt32(color.K * 100.0);
            ColorRGB rgb = Converter.CMYKtoRGB(color.C, color.M, color.Y, color.K);     //!!!
            R = trackBarR.Value = rgb.R;
            G = trackBarG.Value = rgb.G;
            B = trackBarB.Value = rgb.B;
            ColorHLS hls = Converter.RGBtoHLS(R, G, B);         //!!!!!
            trackBarH.Value = H = hls.H;
            trackBarL.Value = L = Convert.ToInt32(hls.L * 100.0);
            trackBarS.Value = S = Convert.ToInt32(hls.S * 100.0);
            ColorXYZ xyz = Converter.RGBtoXYZ(R, G, B);         //!!!!!
            trackBarX.Value = X = Convert.ToInt32(xyz.X * 100.0);
            trackBarY_.Value = Y_ = Convert.ToInt32(xyz.Y * 100.0);
            trackBarZ.Value = Z = Convert.ToInt32(xyz.Z * 100.0);
            ColorLuv luv = Converter.RGBtoLuv(R, G, B);           //!!!!
            trackBarL_.Value = L_ = Convert.ToInt32(luv.L);
            trackBarU.Value = u = Convert.ToInt32(luv.u);
            trackBarV.Value = v = Convert.ToInt32(luv.v);

            numericUpDownR.Value = R;
            numericUpDownG.Value = G;
            numericUpDownB.Value = B;
            numericUpDownH.Value = H;
            numericUpDownL.Value = L;
            numericUpDownS.Value = S;
            numericUpDownC.Value = C;
            numericUpDownM.Value = M;
            numericUpDownY.Value = Y;
            numericUpDownK.Value = K;
            numericUpDownL_.Value = L_;
            numericUpDownU.Value = u;
            numericUpDownV.Value = v;
            numericUpDownX.Value = X;
            numericUpDownY_.Value = Y_;
            numericUpDownZ.Value = Z;

            if (colorCut)
            {
                label14.Visible = true;

                if (cuttedComponents[0])
                    numericUpDownR.BackColor = Color.Red;
                else
                    numericUpDownR.BackColor = Color.White;
                if (cuttedComponents[1])
                    numericUpDownG.BackColor = Color.Red;
                else
                    numericUpDownG.BackColor = Color.White;
                if (cuttedComponents[2])
                    numericUpDownB.BackColor = Color.Red;
                else
                    numericUpDownB.BackColor = Color.White;

                colorCut = false;
            }
            else
            {
                label14.Visible = false;

                numericUpDownR.BackColor = Color.White;
                numericUpDownG.BackColor = Color.White;
                numericUpDownB.BackColor = Color.White;
            }

            currentColor = Color.FromArgb(R, G, B);
            panel1_Paint(null, null);
        }

        private void refreshAll(ColorLuv color)
        {
            trackBarL_.Value = L_ = Convert.ToInt32(color.L);
            trackBarU.Value = u = Convert.ToInt32(color.u);
            trackBarV.Value = v = Convert.ToInt32(color.v);
            ColorRGB rgb = Converter.LuvtoRGB(color.L, color.u, color.v);     //!!!
            R = trackBarR.Value = rgb.R;
            G = trackBarG.Value = rgb.G;
            B = trackBarB.Value = rgb.B;
            ColorCMYK cmyk = Converter.RGBtoCMYK(R, G, B);        //!!!!
            trackBarC.Value = C = Convert.ToInt32(cmyk.C * 100.0);
            trackBarM.Value = M = Convert.ToInt32(cmyk.M * 100.0);
            trackBarY.Value = Y = Convert.ToInt32(cmyk.Y * 100.0);
            trackBarK.Value = K = Convert.ToInt32(cmyk.K * 100.0);
            ColorHLS hls = Converter.RGBtoHLS(R, G, B);         //!!!!!
            trackBarH.Value = H = hls.H;
            trackBarL.Value = L = Convert.ToInt32(hls.L * 100.0);
            trackBarS.Value = S = Convert.ToInt32(hls.S * 100.0);
            ColorXYZ xyz = Converter.RGBtoXYZ(R, G, B);         //!!!!!
            trackBarX.Value = X = Convert.ToInt32(xyz.X * 100.0);
            trackBarY_.Value = Y_ = Convert.ToInt32(xyz.Y * 100.0);
            trackBarZ.Value = Z = Convert.ToInt32(xyz.Z * 100.0);

            numericUpDownR.Value = R;
            numericUpDownG.Value = G;
            numericUpDownB.Value = B;
            numericUpDownH.Value = H;
            numericUpDownL.Value = L;
            numericUpDownS.Value = S;
            numericUpDownC.Value = C;
            numericUpDownM.Value = M;
            numericUpDownY.Value = Y;
            numericUpDownK.Value = K;
            numericUpDownL_.Value = L_;
            numericUpDownU.Value = u;
            numericUpDownV.Value = v;
            numericUpDownX.Value = X;
            numericUpDownY_.Value = Y_;
            numericUpDownZ.Value = Z;

            if (colorCut)
            {
                label14.Visible = true;

                if (cuttedComponents[0])
                    numericUpDownR.BackColor = Color.Red;
                else
                    numericUpDownR.BackColor = Color.White;
                if (cuttedComponents[1])
                    numericUpDownG.BackColor = Color.Red;
                else
                    numericUpDownG.BackColor = Color.White;
                if (cuttedComponents[2])
                    numericUpDownB.BackColor = Color.Red;
                else
                    numericUpDownB.BackColor = Color.White;

                colorCut = false;
            }
            else
            {
                label14.Visible = false;

                numericUpDownR.BackColor = Color.White;
                numericUpDownG.BackColor = Color.White;
                numericUpDownB.BackColor = Color.White;
            }

            currentColor = Color.FromArgb(R, G, B);
            panel1_Paint(null, null);
        }

        private void refreshAll(ColorXYZ color)
        {
            trackBarX.Value = X = Convert.ToInt32(color.X * 100.0);
            trackBarY_.Value = Y_ = Convert.ToInt32(color.Y * 100.0);
            trackBarZ.Value = Z = Convert.ToInt32(color.Z * 100.0);
            ColorRGB rgb = Converter.XYZtoRGB(color.X, color.Y, color.Z);     //!!!
            R = trackBarR.Value = rgb.R;
            G = trackBarG.Value = rgb.G;
            B = trackBarB.Value = rgb.B;
            ColorCMYK cmyk = Converter.RGBtoCMYK(R, G, B);        //!!!!
            trackBarC.Value = C = Convert.ToInt32(cmyk.C * 100.0);
            trackBarM.Value = M = Convert.ToInt32(cmyk.M * 100.0);
            trackBarY.Value = Y = Convert.ToInt32(cmyk.Y * 100.0);
            trackBarK.Value = K = Convert.ToInt32(cmyk.K * 100.0);
            ColorHLS hls = Converter.RGBtoHLS(R, G, B);         //!!!!!
            trackBarH.Value = H = hls.H;
            trackBarL.Value = L = Convert.ToInt32(hls.L * 100.0);
            trackBarS.Value = S = Convert.ToInt32(hls.S * 100.0);
            ColorLuv luv = Converter.RGBtoLuv(R, G, B);           //!!!!
            trackBarL_.Value = L_ = Convert.ToInt32(luv.L);
            trackBarU.Value = u = Convert.ToInt32(luv.u);
            trackBarV.Value = v = Convert.ToInt32(luv.v);
            
            numericUpDownR.Value = R;
            numericUpDownG.Value = G;
            numericUpDownB.Value = B;
            numericUpDownH.Value = H;
            numericUpDownL.Value = L;
            numericUpDownS.Value = S;
            numericUpDownC.Value = C;
            numericUpDownM.Value = M;
            numericUpDownY.Value = Y;
            numericUpDownK.Value = K;
            numericUpDownL_.Value = L_;
            numericUpDownU.Value = u;
            numericUpDownV.Value = v;
            numericUpDownX.Value = X;
            numericUpDownY_.Value = Y_;
            numericUpDownZ.Value = Z;

            if (colorCut)
            {
                label14.Visible = true;

                if (cuttedComponents[0])
                    numericUpDownR.BackColor = Color.Red;
                else
                    numericUpDownR.BackColor = Color.White;
                if (cuttedComponents[1])
                    numericUpDownG.BackColor = Color.Red;
                else
                    numericUpDownG.BackColor = Color.White;
                if (cuttedComponents[2])
                    numericUpDownB.BackColor = Color.Red;
                else
                    numericUpDownB.BackColor = Color.White;

                colorCut = false;
            }
            else
            {
                label14.Visible = false;

                numericUpDownR.BackColor = Color.White;
                numericUpDownG.BackColor = Color.White;
                numericUpDownB.BackColor = Color.White;
            }

            currentColor = Color.FromArgb(R, G, B);
            panel1_Paint(null, null);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            SolidBrush myBrush = new SolidBrush(currentColor);
            Graphics graphics = panel1.CreateGraphics();
            graphics.FillRectangle(myBrush, new Rectangle(0, 0, panel1.Width, panel1.Height));
            myBrush.Dispose();
            graphics.Dispose();
        }

        private void pictureBox1_Click(object sender, MouseEventArgs e)
        {
            currentColor = ((Bitmap)pictureBox1.Image).GetPixel(e.X, e.Y);
            refreshAll(new ColorRGB(currentColor.R, currentColor.G, currentColor.B));
        }

        private void RGB_Scroll(object sender, EventArgs e)
        {
            R = trackBarR.Value;
            G = trackBarG.Value;
            B = trackBarB.Value;
            refreshAll(new ColorRGB(R, G, B));
        }

        private void HLS_Scroll(object sender, EventArgs e)
        {
            H = trackBarH.Value;
            L = trackBarL.Value;
            S = trackBarS.Value;
            if (H == 360)
            {
                H = 0;
                Cursor.Position = new Point(Cursor.Position.X - 89, Cursor.Position.Y);
            }
            if (H == -1)
            {
                H = 359;
                Cursor.Position = new Point(Cursor.Position.X + 89, Cursor.Position.Y);
            }
            refreshAll(new ColorHLS(H, L / 100.0, S / 100.0));
        }

        private void CMYK_Scroll(object sender, EventArgs e)
        {
            C = trackBarC.Value; 
            M = trackBarM.Value;
            Y = trackBarY.Value;
            K = trackBarK.Value;
            refreshAll(new ColorCMYK(C / 100.0, M / 100.0, Y / 100.0, K / 100.0));
        }

        private void Luv_Scroll(object sender, EventArgs e)
        {
            L_ = trackBarL_.Value; 
            u = trackBarU.Value;
            v = trackBarV.Value;
            refreshAll(new ColorLuv(L_, u, v));
        }

        private void XYZ_Scroll(object sender, EventArgs e)
        {
            X = trackBarX.Value;
            Y_ = trackBarY_.Value;
            Z = trackBarZ.Value;
            refreshAll(new ColorXYZ(X / 100.0, Y_ / 100.0, Z / 100.0));
        }

        private void numericRGB_mouseUp(object sender, MouseEventArgs e)
        {
            R = Convert.ToInt32(numericUpDownR.Value);
            G = Convert.ToInt32(numericUpDownG.Value);
            B = Convert.ToInt32(numericUpDownB.Value);
            refreshAll(new ColorRGB(R, G, B));
        }

        private void numericRGB_keyUp(object sender, KeyEventArgs e)
        {
            R = Convert.ToInt32(numericUpDownR.Value);
            G = Convert.ToInt32(numericUpDownG.Value);
            B = Convert.ToInt32(numericUpDownB.Value);
            refreshAll(new ColorRGB(R, G, B));
        }

        private void numericHLS_mouseUp(object sender, MouseEventArgs e)
        {
            H = Convert.ToInt32(numericUpDownH.Value);
            L = Convert.ToInt32(numericUpDownL.Value);
            S = Convert.ToInt32(numericUpDownS.Value);
            if (H == 360)
                H = 0;
            if (H == -1)
                H = 359;
            refreshAll(new ColorHLS(H, L / 100.0, S / 100.0));
        }

        private void numericHLS_keyUp(object sender, KeyEventArgs e)
        {
            H = Convert.ToInt32(numericUpDownH.Value);
            L = Convert.ToInt32(numericUpDownL.Value);
            S = Convert.ToInt32(numericUpDownS.Value);
            if (H == 360)
                H = 0;
            if (H == -1)
                H = 359;
            refreshAll(new ColorHLS(H, L / 100.0, S / 100.0));
        }

        private void numericCMYK_mouseUp(object sender, MouseEventArgs e)
        {
            C = Convert.ToInt32(numericUpDownC.Value);
            M = Convert.ToInt32(numericUpDownM.Value);
            Y = Convert.ToInt32(numericUpDownY.Value);
            K = Convert.ToInt32(numericUpDownK.Value);
            refreshAll(new ColorCMYK(C / 100.0, M / 100.0, Y / 100.0, K / 100.0));
        }

        private void numericCMYK_keyUp(object sender, KeyEventArgs e)
        {
            C = Convert.ToInt32(numericUpDownC.Value);
            M = Convert.ToInt32(numericUpDownM.Value);
            Y = Convert.ToInt32(numericUpDownY.Value);
            K = Convert.ToInt32(numericUpDownK.Value);
            refreshAll(new ColorCMYK(C / 100.0, M / 100.0, Y / 100.0, K / 100.0));
        }

        private void numericLuv_mouseUp(object sender, MouseEventArgs e)
        {
            L_ = Convert.ToInt32(numericUpDownL_.Value);
            u = Convert.ToInt32(numericUpDownU.Value);
            v = Convert.ToInt32(numericUpDownV.Value);
            refreshAll(new ColorLuv(L_, u, v));
        }

        private void numericLuv_keyUp(object sender, KeyEventArgs e)
        {
            L_ = Convert.ToInt32(numericUpDownL_.Value);
            u = Convert.ToInt32(numericUpDownU.Value);
            v = Convert.ToInt32(numericUpDownV.Value);
            refreshAll(new ColorLuv(L_, u, v));
        }

        private void numericXYZ_mouseUp(object sender, MouseEventArgs e)
        {
            X = Convert.ToInt32(numericUpDownX.Value);
            Y_ = Convert.ToInt32(numericUpDownY_.Value);
            Z = Convert.ToInt32(numericUpDownZ.Value);
            refreshAll(new ColorXYZ(X / 100.0, Y_ / 100.0, Z / 100.0));
        }

        private void numericXYZ_keyUp(object sender, KeyEventArgs e)
        {
            X = Convert.ToInt32(numericUpDownX.Value);
            Y_ = Convert.ToInt32(numericUpDownY_.Value);
            Z = Convert.ToInt32(numericUpDownZ.Value);
            refreshAll(new ColorXYZ(X / 100.0, Y_ / 100.0, Z / 100.0));
        }

        private void pictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Bitmap image = new Bitmap(Image.FromFile(openFileDialog1.FileName), pictureBox1.Width, pictureBox1.Height);
                pictureBox1.Image = image;
            }
        }
    }


    public class ColorRGB
    {
        public int R;
        public int G;
        public int B;

        public ColorRGB(int r, int g, int b)
        {
            R = r;
            G = g;
            B = b;
        }
    }

    public class ColorHLS
    {
        public int H;
        public double L;
        public double S;

        public ColorHLS(int h, double l, double s)
        {
            H = h;
            L = l;
            S = s;
        }
    }

    public class ColorCMYK
    {
        public double C, M, Y, K;

        public ColorCMYK(double c, double m, double y, double k)
        {
            C = c;
            M = m;
            Y = y;
            K = k;
        }
    }

    public class ColorXYZ
    {
        public double X, Y, Z;

        public ColorXYZ(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
    public class ColorLuv
    {
        public double L, u, v;

        public ColorLuv(double l, double u, double v)
        {
            L = l;
            this.u = u;
            this.v = v;
        }
    }
}
