using System;
using System.Collections.Generic;
using System.Text;

namespace Perceptron.NeuronalNetwork
{
    class Biais : IDataSender
    {
        public double Value { get; set; } = 1;

        public double LastCalculateValue => Value;

        public void Learning(double realValue)
        {
            
        }
    }
}
