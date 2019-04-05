using ReseauNeuronal.NeuronalNetwork.flux;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReseauNeuronal.NeuronalNetwork.extremite
{
    interface IEndPoint : IDataReceiver
    {
        double Value { get; }
    }
}
