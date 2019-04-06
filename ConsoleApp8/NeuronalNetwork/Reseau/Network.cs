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
    class Network : AbstractNetwork
    {
        private static WeightInitialisation weight = Functions.RandomInit;
        private Func<Perceptron> newPerceptronLayer = () => new PerceptronLayer(weight);
        private Func<Perceptron> newPerceptronFinal = () => new PerceptronFinal(weight);


        [JsonProperty]
        private Layer input;
        [JsonProperty]
        private List<Layer> hiddens = new List<Layer>();
        [JsonProperty]
        private Layer output;

        private List<Layer> ReverseHiddens;
        
        public LayerStart starts;
        public Layer FinalLayer => output;
        public Layer FirstLayer => input;
        public Network(int nbDataInput, int nbDataOutput, int nbHiddenLayer)
        {
            input = new Layer(nbDataInput, newPerceptronLayer);
            output = new Layer(nbDataOutput, newPerceptronFinal);
            starts = new LayerStart(nbDataInput);

            GenerateHiddenLayers(nbDataInput, nbDataOutput, nbHiddenLayer);
            GenerateConnexion();
            ReverseHiddens = hiddens.AsEnumerable().Reverse().ToList();
        }

        public Network(int nbDataInput, int nbDataOutput, int nbHiddenLayer, Func<Perceptron> createFinal, Func<Perceptron> createLayer)
        {
            newPerceptronLayer = createLayer;
            newPerceptronFinal = createFinal;

            input = new Layer(nbDataInput, newPerceptronLayer);
            output = new Layer(nbDataOutput, newPerceptronFinal);
            starts = new LayerStart(nbDataInput);

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
            //ends.ConnectTo(output);
        }

        public override IEnumerable<double> Predict(IEnumerable<double> row)
        {
            Perceptron.iterationNumber++;
            IEnumerable<IDataSender> networkStarts = starts.Senders;
            IEnumerable<Perceptron> networkEnds = output.Receivers;
            foreach (var (data, entree) in row.ZipIteration(networkStarts))
                entree.Value = data;
            //var l = input.Predict();
            return networkEnds.Select(x => x.Value).ToArray();
        }

        public override void Learn(IEnumerable<double> labels)
        {
            output.Learn(labels);
        }
    }
}
