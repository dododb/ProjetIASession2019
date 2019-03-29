using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp8.NeuronalNetwork
{
    interface IDataReceiver
    {
        Dictionary<IDataSender, double> DataSenders { get; set; }
        void ConnectTo(IDataSender dataExit);
    }
}
