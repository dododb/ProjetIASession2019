using ReseauNeuronal.NeuronalNetwork.Reseau;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Drawing;
using System.Drawing.Imaging;
using ReseauNeuronal.NeuronalNetwork;
using ReseauNeuronal.NeuronalNetwork.IEnumerableExtention;

namespace neuronal_network_console
{
    class Program
    {
        static int nbIteration = 1_000;
        static int nbRow = 2;
        static int nbInputOutput = 20;
        static int bootleNeck = 1;
        static int nbHidden = 1;
        static Random randomGenerator = new Random(7894);
        const int sauvegarRate = 1000;
        //static bool startFromZero = true;
        const string path = @"data\chat.bin";

        static IEnumerable<byte> toto()
        {
            yield return 3;
        }

        static IEnumerable<byte[]> GetCat(byte[] bytes)
        {
            var posCat = bytes.AsParallel().Select((x,y) => new { v = x, pos = y }).Where((x, y) => (y % 3073) == 0).Where(x => x.v==3).Select(x => x.pos);
            var cat = posCat.Select(pos => bytes.Skip(pos).Take(3073).ToArray());
            return cat;
        }

        static IEnumerable<double> MatriceSquareDifference(double[] a, double[] b)
        {
            return a.ZipIteration(b).Select(x => Math.Pow(x.Item1 - x.Item2,2));
        }
        static void Main(string[] args)
        {
            //var bytes = File.ReadAllBytes(@"data\chat.bin");
            //var bmpEntree = new[]{
            //    (Bitmap)Bitmap.FromFile("img\\left0.jpg"),

            //    (Bitmap)Bitmap.FromFile("img\\up0.jpg"),
            //};

            //var bmpSortie = new[]{
            //    (Bitmap)Bitmap.FromFile("img\\final_left.jpg"),

            //    (Bitmap)Bitmap.FromFile("img\\final_up.jpg"),
            //};


            //var bytesEntree = bmpEntree.Select(x => Functions.GetAllPixel(x).ToArray());
            //var bytesSortie = bmpSortie.Select(x => Functions.GetAllPixel(x).ToArray());

            //var dsEntree = Functions.NormalizeDS(bytesEntree, 255, true).ToArray();
            //var dsSortie = Functions.NormalizeDS(bytesSortie, 255, true).ToArray();

            var dsEntree = GenerateRandomDataset(100, 2).ToArray();
            var dsSortie = dsEntree;
            for (int i = 0; i< dsEntree.Length; i++)
                Functions.SaveImgGrey(Functions.UnNormalizeRow(dsEntree[i]), @"output\original" + i + ".jpg");

            Stopwatch watch = new Stopwatch();
            INetwork network = null;
            try
            {
                network = AutoEncoderNetwork.GetAutoEncoder($"autoencoder_{nbInputOutput}_{nbHidden}_{bootleNeck}");
            }
            catch
            {
                network = new AutoEncoderNetwork(nbInputOutput, bootleNeck, nbHidden);
            }
            watch.Start();
            for (int i = 1; i <= nbIteration; i++)
            {
                IEnumerable<double> meanSquareErrors = network.Learning(dsEntree, dsSortie).Select(x => MatriceSquareDifference(x.Item1, x.Item2).Average());

                foreach(var error in meanSquareErrors)
                    if (i % 10 == 0 || i % 11 == 0)
                    {
                        Console.WriteLine(error);
                    }

                if (i % 10 == 0 || i % 11 == 0)
                    Console.WriteLine($"iteration : {i}");
                if (i % sauvegarRate == 0)
                {
                    new Thread(network.Sauvegarde).Start();
                }
            }
            watch.Stop();

            var predict = network.Predict(dsEntree);
            int j = 0;
            foreach (var pred in predict)
            { 
                Functions.SaveImgGrey(Functions.UnNormalizeRow(pred), @"output\img" + j++ + ".jpg");
            }

            Console.WriteLine(string.Join(", ", predict.First().Select(x => Math.Round(x, 2))));
            Console.WriteLine("");
            Console.WriteLine(string.Join(", ", dsEntree.First().Select(x => Math.Round(x, 2))));
            //Functions.SaveImgGrey(Functions.UnNormalizeRow(predict.First()), @"output\img_.jpg");
            Console.WriteLine(watch.Elapsed);
            if (nbIteration % sauvegarRate != 0) network.Sauvegarde();
            Console.Read();
        }

        public static IEnumerable<byte[]> GetChat(byte[] bytes)
        {
            byte[] current = bytes;
            for (int i = 0; i < 10000; i++)
            {
                if(current[0] == 3)
                    yield return current.Take(3073).ToArray();
                current = current.Skip(3073).ToArray();
            }
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
            for (int i = 0; i < nbInput; i++)
                yield return randomGenerator.NextDouble();
        }
    }
}

