using ReseauNeuronal.NeuronalNetwork.extremite;
using ReseauNeuronal.NeuronalNetwork.flux;
using ReseauNeuronal.NeuronalNetwork.IEnumerableExtention;
using ReseauNeuronal.NeuronalNetwork.neurone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReseauNeuronal.NeuronalNetwork.Reseau
{
    class Network
    {
        private static WeightInitialisation weight = Functions.RandomInit;
        private static Func<Perceptron> newPerceptronLayer = () => new PerceptronLayer(weight);
        private static Func<Perceptron> newPerceptronFinal = () => new PerceptronFinal(weight);
        //il faudrait faite un objet layer
        private Layer input;

        private List<Layer> hiddens = new List<Layer>();
        private List<Layer> ReverseHiddens;

        private Layer output;

        public LayerEnd ends;
        public LayerStart starts;
        public Network(int nbDataInput, int nbDataOutput, int nbHiddenLayer)
        {
            input = new Layer(nbDataInput, newPerceptronLayer);
            output = new Layer(nbDataOutput, newPerceptronFinal);
            starts = new LayerStart(nbDataInput);
            ends = new LayerEnd(nbDataOutput);

            GenerateHiddenLayers(nbDataInput, nbDataOutput, nbHiddenLayer);
            GenerateConnexion();
            ReverseHiddens = hiddens.AsEnumerable().Reverse().ToList();
        }

        protected virtual void GenerateHiddenLayers(int lengthIn, int lengthOut, int nbHidden)
        {

            for (int j = 0; j < nbHidden; j++)
            {
                int ecart = lengthOut - lengthIn;
                lengthIn += ecart / nbHidden;
                hiddens.Add(new Layer(lengthIn, newPerceptronLayer));
                //for (int i = 0; i < lengthIn; i++)
                //{

                //    hidden.Add(new PerceptronLayer(weight));
                //}
                nbHidden += nbHidden == 1 ? 0 : -1;
            }
        }

        private void GenerateConnexion()
        {
            input.ConnectTo(starts);
            if (hiddens.Count == 0)
            {
                output.ConnectTo(input);
            }
            else
            {
                var hiddenFirst = hiddens.First();
                var hiddenLast = hiddens.Last();

                hiddenFirst.ConnectTo(input);
                Layer.Join(hiddens);
                output.ConnectTo(hiddenLast);
            }
            ends.ConnectTo(output);
        }

        public IEnumerable<double[]> Learn(double[][] dataset, double[][] labelsVector)
        {
            IEnumerable<IDataSender> networkStarts = starts.Senders;
            IEnumerable<NetworkEnd> networkEnds = ends.Receivers;

            foreach (var (row, labels) in dataset.ZipIteration(labelsVector))
            {
                foreach (var (data, entree) in row.ZipIteration(networkStarts))
                    entree.Value = data;

                foreach (var (label, end) in labels.ZipIteration(networkEnds))
                {
                    double prediction = end.Value;
                    Console.WriteLine("prediction : " + Math.Round(prediction, 2) + " || " + "label : " + label);
                    //end.BeginLearning(label);
                }
                BeginLearning(labels);

                Console.WriteLine();
                foreach (var (label, end) in labels.ZipIteration(networkEnds)) end.ResetLearning();
                yield return networkEnds.Select(x => x.Value).ToArray();
            }
        }

        private void BeginLearning(IEnumerable<double> labels)
        {
            output.Learn(labels); // peut etre retourner la somme des sigma ? ou un IEnumerable de sigma
            foreach (var hidden in ReverseHiddens) hidden.Learn(labels);
            input.Learn(labels);
        }
    }
}
