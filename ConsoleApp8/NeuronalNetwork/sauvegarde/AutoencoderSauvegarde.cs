using Newtonsoft.Json;
using ReseauNeuronal.NeuronalNetwork.Reseau;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReseauNeuronal.NeuronalNetwork.sauvegarde
{
    [JsonObject(MemberSerialization.OptIn)]
    class AutoencoderSauvegarde
    {
        [JsonProperty]
        LayerSauvegarde[] input = new LayerSauvegarde[2];
        [JsonProperty]
        List<LayerSauvegarde>[] hiddens = new List<LayerSauvegarde>[2];
        [JsonProperty]
        LayerSauvegarde[] output = new LayerSauvegarde[2];

        public AutoencoderSauvegarde() { }
        public AutoencoderSauvegarde(AutoEncoderNetwork network)
        {
            input = network.FirstLayer.Select(x => new LayerSauvegarde(x)).ToArray();
            output = network.FinalLayer.Select(x => new LayerSauvegarde(x)).ToArray();
            hiddens = network.HiddensLayer.Select(x => x.Select(y => new LayerSauvegarde(y)).ToList()).ToArray();
        }

        public AutoEncoderNetwork GetNetwork()
        {
            int nbInput = input[0].perceptrons.Count;
            LayerStart layerStart = new LayerStart(nbInput);
            return GetNetwork(layerStart);
        }

        public AutoEncoderNetwork GetNetwork(ILayerSender layerPre)
        {
            Layer leftInput = input[0].GetLayer(layerPre);
            List<Layer> leftHiddensLayer = new List<Layer>();
            Layer current = leftInput;
            foreach (var hidden in hiddens[0])
            {
                current = hidden.GetLayer(current);
                leftHiddensLayer.Add(current);
            }
            Layer leftOutput = output[0].GetLayer(current);

            Layer rightInput = input[1].GetLayer(leftOutput);
            List<Layer> rightHiddensLayer = new List<Layer>();
            current = rightInput;
            foreach (var hidden in hiddens[1])
            {
                current = hidden.GetLayer(current);
                rightHiddensLayer.Add(current);
            }
            Layer rightOutput = output[1].GetLayer(current);

            return new AutoEncoderNetwork(
                new []{ leftInput, rightInput },
                new[] { leftHiddensLayer, rightHiddensLayer },
                new[] { leftOutput, rightOutput },
                layerPre);
        }
    }
}
