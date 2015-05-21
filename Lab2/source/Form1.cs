using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;

namespace Graphics_Lab2
{
    public partial class Form1 : Form
    {
        private Image currentImage;
        private StreamWriter outFile;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            String imagePath = "Sample.tif";
            currentImage = Image.FromFile(imagePath);
            pictureBox1.Image = currentImage;
            textBox1.Text = imagePath;
            showInfo(currentImage, false);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                currentImage = Image.FromFile(openFileDialog1.FileName);
                pictureBox1.Image = currentImage;
                textBox1.Text = openFileDialog1.FileName;
                showInfo(currentImage, false);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ShowFolderInfo();
        }

        private void ShowFolderInfo()
        {
            Stopwatch timer = new Stopwatch();
            outFile = new StreamWriter("ImageInfo.txt");

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string[] files = Directory.GetFiles(folderBrowserDialog1.SelectedPath);
                int imageCount = files.Length;
                String currentFile = null;

                if (imageCount == 0)
                    return;

                WriteToTextbox("Processing images..." + Environment.NewLine);

                timer.Start();
                foreach (String file in files)
                {
                    if (file.EndsWith(".jpg", true, null) || file.EndsWith(".jpeg", true, null) || file.EndsWith(".png", true, null) || file.EndsWith(".bmp", true, null) || file.EndsWith(".tif", true, null) || file.EndsWith(".gif", true, null))
                    {
                        currentFile = file;
                        outFile.Write(Environment.NewLine + file + Environment.NewLine + Environment.NewLine);
                        showInfo(Image.FromFile(file), true);
                    }
                }
                timer.Stop();

                if (currentFile != null)
                    pictureBox1.Image = Image.FromFile(currentFile);

                AppendToTextbox("Images info saved to file \"ImageInfo.txt\"" + Environment.NewLine);
                MessageBox.Show("Image count: " + imageCount + "\nTime: " + timer.ElapsedMilliseconds + " ms.");
                outFile.Close();
            }
        }

        private void WriteToTextbox(String message)
        {
            textBoxInfo.BeginInvoke(
                    new Action(() =>
                    {
                        textBoxInfo.Text = message;
                    }
                ));
        }

        private void AppendToTextbox(String message)
        {
            textBoxInfo.BeginInvoke(
                    new Action(() =>
                    {
                        textBoxInfo.Text += message;
                    }
                ));
        }

        private void showInfo(Image image, bool forFolder)
        {
            textBoxWidth.Text = image.PhysicalDimension.Width.ToString();
            textBoxHeight.Text = image.PhysicalDimension.Height.ToString();
            textBoxDPI.Text = ((int)(image.VerticalResolution)).ToString();
            richTextBox2.Text = "";
            richTextBox3.Text = "";

            if(!forFolder)
                textBoxInfo.Text = "[Name] : [Type, Value]" + Environment.NewLine + Environment.NewLine;

            Dictionary<PropertyTagId, KeyValuePair<PropertyTagType, Object>> imageMeta = getInfo(image);

            foreach (KeyValuePair<PropertyTagId, KeyValuePair<PropertyTagType, Object>> property in imageMeta)
            {
                if (forFolder)
                    outFile.Write(property.Key.ToString() + ": " + property.Value.ToString() + Environment.NewLine);
                else
                    textBoxInfo.Text += property.Key.ToString() + ": " + property.Value.ToString() + Environment.NewLine;
            }

            if (imageMeta.Keys.Contains(NumToEnum<PropertyTagId>(0x0103)))
                textBoxCompression.Text = imageMeta[NumToEnum<PropertyTagId>(0x0103)].Value.ToString();
            else
                textBoxCompression.Text = "Undefined.";

            richTextBox2.Text = getChrominanceTable(currentImage);
            richTextBox3.Text = getLuminanceTable(currentImage);
        }


        private String getChrominanceTable(Image image)
        {
            String result = "";

            foreach (PropertyItem property in image.PropertyItems)
            {
                if (property.Id == 0x5091)
                {
                    for(int i = 0; i < 8; i++)
                    {
                        for (int j = 0; j < 8; j++)
                            result += String.Format("{0,6:X} ", property.Value[i * 16 + j]);
                        result += "\n";
                    }
                }
            }

            return result;
        }


        private String getLuminanceTable(Image image)
        {
            String result = "";

            foreach (PropertyItem property in image.PropertyItems)
            {
                if (property.Id == 0x5090)
                {
                    for(int i = 0; i < 8; i++)
                    {
                        for (int j = 0; j < 8; j++)
                            result += String.Format("{0,6:X} ", property.Value[i * 16 + j]);
                        result += "\n";
                    }
                }
            }

            return result;
        }

        public Dictionary<PropertyTagId, KeyValuePair<PropertyTagType, Object>> getInfo(Image image)
        {
            Dictionary<PropertyTagId, KeyValuePair<PropertyTagType, Object>> returnImageProps =
                new Dictionary<PropertyTagId, KeyValuePair<PropertyTagType, Object>>();

            foreach (PropertyItem property in image.PropertyItems)
            {
                Object propValue = new Object();
                switch ((PropertyTagType)property.Type)
                {
                    case PropertyTagType.ASCII:
                        ASCIIEncoding encoding = new ASCIIEncoding();
                        propValue = encoding.GetString(property.Value, 0, property.Len - 1);
                        break;
                    case PropertyTagType.Int16:
                        propValue = BitConverter.ToInt16(property.Value, 0);
                        if(property.Id == 0x103) //Get compression
                        {
                            if(Convert.ToInt32(propValue) == 2)
                                propValue = "CCITT Group 3";
                            else if(Convert.ToInt32(propValue) == 3)
                                propValue = "Facsimile-compatible CCITT Group 3";
                            else if (Convert.ToInt32(propValue) == 4)
                                propValue = "CCITT Group 4 (T.6)";
                            else if (Convert.ToInt32(propValue) == 5)
                                propValue = "LZW";
                            else
                                propValue = "No compression";
                        }
                        break;
                    case PropertyTagType.SLONG:
                    case PropertyTagType.Int32:
                        propValue = BitConverter.ToInt32(property.Value, 0);
                        break;
                    case PropertyTagType.SRational:
                    case PropertyTagType.Rational:
                        UInt32 numberator = BitConverter.ToUInt32(property.Value, 0);
                        UInt32 denominator = BitConverter.ToUInt32(property.Value, 4);
                        try
                        {
                            propValue = ((double)numberator / (double)denominator).ToString();

                            if (propValue.ToString() == "NaN")
                                propValue = "0";
                        }
                        catch (DivideByZeroException)
                        {
                            propValue = "0";
                        }
                        break;
                    case PropertyTagType.Undefined:
                        propValue = "Undefined Data";
                        break;
                }
                returnImageProps.Add(NumToEnum<PropertyTagId>(property.Id),
                    new KeyValuePair<PropertyTagType, object>(NumToEnum<PropertyTagType>(property.Type), propValue));
            }

            return returnImageProps;
        }

        public T NumToEnum<T>(int number)
        {
            return (T)Enum.ToObject(typeof(T), number);
        }
    }
}
