using Newtonsoft.Json;
using ReseauNeuronal.NeuronalNetwork;
using ReseauNeuronal.NeuronalNetwork.extremite;
using ReseauNeuronal.NeuronalNetwork.IEnumerableExtention;
using ReseauNeuronal.NeuronalNetwork.neurone;
using ReseauNeuronal.NeuronalNetwork.Reseau;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReseauNeuronal
{
    class Program
    {
        static int nbIteration = 10_000;
        static int nbRow = 100;
        static int nbInputOutput = 10;
        static Random randomGenerator = new Random(41);
        static void Main(string[] args)
        {
            var network = new Network(nbInputOutput, nbInputOutput, 1);

            //string output = JsonConvert.SerializeObject(network); marche pas encore
            var ds = GenerateRandomDataset(nbInputOutput, nbRow).ToArray();

            for (int i = 1; i <= nbIteration; i++)
            {
                Console.WriteLine($"iteration : {i}");
                foreach (var (prediction, label) in network.Learning(ds, ds))
                    Console.WriteLine(
                        $"prediction : [{String.Join(", ", prediction.Select(x => Math.Round(x, 2)))}]" +
                        $"|| label : [{String.Join(", ", label.Select(x => Math.Round(x, 2)))}]");
                Console.WriteLine();
            }

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
