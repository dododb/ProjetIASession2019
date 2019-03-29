using System;
using System.Collections.Generic;
using System.Text;

namespace Perceptron.NeuronalNetwork
{
    interface IDataSender
    {
        double Value { get; set; }

        double LastCalculateValue { get; }

        void Learning(double realValue);

    }
}
