using ReseauNeuronal.NeuronalNetwork.extremite;
using ReseauNeuronal.NeuronalNetwork.flux;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReseauNeuronal.NeuronalNetwork.Reseau
{
    class LayerEnd : AbstractLayerReceiver<NetworkEnd>
    {
        public LayerEnd(int nbOutput)
        {
            for (int i = 0; i < nbOutput; i++) layer.Add(new NetworkEnd());
        }
    }
}
