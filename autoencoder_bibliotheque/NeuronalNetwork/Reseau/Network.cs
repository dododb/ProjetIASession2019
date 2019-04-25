using Newtonsoft.Json;
using ReseauNeuronal.NeuronalNetwork.extremite;
using ReseauNeuronal.NeuronalNetwork.flux;
using ReseauNeuronal.NeuronalNetwork.IEnumerableExtention;
using ReseauNeuronal.NeuronalNetwork.neurone;
using ReseauNeuronal.NeuronalNetwork.sauvegarde;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ReseauNeuronal.NeuronalNetwork.Reseau
{
    class Network : AbstractNetwork
    {
        private const string extention = "nwk";
        public const string defaultFileName = "neuronalNetwork"; // default name if not set, must contain the path
        public string fileName = defaultFileName;
        private Layer input;
        private List<Layer> hiddens = new List<Layer>();
        private Layer output;

        private List<Layer> ReverseHiddens;
        
        public ILayerSender starts;
        public Layer FinalLayer => output;
        public Layer FirstLayer => input;
        public List<Layer> HiddensLayer => hiddens;

        /// <summary>
        /// instancie un nouveau Network a partir de layer existant
        /// </summary>
        /// <param name="input"></param>
        /// <param name="hiddens"></param>
        /// <param name="output"></param>
        /// <param name="layerStart"></param>
        public Network(Layer input, List<Layer> hiddens, Layer output, ILayerSender layerStart)
        {
            starts = layerStart;
            this.input = input;
            this.output = output;
            this.hiddens = hiddens;

            ReverseHiddens = hiddens.AsEnumerable().Reverse().ToList();
        }

        /// <summary>
        /// instancie un nouveau Network a partir des ses dimension (taille entrée, taille sortie, nb couche caché)
        /// </summary>
        /// <param name="nbDataInput"></param>
        /// <param name="nbDataOutput"></param>
        /// <param name="nbHiddenLayer"></param>
        /// <param name="isLast"> indique si il existe des couche aprés le neurone ou pas => change l'algorithme de calcul du lambda pour la derniere couche</param>
        public Network(int nbDataInput, int nbDataOutput, int nbHiddenLayer, bool isLast = true)
        {
            input = new Layer(nbDataInput, newPerceptronLayer);
            output = new Layer(nbDataOutput, isLast ? newPerceptronFinal : newPerceptronLayer);
            starts = new LayerStart(nbDataInput);

            GenerateHiddenLayers(nbDataInput, nbDataOutput, nbHiddenLayer);
            GenerateConnexion();
            ReverseHiddens = hiddens.AsEnumerable().Reverse().ToList();
        }

        /// <summary>
        /// instantie un nouveau network a l'aide de ses dimension ainsi que du layer qui le précéde
        /// </summary>
        /// <param name="nbDataInput"></param>
        /// <param name="nbDataOutput"></param>
        /// <param name="nbHiddenLayer"></param>
        /// <param name="layerStart"></param>
        /// <param name="isLast"></param>
        public Network(int nbDataInput, int nbDataOutput, int nbHiddenLayer, ILayerSender layerStart, bool isLast = true)
        {
            input = new Layer(nbDataInput, newPerceptronLayer);
            output = new Layer(nbDataOutput, isLast ? newPerceptronFinal : newPerceptronLayer);
            starts = layerStart;

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
        }

        public override double[] Predict(IEnumerable<double> row)
        {
            IEnumerable<IDataSender> networkStarts = starts.Senders;
            foreach (var (data, entree) in row.ZipIteration(networkStarts))
                entree.Value = data;

            input.Predict();
            foreach (var hidden in hiddens) hidden.Predict();
            return output.Predict();
        }

        public override void Learn(IEnumerable<double> labels)
        {
            output.Learn(labels);
            foreach (var hidden in ReverseHiddens) hidden.Learn(labels);
            input.Learn(labels);
        }

        public override string ToString()
        {
            return new NetworkSauvegarde(this).ToString();
        }
        public override void Sauvegarde()
        {
            File.WriteAllText($"{fileName}{extention}", ToString());
        }
    }
}
