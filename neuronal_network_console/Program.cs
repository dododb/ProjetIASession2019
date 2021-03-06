﻿using ReseauNeuronal.NeuronalNetwork.Reseau;
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
        static int nbIteration = 0;
        //static int nbRow = 2;
        static int nbInputOutput = 100;
        static int bootleNeck = 75;
        static int nbHidden = 0
            ;
        static Random randomGenerator = new Random(7894);
        const int sauvegarRate = 100;
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

        

        static Random RandomNoise = new Random();
        static byte RandomByte(byte b)
        {
            if (RandomNoise.NextDouble() < 0.15)
                return (byte) (b < (256 / 2) ? 255 : 0);
            else return b;
        }

        static IEnumerable<byte> RandomBytes(byte[] bs)
        {
            foreach(var b in bs)
            {
                yield return RandomByte(b);
            }
        }

        static void Main(string[] args)
        {
            //var bytes = File.ReadAllBytes(@"data\arrow.bin");

            var bmpEntree = new List<Bitmap>();
            for (int i = 0; i < 100; i++)
                bmpEntree.Add((Bitmap)Bitmap.FromFile($"trainSet_15\\random_{i}.jpg"));

            var bmpSortie = new List<Bitmap>();
            for (int k = 0; k < 50; k++)
            {
                for (int i = 0; i < 1; i++)
                    bmpSortie.Add((Bitmap)Bitmap.FromFile($"img\\final_left.jpg"));
                for (int i = 0; i < 1; i++)
                    bmpSortie.Add((Bitmap)Bitmap.FromFile($"img\\final_up.jpg"));
            }

            //var colo = bmpEntree.Select(x => Functions.GetAllPixelColo(x).ToArray()).ToArray();
            //var red = colo.Select(x => toto().Concat(x.Select(y => y.R)).ToArray());
            //var green = colo.Select(x => x.Select(y => y.G).ToArray());
            //var blue = colo.Select(x => x.Select(y => y.B).ToArray());

            //var binaryFile = red.Concat(green).Concat(blue).SelectMany(x => x).ToArray();
            //File.WriteAllBytes("data\\arrow.bin", binaryFile);

            //var bmpEntree = new List<Bitmap>() {
            //    (Bitmap)Bitmap.FromFile($"img\\left0.jpg"),
            //    (Bitmap)Bitmap.FromFile($"img\\left1.jpg"),
            //    (Bitmap)Bitmap.FromFile($"img\\left2.jpg"),

            //    (Bitmap)Bitmap.FromFile($"img\\up0.jpg"),
            //    (Bitmap)Bitmap.FromFile($"img\\up1.jpg"),
            //    (Bitmap)Bitmap.FromFile($"img\\up2.jpg"),

            //};
            //var bmpSortie = new List<Bitmap>()
            //{
            //    (Bitmap) Bitmap.FromFile($"img\\final_left.jpg"),
            //    (Bitmap) Bitmap.FromFile($"img\\final_left.jpg"),
            //    (Bitmap) Bitmap.FromFile($"img\\final_left.jpg"),

            //    (Bitmap) Bitmap.FromFile($"img\\final_up.jpg"),
            //    (Bitmap) Bitmap.FromFile($"img\\final_up.jpg"),
            //    (Bitmap) Bitmap.FromFile($"img\\final_up.jpg"),
            //};

            //var bmpEntree = new List<Bitmap>() {
            //    //(Bitmap)Bitmap.FromFile($"img\\left1.jpg"),
            //    (Bitmap)Bitmap.FromFile($"img\\final_left.jpg"),

            //    (Bitmap)Bitmap.FromFile($"img\\final_up.jpg"),

            //};
            //var bmpSortie = new List<Bitmap>()
            //{
            //    (Bitmap) Bitmap.FromFile($"img\\final_left.jpg"),

            //    (Bitmap) Bitmap.FromFile($"img\\final_up.jpg"),
            //};
            var bytesEntree = bmpEntree.Select(x => Functions.GetAllPixelGrey(x).ToArray());


            //var sauvegarde = bytesEntree.Select(x => RandomBytes(x));
            //int k = 0;
            //for (int l = 0; l < 500 / 2; l++)
            //    foreach (var b in sauvegarde)
            //        Functions.SaveImgGrey(b.ToArray(), $"trainSet_15\\random_{(k++)}.jpg");

            //k = 0;
            //for (int l = 0; l < 125 / 2; l++)
            //    foreach (var b in sauvegarde)
            //        Functions.SaveImgGrey(b.ToArray(), $"testSet_15\\random_{(k++)}.jpg");

            var bytesSortie = bmpSortie.Select(x => Functions.GetAllPixelGrey(x).ToArray());
            //var bytesEntree = Functions.SplitBytes(bytes);
            // Functions.SaveImgGrey(bytesEntree.First(), $"testSet\\random_{(k++)}.jpg");
            Console.WriteLine(String.Join(", ", bytesSortie.First()));
            Console.WriteLine(String.Join(", ", bytesEntree.First()));
            var dsEntree = Functions.NormalizeDS(bytesEntree, 255, true).ToArray();
            var dsSortie = Functions.NormalizeDS(bytesSortie, 255, true).ToArray();
            //var dsEntree = GenerateRandomDataset(100, 2).ToArray();
            //var dsSortie = dsEntree;
            for (int i = 0; i< dsEntree.Length; i++)
                Functions.SaveImgGrey(Functions.UnNormalizeRow(dsEntree[i]), @"output\original" + i + ".jpg");

            Stopwatch watch = new Stopwatch();
            INetwork network = null;
            try
            {
                network = AutoEncoderNetwork.GetAutoEncoder(/*$"autoencoder_{nbInputOutput}_{nbHidden}_{bootleNeck}"*/);
            }
            catch
            {
                network = new AutoEncoderNetwork(nbInputOutput, bootleNeck, nbHidden);
            }
            watch.Start();
            for (int i = 1; i <= nbIteration; i++)
            {
                IEnumerable<double> meanSquareErrors = network.Learning(dsEntree, dsSortie).Select(x => Functions.MatriceSquareDifference(x.Item1, x.Item2).Average());

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
            Console.WriteLine();
            Console.WriteLine(string.Join(", ", Functions.UnNormalizeRow(predict.First())));
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

