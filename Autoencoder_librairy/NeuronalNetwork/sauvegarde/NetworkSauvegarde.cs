using Newtonsoft.Json;
using ReseauNeuronal.NeuronalNetwork.Reseau;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReseauNeuronal.NeuronalNetwork.sauvegarde
{

    [JsonObject(MemberSerialization.OptIn)]
    public class NetworkSauvegarde
    {
        [JsonProperty]
        LayerSauvegarde input;
        [JsonProperty]
        List<LayerSauvegarde> hiddens;
        [JsonProperty]
        LayerSauvegarde output;

        public NetworkSauvegarde() { }
        public NetworkSauvegarde(Network network)
        {
            input = new LayerSauvegarde(network.FirstLayer);
            output = new LayerSauvegarde(network.FinalLayer);
            hiddens = network.HiddensLayer.Select(x => new LayerSauvegarde(x)).ToList();
        }

        public Network GetNetwork()
        {
            int nbInput = input.perceptrons.Count;
            LayerStart layerStart = new LayerStart(nbInput);
            return GetNetwork(layerStart);
        }

        public Network GetNetwork(ILayerSender layerPre)
        {
            Layer inputLayer = input.GetLayer(layerPre);

            List<Layer> hiddensLayer = new List<Layer>();
            Layer current = inputLayer;
            foreach (var hidden in hiddens)
            {
                current = hidden.GetLayer(current);
                hiddensLayer.Add(current);
            }
            Layer outputLayer = output.GetLayer(current);

            return new Network(inputLayer, hiddensLayer, outputLayer, layerPre);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
