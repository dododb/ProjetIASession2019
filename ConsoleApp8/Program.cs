using Perceptron.NeuronalNetwork;
using System;

namespace Perceptron
{
    class Program
    {
        static int nbIteration = 5;
        static void Main(string[] args)
        {
            var perceptron = new NeuronalNetwork.Perceptron();
            var entree1 = new NetworkStart();
            var entree2 = new NetworkStart();
            var end = new NetworkEnd();

            perceptron.ConnectTo(entree1);
            perceptron.ConnectTo(entree2);

            end.ConnectTo(perceptron);

            double[] data00 = new[] { 0d, 0d, 0d };
            double[] data01 = new[] { 0d, 1d, 1d };
            double[] data10 = new[] { 1d, 0d, 1d };
            double[] data11 = new[] { 1d, 1d, 1d };

            double[][] dataOu = new[] { data00, data01, data10, data11 };

            NetworkStart[] entrees = { entree1, entree2 };

            Console.WriteLine("données d'entrée");
            Console.WriteLine("A\tB\tlabel");
            foreach (var data in dataOu)
                Console.WriteLine(data[0] + "\t" + data[1] + "\t" + data[2]);

            Console.WriteLine("\n\nEntrainement\n\n");
            for (int i = 0; i < nbIteration; i++)
            {
                Console.WriteLine("itération numéro : " + (i+1));
                Learn(dataOu, entrees, end);
            }
            
            Console.Read();
        }

        static void Learn(double[][] datas, NetworkStart[] entree, NetworkEnd end)
        {
            foreach (var data in datas)
            {
                entree[0].Value = data[0];
                entree[1].Value = data[1];

                double label = data[2];

                double prediction = end.GetAvgValues();
                Console.WriteLine("prediction : " + prediction + " || " + "label : " + label);
                end.Learning(label);
            }
            Console.WriteLine("");
        }
    }
}
