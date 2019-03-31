using ReseauNeuronal.NeuronalNetwork.extremite;
using ReseauNeuronal.NeuronalNetwork.flux;
using ReseauNeuronal.NeuronalNetwork.Lien;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace ReseauNeuronal.NeuronalNetwork
{
    class Biais : IDataSender, IEntreePoint
    {
        public double Value { get; set; } = 1;

        public double LastCalculateValue => Value;

        static Biais instance = null;
        private Biais()
        {
        }

        //[MethodImpl(MethodImplOptions.Synchronized)]
        public static Biais GetInstance()
        {
            if (instance == null) instance = new Biais();
            return instance;
        }

        public void Learn(double realValue)
        {
            
        }

        public void ReverseConnexion(IDataReceiver dataReceiver, Link l)
        {
        }

        public void ResetLearning()
        {
        }
    }
}
