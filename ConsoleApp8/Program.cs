using Newtonsoft.Json;
using ReseauNeuronal.NeuronalNetwork;
using ReseauNeuronal.NeuronalNetwork.extremite;
using ReseauNeuronal.NeuronalNetwork.IEnumerableExtention;
using ReseauNeuronal.NeuronalNetwork.neurone;
using ReseauNeuronal.NeuronalNetwork.Reseau;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;

namespace ReseauNeuronal
{
    class Program
    {
        static int nbIteration = 10_000;
        static int nbRow = 2;
        static int nbInputOutput = 100;
        static int bootleNeck = 50;
        static int nbHidden = 0;
        static Random randomGenerator = new Random(7894);
        const int sauvegarRate = 5_000;
        //static bool startFromZero = true;


        static IEnumerable<double> MatriceSquareDifference(double[] a, double[] b)
        {
            return a.ZipIteration(b).Select(x => Math.Pow(x.Item1 - x.Item2, 2));
        }
        static void Main(string[] args)
        {
            Stopwatch watch = new Stopwatch();

            var bmpEntree = new[]{
                (Bitmap)Bitmap.FromFile("img\\left0.jpg"),
                (Bitmap)Bitmap.FromFile("img\\left1.jpg"),
                (Bitmap)Bitmap.FromFile("img\\left2.jpg"),

                (Bitmap)Bitmap.FromFile("img\\up0.jpg"),
                (Bitmap)Bitmap.FromFile("img\\up1.jpg"),
                (Bitmap)Bitmap.FromFile("img\\up2.jpg"),

            };

            var bmpSortie = new[]{
                (Bitmap)Bitmap.FromFile("img\\final_left.jpg"),
                (Bitmap)Bitmap.FromFile("img\\final_left.jpg"),
                (Bitmap)Bitmap.FromFile("img\\final_left.jpg"),

                (Bitmap)Bitmap.FromFile("img\\final_up.jpg"),
                (Bitmap)Bitmap.FromFile("img\\final_up.jpg"),
                (Bitmap)Bitmap.FromFile("img\\final_up.jpg"),
            };


            var bytesEntree = bmpEntree.Select(x => Functions.GetAllPixel(x).ToArray());
            var bytesSortie = bmpSortie.Select(x => Functions.GetAllPixel(x).ToArray());

            var dsEntree = Functions.NormalizeDS(bytesEntree, 255, true).ToArray();
            var dsSortie = Functions.NormalizeDS(bytesSortie, 255, true).ToArray();


            INetwork network = null;
            try
            {
                network = AutoEncoderNetwork.GetAutoEncoder($"autoencoder_{nbInputOutput}_{nbHidden}_{bootleNeck}");
            }
            catch
            {
                network = new AutoEncoderNetwork(nbInputOutput, bootleNeck, nbHidden);
            }
            //if(startFromZero)

            //else


            var ds = GenerateRandomDataset(nbInputOutput, nbRow).ToArray();
            watch.Start();
            for (int i = 1; i <= nbIteration; i++)
            {
                Console.WriteLine($"iteration : {i}");
                IEnumerable<double> meanSquareErrors = network.Learning(ds, ds).Select(x => MatriceSquareDifference(x.Item1, x.Item2).Average());
                foreach(var error in meanSquareErrors)
                    Console.WriteLine(error);
                //foreach (var (prediction, label) in network.Learning(ds, ds))
                //    Console.WriteLine(
                //        $"prediction : [{String.Join(", ", prediction.Select(x => Math.Round(x, 2)))}]" +
                //        $"\nlabel : [{String.Join(", ", label.Select(x => Math.Round(x, 2)))}]\n");
                Console.WriteLine();
                if (i % sauvegarRate == 0) new Thread(network.Sauvegarde).Start();
            }
            watch.Stop();

            Console.WriteLine(watch.Elapsed);
            if(nbIteration % sauvegarRate != 0) network.Sauvegarde();
            Console.Read();
        }

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
            for(int i = 0; i < nbInput; i++)
                yield return randomGenerator.NextDouble();
        }
    }
}
