using ReseauNeuronal.NeuronalNetwork.Reseau;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Drawing;
using System.Drawing.Imaging;

namespace neuronal_network_console
{
    class Program
    {
        static int nbIteration = 0;
        static int nbRow = 2;
        static int nbInputOutput = 1024;
        static int bootleNeck = 500;
        static int nbHidden = 1;
        static Random randomGenerator = new Random(7894);
        const int sauvegarRate = 1_000;
        //static bool startFromZero = true;
        const string path = @"data\data_batch_1.bin";
        static void Main(string[] args)
        {
            var bytes = File.ReadAllBytes(path);
            Console.WriteLine(bytes.Length);

            var ds = Normalize(SplitBytes(bytes)).ToArray();
            SaveImg(UnNormalizeRow(ds[0]), @"output\original.jpg");
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
            //if(startFromZero)

            //else


            //var ds = GenerateRandomDataset(nbInputOutput, nbRow).ToArray();
            watch.Start();
            for (int i = 1; i <= nbIteration; i++)
            {
                Console.WriteLine($"iteration : {i}");
                foreach (var (prediction, label) in network.Learning(ds, ds))
                    Console.WriteLine(
                        $"prediction : [{String.Join(", ", prediction.Select(x => Math.Round(x, 2)))}]" +
                        $"\nlabel : [{String.Join(", ", label.Select(x => Math.Round(x, 2)))}]\n");
                Console.WriteLine();
                if (i % sauvegarRate == 0) new Thread(network.Sauvegarde).Start();
            }
            watch.Stop();

            var predict = network.Predict(ds[0]);
            var unNormalizedPrediction = UnNormalizeRow(predict);
            SaveImg(unNormalizedPrediction);
            Console.WriteLine(watch.Elapsed);
            if (nbIteration % sauvegarRate != 0) network.Sauvegarde();
            Console.Read();
        }

        public static IEnumerable<byte[]> SplitBytes(byte[] bytes, int splitNumber = 2, int size = 3072, int lengthLabel = 1)
        {
            int min = Math.Min(bytes.Length/(size+1), splitNumber);
            byte[] current = bytes;
            for(int i = 0; i< min; i++)
            {
                current = current.Skip(lengthLabel).ToArray();
                yield return ShadesOfGray(current.Take(size).ToArray());
            }
        }

        /// <summary>
        /// on assum les dimmension etc.. (32*32) 3 couleurs
        /// </summary>
        /// <param name="bytes"></param>
        public static byte[] ShadesOfGray(byte[] bytes)
        {
            var output = new byte[bytes.Length / 3];
            for(int i = 0; i< output.Length; i++)
            {
                output[i] = Convert.ToByte((bytes[i] + bytes[i + 1024] + bytes[i + 2 * 1024])/3);
            }
            return output;
        }

        public static IEnumerable<double[]> Normalize(IEnumerable<byte[]> bytes, int minValue = 0, int maxValue = 255)
        {
            foreach(var bytesArray in bytes)
            {
                yield return bytesArray.Select(x => (double)x / maxValue).ToArray();
            }
        }

        public static IEnumerable<byte[]> UnNormalizeDs(double[][] outputs, int minValue = 0, int maxValue = 255)
        {
            foreach(var output in outputs)
            {
                yield return UnNormalizeRow(output);
            }
        }

        public static byte[] UnNormalizeRow(double[] outputs, int minValue = 0, int maxValue = 255)
        {
            return outputs.Select(x => Convert.ToByte(x * maxValue)).ToArray();
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

        public static void SaveImg(byte[] bytes, string name = @"output\img.jpg")
        {
            Bitmap empty = new Bitmap(32, 32);
            for (int y = 0; y < 32; y++)
            {
                for (int x = 0; x < 32; x++)
                {
                    var pos = y + x * 32;
                    var grey = bytes[pos];
                    Color c = Color.FromArgb(grey, grey, grey);
                    empty.SetPixel(x, y, c);
                }
            }
            empty.Save(name);
        }
    }
}

