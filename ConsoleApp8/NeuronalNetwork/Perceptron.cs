using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Perceptron.NeuronalNetwork
{
    class Perceptron : IDataReceiver, IDataSender
    {
        private float alpha = 1;

        private double lastCalculateValue;
        
        // On pourrais rajouter une amélioration afin qu'on ne recalcule pas la valeur lors de l'apprentissage
        public double Value
        {
            get
            {
                lastCalculateValue = activateFunction(SumOfEntree());
                return lastCalculateValue;
            }

            set
            {
                lastCalculateValue = value;
            }
        }
        public Dictionary<IDataSender, double> DataSenders { get; set; } = new Dictionary<IDataSender, double>();

        public double LastCalculateValue => lastCalculateValue;

        ActivateFunction activateFunction;
        WeightInitialisation weightInitialisation;

        public Perceptron(ActivateFunction f=null, WeightInitialisation weight=null)
        {
            if (f == null) f = Functions.SignFunction;
            if (weight == null) weight = Functions.Init0;
            activateFunction = f;
            weightInitialisation = weight;
            ConnectTo(new Biais());
        }
        public void ConnectTo(IDataSender dataExit)
        {
            DataSenders.Add(dataExit, weightInitialisation());
        }

        /// <summary>
        /// faudra changer cette methode pour utiliser le bon calcule d'apprentissage
        /// </summary>
        /// <param name="realValue"></param>
        public void Learning(double realValue)
        {
            if (realValue == lastCalculateValue) return;
            int length = DataSenders.Count();
            for (int i = 0; i< length; i++)
            {
                var sender = DataSenders.ElementAt(i).Key;
                DataSenders[sender] = DataSenders[sender] + alpha * (realValue - LastCalculateValue) * sender.LastCalculateValue;
                //Console.WriteLine("poids : " + DataSenders[sender] + " || realValue : " + realValue + " || value : " + LastCalculateValue + " || x : " + sender.Value);
            }
            //foreach (var sender in DataSenders.Keys)
            //{
            //    DataSenders[sender] += alpha * (realValue - Value) * sender.Value;
            //}
        }

        public double SumOfEntree()
        {
            double result = 0;
            foreach (var ent in DataSenders) result += ent.Key.Value * ent.Value;
            return result;
        }
    }
}
