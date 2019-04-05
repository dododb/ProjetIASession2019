using Newtonsoft.Json;
using ReseauNeuronal.NeuronalNetwork.extremite;
using ReseauNeuronal.NeuronalNetwork.flux;
using ReseauNeuronal.NeuronalNetwork.IEnumerableExtention;
using ReseauNeuronal.NeuronalNetwork.neurone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ReseauNeuronal.NeuronalNetwork.Reseau
{
    [JsonObject(MemberSerialization.OptIn)]
    class Network
    {
        private static WeightInitialisation weight = Functions.RandomInit;
        private static Func<Perceptron> newPerceptronLayer = () => new PerceptronLayer(weight);
        private static Func<Perceptron> newPerceptronFinal = () => new PerceptronFinal(weight);


        [JsonProperty]
        private Layer input;
        [JsonProperty]
        private List<Layer> hiddens = new List<Layer>();
        [JsonProperty]
        private Layer output;

        private List<Layer> ReverseHiddens;

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

        public IEnumerable<double[]> Predict(double[][] dataset)
        {
            IEnumerable<IDataSender> networkStarts = starts.Senders;
            IEnumerable<NetworkEnd> networkEnds = ends.Receivers;

            foreach (var row in dataset)
            {
                foreach (var (data, entree) in row.ZipIteration(networkStarts))
                    entree.Value = data;

                yield return networkEnds.Select(x => x.Value).ToArray();
            }
        }

        public IEnumerable<(double[], double[])> Learning(double[][] dataset, double[][] labelsVector)
        {
            IEnumerable<IDataSender> networkStarts = starts.Senders;
            IEnumerable<NetworkEnd> networkEnds = ends.Receivers;

            var predict = Predict(dataset);
            foreach (var (predictions, labels) in predict.ZipIteration(labelsVector))
            {
                foreach(var (prediction, label) in predictions.ZipIteration(labels))
                {
                    
                }
                Learn(labels);
                
                yield return (predictions, labels);
            }
        }

        private void Learn(IEnumerable<double> labels)
        {
            output.Learn(labels); // peut etre retourner un IEnumerable de sigma 
            foreach (var hidden in ReverseHiddens) hidden.Learn(labels);
            input.Learn(labels);
        }
    }
}
