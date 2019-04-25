using ReseauNeuronal.NeuronalNetwork.IEnumerableExtention;
using ReseauNeuronal.NeuronalNetwork.neurone;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReseauNeuronal.NeuronalNetwork.Reseau
{
    abstract class AbstractNetwork : INetwork
    {
        private static WeightInitialisation weight = Functions.RandomInit;
        protected Func<Perceptron> newPerceptronLayer = () => new PerceptronLayer(weight);
        protected Func<Perceptron> newPerceptronFinal = () => new PerceptronFinal(weight);

        public abstract IEnumerable<double> Predict(IEnumerable<double> row);

        public IEnumerable<IEnumerable<double>> Predict(double[][] dataset)
        {
            foreach (var row in dataset)
            {
                yield return Predict(row);
            }
        }

        public IEnumerable<(IEnumerable<double>, IEnumerable<double>)> Learning(double[][] dataset, double[][] labelsVector)
        {
            var predict = Predict(dataset);
            foreach (var (predictions, labels) in predict.ZipIteration(labelsVector))
            {
                Learn(labels);

                yield return (predictions, labels);
            }
        }

        public abstract void Learn(IEnumerable<double> labels);

        protected virtual IEnumerable<Layer> GenerateHiddenLayers(int lengthIn, int lengthOut, int nbHidden)
        {
            for (int j = 0; j < nbHidden; j++)
            {
                int ecart = lengthOut - lengthIn;
                lengthIn += ecart / (nbHidden + 1 - j);
                yield return new Layer(lengthIn, newPerceptronLayer);
            }
        }
    }
}
