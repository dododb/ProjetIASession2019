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
    class Layer : AbstractLayerReceiver<Perceptron>, ILayerSender
    {
        public IEnumerable<IDataSender> Senders => layer;

        public Layer(int nbPerceptron, Func<Perceptron> func)
        {
            for (int i = 0; i < nbPerceptron; i++) layer.Add(func());
        }

        public void Learn(IEnumerable<double> labels)
        {
            foreach(var (perceptron, label) in layer.ZipIteration(labels).AsParallel())
            {
                perceptron.Learn(label);
            }
            if (Sender is Layer l) l.Learn(labels);
        }

        /// <summary>
        /// pas utilisé
        /// </summary>
        /// <returns></returns>
        public IEnumerable<double> Predict()
        {
            foreach (var perceptron in layer)
                yield return perceptron.Value;
        }

        public static void Join(IEnumerable<Layer> layers)
        {
            var layerPre = layers.First();
            foreach(var layer in layers)
            {
                if (layer == layerPre) continue;
                layer.ConnectTo(layerPre);
                layerPre = layer;
            }
        }
    }
}
