using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp8.NeuronalNetwork
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
        }
        public Dictionary<IDataSender, double> DataSenders { get; set; } = new Dictionary<IDataSender, double>();
        ActivateFunction activateFunction;
        WeightInitialisation weightInitialisation;

        public Perceptron(ActivateFunction f, WeightInitialisation weight)
        {
            activateFunction = f;
            weightInitialisation = weight;
            ConnectTo(new Biais());
        }
        public void ConnectTo(IDataSender dataExit)
        {
            DataSenders.Add(dataExit, weightInitialisation());
        }

        public void Learning(float realValue)
        {
            foreach (var sender in DataSenders.Keys)
            {
                DataSenders[sender] += 0 + alpha * (realValue - Value) * sender.Value;
            }
        }

        public double SumOfEntree()
        {
            double result = 0;
            foreach (var ent in DataSenders) result += ent.Key.Value * ent.Value;
            return result;
        }
    }
}
