using ReseauNeuronal.NeuronalNetwork.IEnumerableExtention;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReseauNeuronal.NeuronalNetwork.Reseau
{
    abstract class AbstractNetwork : INetwork
    {

        public abstract double[] Predict(double[] row);

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
    }
}
