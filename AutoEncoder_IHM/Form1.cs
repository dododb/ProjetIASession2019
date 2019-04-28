using ReseauNeuronal.NeuronalNetwork;
using ReseauNeuronal.NeuronalNetwork.Reseau;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoEncoder_IHM
{
    public partial class Form1 : Form
    {
        string[] files;
        string[] fileArray
        {
            get
            {
                return files;
            }
            set
            {
                files = value;
                textBox5.Text = String.Join(";", value);
            }

        }
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //const int sizeImage = 1024;
            this.Enabled = false;

            int nbEntree = Convert.ToInt32(this.numericUpDown2.Value);
            int nbBottleNeck = Convert.ToInt32(numericUpDown3.Value);
            int nbHidden = Convert.ToInt32(numericUpDown4.Value);

            var bytes = File.ReadAllBytes(textBox3.Text);
            var ds = Functions.NormalizeDS(Functions.SplitBytes(bytes)).ToArray();
            INetwork network = null;
            try
            {
                string reseauxName = textBox1.Text == "" ?
                    $"autoencoder_{nbEntree}_{nbHidden}_{nbBottleNeck}"
                    : textBox1.Text;
                network = AutoEncoderNetwork.GetAutoEncoder(reseauxName);
            }
            catch
            {
                network = new AutoEncoderNetwork(nbEntree, nbBottleNeck, nbHidden);
            }

            int nbRow = Convert.ToInt32(numericUpDown5.Value);
            int nbIteration = Convert.ToInt32(numericUpDown1.Value);
            int sauvegardeRate = Convert.ToInt32(numericUpDown6.Value);
            for (int i = 1; i <= nbIteration; i++)
            {
                foreach (var (prediction, label) in network.Learning(ds, ds)) ;
                    //TextBoxWriteLine(
                    //    $"prediction : [{String.Join(", ", prediction.Select(x => Math.Round(x, 2)))}]" +
                    //    $"\nlabel : [{String.Join(", ", label.Select(x => Math.Round(x, 2)))}]\n");
                Console.WriteLine();
                if (i % sauvegardeRate == 0) new Thread(network.Sauvegarde).Start();
                if(i % 100 == 0)
                {
                    textBox2.Text = i.ToString();
                    this.textBox2.Refresh();
                    this.textBox4.Refresh();
                }
            }
            this.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Enabled = false;

            int nbEntree = Convert.ToInt32(this.numericUpDown2.Value);
            int nbBottleNeck = Convert.ToInt32(numericUpDown3.Value);
            int nbHidden = Convert.ToInt32(numericUpDown4.Value);

            Bitmap imgToDenoise = (Bitmap)Bitmap.FromFile(fileArray[Convert.ToInt32(numericUpDown7.Value)]);

            var bytes = Functions.GetAllPixelGrey(imgToDenoise).ToArray();
            var img = Functions.NormalizeRow(bytes, 255, true);

            INetwork network = null;
            try
            {
                string reseauxName = textBox6.Text == "" ?
                    $"autoencoder_{nbEntree}_{nbHidden}_{nbBottleNeck}"
                    : textBox6.Text;
                network = AutoEncoderNetwork.GetAutoEncoder(reseauxName);
            }
            catch
            {
                Console.WriteLine("error network doesn't exist");
            }

            var imgOut = network.Predict(img);
            Functions.SaveImgGrey(Functions.UnNormalizeRow(img), @"output\displayedIn.jpg");
            Functions.SaveImgGrey(bytes, @"output\displayedInOrigin.jpg");
            Functions.SaveImgGrey(Functions.UnNormalizeRow(imgOut), @"output\displayedOut.jpg");

            SetPictureBox(pictureBox1, @"output\displayedIn.jpg");
            SetPictureBox(pictureBox2, @"output\displayedOut.jpg");

            this.Enabled = true;
        }

        void SetPictureBox(PictureBox pic, string imgPath)
        {
            pic.ImageLocation = imgPath;
            pic.SizeMode = PictureBoxSizeMode.Zoom;
            pic.Refresh();
        }

        void TextBoxWriteLine(string text)
        {
            textBox4.Text = text + "\n";
        }

        static Random randomGenerator = new Random(7894);
        public static IEnumerable<double[]> GenerateRandomDataset(int nbInput, int nbRow)
        {
            for (int i = 0; i < nbRow; i++)
            {
                var input = GenerateRandomVector(nbInput).ToArray();
                yield return input;
            }
        }

        public static IEnumerable<double> GenerateRandomVector(int nbInput)
        {
            for (int i = 0; i < nbInput; i++)
                yield return randomGenerator.NextDouble();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fileArray = openFileDialog1.FileNames;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                var folder = folderBrowserDialog1.SelectedPath;
                fileArray = Directory.GetFiles(folder);
            }
        }
    }
}
