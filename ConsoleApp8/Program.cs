using ReseauNeuronal.NeuronalNetwork;
using ReseauNeuronal.NeuronalNetwork.extremite;
using ReseauNeuronal.NeuronalNetwork.IEnumerableExtention;
using ReseauNeuronal.NeuronalNetwork.neurone;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReseauNeuronal
{
    class Program
    {
        static int nbIteration = 10_000;
        static void Main(string[] args)
        {
            var perceptronF = new PerceptronFinal(Functions.RandomInit);
            var perceptronLeft1 = new PerceptronLayer(Functions.RandomInit);
            var perceptronLeft2 = new PerceptronLayer(Functions.RandomInit);
            var perceptronRight1 = new PerceptronLayer(Functions.RandomInit);
            var perceptronRight2 = new PerceptronLayer(Functions.RandomInit);
            var center = new PerceptronLayer(Functions.RandomInit);
            var entree1 = new NetworkStart();
            var entree2 = new NetworkStart();
            var end = new NetworkEnd();
            var end2 = new NetworkEnd();

            perceptronLeft1.ConnectTo(entree1);
            perceptronLeft2.ConnectTo(entree2);

            perceptronLeft1.ConnectTo(entree2);
            perceptronLeft2.ConnectTo(entree1);

            perceptronF.ConnectTo(perceptronLeft1);
            perceptronF.ConnectTo(perceptronLeft2);

            //center.ConnectTo(perceptronLeft1);
            //center.ConnectTo(perceptronLeft2);

            //perceptronL3.ConnectTo(perceptronL1);
            //perceptronL3.ConnectTo(perceptronL2);

            //perceptronL4.ConnectTo(perceptronL1);
            //perceptronL4.ConnectTo(perceptronL2);

            //perceptronRight1.ConnectTo(center);
            //perceptronRight2.ConnectTo(center);

            end.ConnectTo(perceptronF);
            //end2.ConnectTo(perceptronRight2);

            double[] data00 = new[] { 0d, 0d };
            double[] data01 = new[] { 0d, 1d };
            double[] data10 = new[] { 1d, 0d };
            double[] data11 = new[] { 1d, 1d };

            double[] label00 = new[] { 0d };
            double[] label01 = new[] { 1d };
            double[] label10 = new[] { 1d };
            double[] label11 = new[] { 0d };

            //double[][] dataXOR = new[] { data00 };
            //double[][] labels = new[] { label00 };
            double[][] dataXOR = new[] { data00, data01, data10, data11 };
            double[][] labels = new[] { label00, label01, label10, label11 };

            IEntreePoint[] entrees = { entree1, entree2 };
            IEndPoint[] ends = { end };

            Console.WriteLine("données d'entrée");
            Console.WriteLine("A\tB\tlabel");
            foreach (var (data, label) in dataXOR.ZipIteration(labels))
                Console.WriteLine(data[0] + "\t" + data[1] + "\t" + label[0]);

            Console.WriteLine("\nPoids du perceptron avant");
            perceptronLeft1.PrintData();
            Console.WriteLine("\nPoids du perceptron avant");
            perceptronLeft2.PrintData();

            Console.WriteLine("\nEntrainement\n");
            for (int i = 0; i < nbIteration; i++)
            {
                Console.WriteLine("itération numéro : " + (i+1));
                Learn(dataXOR, labels, entrees, ends);
            }

            Console.WriteLine("Poids du perceptron après");
            perceptronF.PrintData();

            Console.WriteLine("\nPoids du perceptron après");
            perceptronLeft1.PrintData();
            Console.WriteLine("\nPoids du perceptron après");
            perceptronLeft2.PrintData();
            Console.Read();
        }

        /// <summary>
        /// algorithme d'apprentissage pour sortie multiple
        /// </summary>
        /// <param name="dataset"></param>
        /// <param name="entrees"></param>
        /// <param name="ends"></param>
        static void Learn(IEnumerable<double[]> dataset, IEnumerable<double[]> labelsVector, IEnumerable<IEntreePoint> entrees, IEnumerable<IEndPoint> ends)
        {
            foreach (var (row, labels) in dataset.ZipIteration(labelsVector))
            {
                foreach (var (data, entree) in row.ZipIteration(entrees))
                    entree.Value = data;

                foreach (var (label, end) in labels.ZipIteration(ends))
                {
                    double prediction = end.Value;
                    Console.WriteLine("prediction : " + prediction + " || " + "label : " + label);
                    end.BeginLearning(label);
                }

                foreach (var (label, end) in labels.ZipIteration(ends)) end.ResetLearning();
            }

            Console.WriteLine("");
        }

        /// <summary>
        /// algorithme d'apprentissage avec une sortie unique
        /// ici on assume que le dernier element d'un enregistrement est le label
        /// </summary>
        /// <param name="dataset"></param>
        /// <param name="entrees"></param>
        /// <param name="end"></param>
        static void Learn(IEnumerable<double[]> dataset, IEnumerable<IEntreePoint> entrees, IEndPoint end)
        {
            foreach (var row in dataset)
            {
                var label = row.Last();
                var datasWithoutLast = row.SkipLast(1);
                foreach (var (data, entree) in datasWithoutLast.ZipIteration(entrees))
                    entree.Value = data;
                
                double prediction = end.Value;
                Console.WriteLine("prediction : " + prediction + " || " + "label : " + label);
                end.BeginLearning(label);
            }

            Console.WriteLine("");
        }
    }
}
