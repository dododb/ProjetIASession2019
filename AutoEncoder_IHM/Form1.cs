using ReseauNeuronal.NeuronalNetwork.Reseau;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoEncoder_IHM
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Enabled = false;

            int nbEntree = Convert.ToInt32(this.numericUpDown2.Value);
            int nbBottleNeck = Convert.ToInt32(numericUpDown3.Value);
            int nbHidden = Convert.ToInt32(numericUpDown4.Value);

            
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
            var ds = GenerateRandomDataset(nbEntree, nbRow).ToArray();
            for (int i = 1; i <= nbIteration; i++)
            {
                textBox2.Text = i.ToString();
                foreach (var (prediction, label) in network.Learning(ds, ds))
                    TextBoxWriteLine(
                        $"prediction : [{String.Join(", ", prediction.Select(x => Math.Round(x, 2)))}]" +
                        $"\nlabel : [{String.Join(", ", label.Select(x => Math.Round(x, 2)))}]\n");
                Console.WriteLine();
                if (i % sauvegardeRate == 0) new Thread(network.Sauvegarde).Start();
                if(i % 100 == 0)
                {
                    this.textBox2.Refresh();
                    this.textBox4.Refresh();
                }
            }
            this.Enabled = true;
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
    }
}
