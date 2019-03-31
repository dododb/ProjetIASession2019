using ReseauNeuronal.NeuronalNetwork.flux;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReseauNeuronal.NeuronalNetwork.extremite
{
    interface IEndPoint : IDataReceiver
    {
        double Value { get; }

        /// <summary>
        /// permet de reaclculer les poid a partir de la valeur réel
        /// </summary>
        /// <param name="realValue"></param>
        void BeginLearning(double realValue);

        void ResetLearning();
    }
}
