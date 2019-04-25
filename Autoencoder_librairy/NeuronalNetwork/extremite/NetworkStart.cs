using ReseauNeuronal.NeuronalNetwork.extremite;
using ReseauNeuronal.NeuronalNetwork.flux;
using ReseauNeuronal.NeuronalNetwork.Lien;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReseauNeuronal.NeuronalNetwork.extremite
{
    public class NetworkStart : IEntreePoint
    {
        public double Value { get; set; }

        public double LastCalculateValue => Value;

        public void Learn(double realValue)
        {
        }

        public void ResetLearning()
        {
        }

        public void ReverseConnexion(IDataReceiver dataReceiver, Link l)
        {
        }
    }
}
