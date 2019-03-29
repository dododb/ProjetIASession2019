using System;
using System.Collections.Generic;
using System.Text;

namespace Perceptron.NeuronalNetwork
{
    class NetworkStart : IDataSender
    {
        public double Value { get; set; }

        public double LastCalculateValue => Value;

        public void Learning(double realValue)
        {

        }
    }
}
