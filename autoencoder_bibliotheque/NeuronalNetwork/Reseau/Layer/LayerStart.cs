using System;
using System.Collections.Generic;
using System.Text;
using ReseauNeuronal.NeuronalNetwork.extremite;
using ReseauNeuronal.NeuronalNetwork.flux;

namespace ReseauNeuronal.NeuronalNetwork.Reseau
{
    class LayerStart : ILayerSender
    {
        public IEnumerable<IDataSender> Senders => layer;

        private List<NetworkStart> layer = new List<NetworkStart>();

        public LayerStart(int nbInput)
        {
            for (int i = 0; i < nbInput; i++) layer.Add(new NetworkStart());
        }
    }
}
