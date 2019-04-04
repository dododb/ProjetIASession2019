using ReseauNeuronal.NeuronalNetwork.flux;
using ReseauNeuronal.NeuronalNetwork.neurone;
using System;
using System.Collections.Generic;
using System.Linq;
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
