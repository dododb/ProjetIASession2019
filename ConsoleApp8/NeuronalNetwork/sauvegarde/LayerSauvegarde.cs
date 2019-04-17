using Newtonsoft.Json;
using ReseauNeuronal.NeuronalNetwork.flux;
using ReseauNeuronal.NeuronalNetwork.Reseau;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReseauNeuronal.NeuronalNetwork.sauvegarde
{

    [JsonObject(MemberSerialization.OptIn)]
    class LayerSauvegarde
    {
        [JsonProperty]
        public List<PerceptronSauvegarde> perceptrons;

        public LayerSauvegarde() { }
        public LayerSauvegarde(Layer layer)
        {
            perceptrons = layer.Receivers.Select(x => new PerceptronSauvegarde(x)).ToList();
        }

        public Layer GetLayer(ILayerSender sendersLayer)
        {
            List<IDataSender> senders = sendersLayer.Senders.ToList();
            Layer l = new Layer(perceptrons.Select(x => x.GetPerceptron(senders)).ToList());
            l.Sender = sendersLayer;
            return l;
        }
    }
}
