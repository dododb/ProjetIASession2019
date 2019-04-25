using ReseauNeuronal.NeuronalNetwork.flux;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReseauNeuronal.NeuronalNetwork.extremite
{
    interface IEntreePoint : IDataSender
    {
        new double Value { get; set; } // j'aime pas trop...
    }
}
