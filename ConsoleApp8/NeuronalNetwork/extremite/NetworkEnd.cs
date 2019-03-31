using ReseauNeuronal.NeuronalNetwork.extremite;
using ReseauNeuronal.NeuronalNetwork.flux;
using ReseauNeuronal.NeuronalNetwork.neurone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReseauNeuronal.NeuronalNetwork.extremite
{
    /// <summary>
    /// faudra revoir un peu
    /// peut etre le supprimer ce truc
    /// </summary>
    class NetworkEnd : IEndPoint
    {
        public double Value => dataSender.Value;

        public double Sigma => 0;

        private IDataSender dataSender;

        public void ConnectTo(IDataSender sender)
        {
            //if(sender is PerceptronFinal perceptronFinal)
            dataSender = sender;
        }

        public void ResetLearning()
        {
            dataSender.ResetLearning();
        }

        public void BeginLearning(double realValue)
        {
            dataSender.Learn(realValue);
        }

        public void PrintData()
        { 
            Console.WriteLine(dataSender.Value);
        }

        public double GetWeigth(IDataSender sender)
        {
            return 0;
        }

        public double GetSigma(double y)
        {
            return 0;
        }
    }
}
