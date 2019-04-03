using System;
using System.Collections.Generic;
using System.Text;

namespace ReseauNeuronal.NeuronalNetwork
{
    public delegate double ActivateFunction(double x);
    public delegate double WeightInitialisation();
    public abstract class Functions
    {
        static Random rand = new Random(42);
        public static double Sign(double x)
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
