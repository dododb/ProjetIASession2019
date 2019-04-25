using Newtonsoft.Json;
using ReseauNeuronal.NeuronalNetwork.flux;
using ReseauNeuronal.NeuronalNetwork.IEnumerableExtention;
using ReseauNeuronal.NeuronalNetwork.neurone;
using ReseauNeuronal.NeuronalNetwork.sauvegarde;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReseauNeuronal.NeuronalNetwork.Reseau
{
    class AutoEncoderNetwork : AbstractNetwork
    {
        private Layer[] input = new Layer[2];
        private List<Layer>[] hiddens = { new List<Layer>(), new List<Layer>() };
        private Layer[] output = new Layer[2];

        private List<Layer>[] ReverseHiddens = new List<Layer>[2];

        public ILayerSender starts;
        public Layer[] FinalLayer => output;
        public Layer[] FirstLayer => input;

        public List<Layer>[] HiddensLayer => hiddens;


        public AutoEncoderNetwork(Layer[] input, List<Layer>[] hiddens, Layer[] output, ILayerSender layerStart)
        {
            for(int i =0; i<2; i++)
            {
                this.input[i] = input[i];
                this.hiddens[i] = hiddens[i];
                this.output[i] = output[i];
            }
            starts = layerStart;
        }
        public AutoEncoderNetwork(int nbInput, int nbBottleNeck, int nbHidden)
        {
            input[0] = new Layer(nbInput, newPerceptronLayer);
            hiddens[0] = GenerateHiddenLayers(nbInput, nbBottleNeck, nbHidden).ToList();
            output[0] = new Layer(nbBottleNeck, newPerceptronLayer);

            input[1] = new Layer(nbBottleNeck, newPerceptronLayer);
            hiddens[1] = GenerateHiddenLayers(nbInput, nbBottleNeck, nbHidden).Reverse().ToList();
            output[1] = new Layer(nbInput, newPerceptronFinal);

            starts = new LayerStart(nbInput);

            GenerateConnexion();

            ReverseHiddens[0] = hiddens[0].AsEnumerable().Reverse().ToList();
            ReverseHiddens[1] = hiddens[1].AsEnumerable().Reverse().ToList();
        }

        private void GenerateConnexion()
        {
            input[0].ConnectTo(starts);
            for (int i = 0; i < 2; i++)
            {
                if (hiddens[i].Count == 0)
                {
                    output[i].ConnectTo(input[i]);
                }
                else
                {
                    var hiddenFirst = hiddens[i].First();
                    var hiddenLast = hiddens[i].Last();

                    hiddenFirst.ConnectTo(input[i]);
                    Layer.Join(hiddens[i]);
                    output[i].ConnectTo(hiddenLast);
                }
            }
            input[1].ConnectTo(output[0]);
        }

        public override IEnumerable<double> Predict(IEnumerable<double> row)
        {
            IEnumerable<IDataSender> networkStarts = starts.Senders;
            foreach (var (data, entree) in row.ZipIteration(networkStarts))
                entree.Value = data;

            foreach (var i in input[0].Predict()) ;
            foreach (var hidden in hiddens[0]) foreach (var i in hidden.Predict()) ;
            foreach (var o in output[0].Predict()) ;

            foreach (var i in input[1].Predict()) ;
            foreach (var hidden in hiddens[1]) foreach (var i in hidden.Predict()) ;

            return output[1].Predict().ToArray(); // ça ne marche pas sans le toArray()
        }

        public override void Learn(IEnumerable<double> labels)
        {
            output[1].Learn(labels);
            foreach (var hidden in ReverseHiddens[1]) hidden.Learn(labels);
            input[1].Learn(labels);

            output[0].Learn(labels);
            foreach (var hidden in ReverseHiddens[0]) hidden.Learn(labels);
            input[0].Learn(labels);
        }

        public AutoencoderSauvegarde Sauvegarde()
        {
            return new AutoencoderSauvegarde(this);
        }
    }
}
