using System;
using System.Collections.Generic;
using System.Text;

namespace Perceptron.NeuronalNetwork
{
    public delegate double ActivateFunction(double x);
    public delegate double WeightInitialisation();
    public class Functions
    {
        static Random rand = new Random();
        public static double SignFunction(double x)
        {
            if (x > 0) return 1;
            else return 0;
        }

        public static double Sigmoid(double x)
        {
            return (1 / (1 + Math.Exp(-x)));
        }

        public static double RandomInit()
        {
            return rand.NextDouble();
        }

        public static double Init0()
        {
            return 0d;
        }
    }
}
