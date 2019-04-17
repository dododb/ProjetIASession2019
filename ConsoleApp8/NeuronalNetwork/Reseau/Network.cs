using Newtonsoft.Json;
using ReseauNeuronal.NeuronalNetwork.extremite;
using ReseauNeuronal.NeuronalNetwork.flux;
using ReseauNeuronal.NeuronalNetwork.IEnumerableExtention;
using ReseauNeuronal.NeuronalNetwork.neurone;
using ReseauNeuronal.NeuronalNetwork.sauvegarde;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ReseauNeuronal.NeuronalNetwork.Reseau
{
    class Network : AbstractNetwork
    {
        private static WeightInitialisation weight = Functions.RandomInit;
        private Func<Perceptron> newPerceptronLayer = () => new PerceptronLayer(weight);
        private Func<Perceptron> newPerceptronFinal = () => new PerceptronFinal(weight);

        
        private Layer input;
        private List<Layer> hiddens = new List<Layer>();
        private Layer output;

        private List<Layer> ReverseHiddens;
        
        public ILayerSender starts;
        public Layer FinalLayer => output;
        public Layer FirstLayer => input;
        public List<Layer> HiddensLayer => hiddens;

        public Network(Layer input, List<Layer> hiddens, Layer output, LayerStart layerStart)
        {
            starts = layerStart;
            this.input = input;
            this.output = output;
            this.hiddens = hiddens;

            ReverseHiddens = hiddens.AsEnumerable().Reverse().ToList();
        }

        public Network(Layer input, List<Layer> hiddens, Layer output)
        {
            this.input = input;
            this.output = output;
            this.hiddens = hiddens;

            ReverseHiddens = hiddens.AsEnumerable().Reverse().ToList();
        }

        public Network(int nbDataInput, int nbDataOutput, int nbHiddenLayer)
        {
            input = new Layer(nbDataInput, newPerceptronLayer);
            output = new Layer(nbDataOutput, newPerceptronFinal);
            starts = new LayerStart(nbDataInput);

            GenerateHiddenLayers(nbDataInput, nbDataOutput, nbHiddenLayer);
            GenerateConnexion();
            ReverseHiddens = hiddens.AsEnumerable().Reverse().ToList();
        }

        public Network(int nbDataInput, int nbDataOutput, int nbHiddenLayer, ILayerSender layerStart)
        {
            input = new Layer(nbDataInput, newPerceptronLayer);
            output = new Layer(nbDataOutput, newPerceptronFinal);
            starts = layerStart;

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
                lengthIn += ecart / (nbHidden+1-j);
                hiddens.Add(new Layer(lengthIn, newPerceptronLayer));
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
            IEnumerable<IDataSender> networkStarts = starts.Senders;
            foreach (var (data, entree) in row.ZipIteration(networkStarts))
                entree.Value = data;

            foreach(var i in input.Predict());
            foreach (var hidden in hiddens) foreach (var i in hidden.Predict()) ;
            return output.Predict().ToArray(); // ça ne marche pas sans le toArray()
        }

        public override void Learn(IEnumerable<double> labels)
        {
            output.Learn(labels);
            foreach (var hidden in ReverseHiddens) hidden.Learn(labels);
            input.Learn(labels);
        }

        public NetworkSauvegarde Sauvegarde()
        {
            return new NetworkSauvegarde(this);
        }
    }
}
