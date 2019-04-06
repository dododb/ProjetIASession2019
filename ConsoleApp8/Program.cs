using Newtonsoft.Json;
using ReseauNeuronal.NeuronalNetwork;
using ReseauNeuronal.NeuronalNetwork.extremite;
using ReseauNeuronal.NeuronalNetwork.IEnumerableExtention;
using ReseauNeuronal.NeuronalNetwork.neurone;
using ReseauNeuronal.NeuronalNetwork.Reseau;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ReseauNeuronal
{
    class Program
    {
        static int nbIteration = 10_000;
        static int nbRow = 2;
        static int nbInputOutput = 1024;
        static int bootleNeck = 512;
        static int nbHidden = 1;
        static Random randomGenerator = new Random(41);
        static void Main(string[] args)
        {
            Stopwatch watch = new Stopwatch();
            var network = new AutoEncoderNetwork(nbInputOutput, bootleNeck, nbHidden);

            //string output = JsonConvert.SerializeObject(network); marche pas encore
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

            //output = JsonConvert.SerializeObject(network);

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
