using ReseauNeuronal.NeuronalNetwork.IEnumerableExtention;
using ReseauNeuronal.NeuronalNetwork.neurone;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReseauNeuronal.NeuronalNetwork.Reseau
{
    public abstract class AbstractNetwork : INetwork
    {
        private static WeightInitialisation weight = Functions.RandomInit;
        protected Func<Perceptron> newPerceptronLayer = () => new PerceptronLayer(weight);
        protected Func<Perceptron> newPerceptronFinal = () => new PerceptronFinal(weight);

        public abstract double[] Predict(IEnumerable<double> row);

        public IEnumerable<double[]> Predict(double[][] dataset)
        {
            foreach (var row in dataset)
            {
                yield return Predict(row);
            }
        }

        public IEnumerable<(double[], double[])> Learning(double[][] dataset, double[][] labelsVector)
        {
            var predict = Predict(dataset);
            foreach (var (predictions, labels) in predict.ZipIteration(labelsVector))
            {
                Learn(labels);

                yield return (predictions, labels);
            }
        }

        public abstract void Learn(IEnumerable<double> labels);
        public abstract void Sauvegarde();
    }
}
