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

namespace ReseauNeuronal
{
    class Program
    {
        static int nbIteration = 300_000;
        static int nbRow = 2;
        static int nbInputOutput = 2;
        static int bootleNeck = 2;
        static int nbHidden = 1;
        static Random randomGenerator = new Random(5645);

        static bool startFromZero = false;
        static void Main(string[] args)
        {
            Stopwatch watch = new Stopwatch();
            INetwork network = null;
            if(startFromZero)
                network = new AutoEncoderNetwork(nbInputOutput, bootleNeck, nbHidden);
            else
            {
                var n1 = File.ReadAllText(@"toto.aed1");
                var n2 = File.ReadAllText(@"toto.aed2");
                network = AutoEncoderNetwork.GetAutoEncoder(new[] { n1, n2 });
            }
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
            }
            watch.Stop();
            Console.WriteLine(watch.Elapsed);
            var saved = network.Sauvegarde();

            File.WriteAllText(@"toto.aed1", saved[0]);
            File.WriteAllText(@"toto.aed2", saved[1]);
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
