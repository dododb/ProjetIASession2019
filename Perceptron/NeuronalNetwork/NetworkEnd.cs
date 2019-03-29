using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp8.NeuronalNetwork
{
    class NetworkEnd : IDataReceiver
    {
        public Dictionary<IDataSender, double> DataSenders { get; set; } = new Dictionary<IDataSender, double>();

        public void ConnectTo(IDataSender dataExit)
        {
            DataSenders.Add(dataExit, 0);
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
