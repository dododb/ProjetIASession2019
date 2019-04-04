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
        static int nbIteration = 1_000;
        static void Main(string[] args)
        {
            var network = new Network(2, 2, 0);

            double[] data00 = new[] { 0d, 0d };
            double[] data01 = new[] { 0d, 1d };
            double[] data10 = new[] { 1d, 0d };
            double[] data11 = new[] { 1d, 1d };

            double[] label00 = new[] { 0d, 0d };
            double[] label01 = new[] { 0d, 1d };
            double[] label10 = new[] { 1d, 0d };
            double[] label11 = new[] { 1d, 1d };
            
            double[][] dataXOR = new[] { data00, data01, data10, data11 };
            double[][] labels = new[] { label00, label01, label10, label11 };

            for(int i = 0; i < nbIteration; i++)
                foreach(var t in network.Learn(dataXOR, labels)) ;

            Console.Read();
        }
    }
}
