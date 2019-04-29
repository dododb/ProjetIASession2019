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
        string[] filesPred;
        string[] fileArrayPred
        {
            get
            {
                return filesPred;
            }
            set
            {
                filesPred = value;
                textBox5.Text = String.Join(";", value);
                //textBox5.Refresh();
            }

        }

        string[] filesTrain;
        string[] fileArrayTrain
        {
            get
            {
                return filesTrain;
            }
            set
            {
                filesTrain = value;
                textBox4.Text = String.Join(";", value);
                //textBox4.Refresh();
            }

        }

        string[] filesReel;
        string[] fileArrayReel
        {
            get
            {
                return filesReel;
            }
            set
            {
                filesReel = value;
                textBox8.Text = String.Join(";", value);
                //textBox8.Refresh();
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

            var bmpEntree = new List<Bitmap>();
            foreach(var file in fileArrayTrain)
                bmpEntree.Add((Bitmap)Bitmap.FromFile(file));

            var bmpSortie = new List<Bitmap>();
            for(int i = 0; i<50; i++)
                foreach (var file in fileArrayReel)
                    bmpSortie.Add((Bitmap)Bitmap.FromFile(file));

            var bytesEntree = bmpEntree.Select(x => Functions.GetAllPixelGrey(x).ToArray());
            var bytesSortie = bmpSortie.Select(x => Functions.GetAllPixelGrey(x).ToArray());

            var dsEntree = Functions.NormalizeDS(bytesEntree, 255, true).ToArray();
            var dsSortie = Functions.NormalizeDS(bytesSortie, 255, true).ToArray();
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

            //int nbRow = Convert.ToInt32(numericUpDown5.Value);
            int nbIteration = Convert.ToInt32(numericUpDown1.Value);
            int sauvegardeRate = Convert.ToInt32(numericUpDown6.Value);
            for (int i = 1; i <= nbIteration; i++)
            {
                IEnumerable<double> meanSquareErrors = network.Learning(dsEntree, dsSortie).Select(x => Functions.MatriceSquareDifference(x.Item1, x.Item2).Average());
                double value = meanSquareErrors.Average();
                    //TextBoxWriteLine(
                    //    $"prediction : [{String.Join(", ", prediction.Select(x => Math.Round(x, 2)))}]" +
                    //    $"\nlabel : [{String.Join(", ", label.Select(x => Math.Round(x, 2)))}]\n");
                Console.WriteLine();
                if (i % sauvegardeRate == 0) new Thread(network.Sauvegarde).Start();
                if(i % 10 == 0)
                {
                    textBox2.Text = i.ToString();
                    textBox7.Text = value.ToString();
                    this.textBox2.Refresh();
                    this.textBox4.Refresh();
                    textBox7.Refresh();

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

            Bitmap imgToDenoise = (Bitmap)Bitmap.FromFile(fileArrayPred[Convert.ToInt32(numericUpDown7.Value)]);

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
                fileArrayPred = openFileDialog1.FileNames;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                var folder = folderBrowserDialog1.SelectedPath;
                fileArrayPred = Directory.GetFiles(folder);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fileArrayTrain = openFileDialog1.FileNames;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                var folder = folderBrowserDialog1.SelectedPath;
                fileArrayTrain = Directory.GetFiles(folder);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fileArrayReel = openFileDialog1.FileNames;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                var folder = folderBrowserDialog1.SelectedPath;
                fileArrayReel = Directory.GetFiles(folder);
            }
        }
    }
}
