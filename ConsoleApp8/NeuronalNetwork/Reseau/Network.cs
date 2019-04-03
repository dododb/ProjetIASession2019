using ReseauNeuronal.NeuronalNetwork.extremite;
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
        private WeightInitialisation weight = Functions.RandomInit;

        private List<PerceptronLayer> input = new List<PerceptronLayer>();

        private List<List<PerceptronLayer>> hiddens = new List<List<PerceptronLayer>>();
        private List<List<PerceptronLayer>> ReverseHiddens;

        private List<PerceptronFinal> output = new List<PerceptronFinal>();

        public List<NetworkEnd> ends = new List<NetworkEnd>();
        public List<NetworkStart> starts = new List<NetworkStart>();
        public Network(int nbDataInput, int nbDataOutput, int nbHiddenLayer)
        {
            for (int i = 0; i < nbDataInput; i++)
            {
                starts.Add(new NetworkStart());
                input.Add(new PerceptronLayer(weight));
            }
            for (int i = 0; i < nbDataOutput; i++)
            {
                ends.Add(new NetworkEnd());
                output.Add(new PerceptronFinal(weight));
            }
            for (int i = 0; i < nbHiddenLayer; i++) hiddens.Add(new List<PerceptronLayer>());
            GenerateHiddenLayers();
            ReverseHiddens = hiddens.AsEnumerable().Reverse().ToList();
        }

        protected virtual void GenerateHiddenLayers()
        {
            int lengthOut = output.Count;
            int lengthIn = input.Count;
            int nbHidden = hiddens.Count;

            
            foreach(var hidden in hiddens)
            {
                int ecart = lengthOut - lengthIn;
                lengthIn += ecart / nbHidden;
                for (int i = 0; i < lengthIn; i++)
                {
                    hidden.Add(new PerceptronLayer(weight));
                }
                nbHidden += nbHidden==1 ? 0 : -1;
            }
        }

        public IEnumerable<double[]> Learn(double[][] dataset, double[][] labelsVector)
        {
            foreach(var (row, labels) in dataset.ZipIteration(labelsVector))
            {
                foreach (var (data, entree) in row.ZipIteration(starts))
                    entree.Value = data;

                foreach (var (label, end) in labels.ZipIteration(ends))
                {
                    double prediction = end.Value;
                    Console.WriteLine("prediction : " + prediction + " || " + "label : " + label);
                    end.BeginLearning(label);
                }
                foreach (var (label, end) in labels.ZipIteration(ends)) end.ResetLearning();
                yield return ends.Select(x => x.Value).ToArray();
            }
        }
    }
}
