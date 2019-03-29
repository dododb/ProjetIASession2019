using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Perceptron.NeuronalNetwork
{
    class NetworkEnd : IDataReceiver
    {
        public Dictionary<IDataSender, double> DataSenders { get; set; } = new Dictionary<IDataSender, double>();

        public void ConnectTo(IDataSender dataExit)
        {
            DataSenders.Add(dataExit, 0);
        }

        public double[] GetValues()
        {
            var values = from sender in DataSenders.Keys
                         select sender.Value;
            return values.ToArray();
        }

        public double GetAvgValues()
        {
            return GetValues().Average();
        }

        public void Learning(double realValue)
        {
            foreach(var sender in DataSenders)
            {
                sender.Key.Learning(realValue);
            }
        }

        public void PrintData()
        {
            foreach(var sender in DataSenders)
            {
                Console.WriteLine(sender.Value);
            }
        }
    }
}
