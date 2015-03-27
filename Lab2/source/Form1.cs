using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Graphics_Lab2
{
    public partial class Form1 : Form
    {
        private Image currentImage;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            String imagePath = "Sample.jpg";
            currentImage = Image.FromFile(imagePath);
            pictureBox1.Image = currentImage;
            textBox1.Text = imagePath;
            showInfo(currentImage);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                currentImage = Image.FromFile(openFileDialog1.FileName);
                pictureBox1.Image = currentImage;
                textBox1.Text = openFileDialog1.FileName;
                showInfo(currentImage);
            }
        }

        private void showInfo(Image image)
        {
            textBoxWidth.Text = image.PhysicalDimension.Width.ToString();
            textBoxHeight.Text = image.PhysicalDimension.Height.ToString();
            textBoxDPI.Text = image.VerticalResolution.ToString();

            richTextBox1.Text = "[Name] : [Type, Value]\n\n";

            Dictionary<PropertyTagId, KeyValuePair<PropertyTagType, Object>> imageMeta = getInfo(image);

            foreach (KeyValuePair<PropertyTagId, KeyValuePair<PropertyTagType, Object>> property in imageMeta)
            {
                richTextBox1.Text += property.Key.ToString() + ": " + property.Value.ToString() + "\n";
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
