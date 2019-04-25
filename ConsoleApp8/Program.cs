using Newtonsoft.Json;
using ReseauNeuronal.NeuronalNetwork;
using ReseauNeuronal.NeuronalNetwork.extremite;
using ReseauNeuronal.NeuronalNetwork.IEnumerableExtention;
using ReseauNeuronal.NeuronalNetwork.neurone;
using ReseauNeuronal.NeuronalNetwork.Reseau;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace ReseauNeuronal
{
    class Program
    {
        static int nbIteration = 10_000;
        static int nbRow = 2;
        static int nbInputOutput = 500;
        static int bootleNeck = 50;
        static int nbHidden = 1;
        static Random randomGenerator = new Random(7894);
        const int sauvegarRate = 5_000;
        //static bool startFromZero = true;
        static void Main(string[] args)
        {
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


            var ds = GenerateRandomDataset(nbInputOutput, nbRow).ToArray();
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
