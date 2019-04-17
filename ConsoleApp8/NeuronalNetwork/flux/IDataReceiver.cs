using System;
using System.Collections.Generic;
using System.Text;

namespace ReseauNeuronal.NeuronalNetwork.flux
{
    interface IDataReceiver
    {
        /// <summary>
        /// permet de creer une connexion
        /// </summary>
        /// <param name="dataExit"></param>
        void ConnectTo(IDataSender dataSender);

        void ClearBeforeConnexion();

        double GetWeigth(IDataSender sender);

        double GetSigma(double y);
    }
}
