using System;
using System.Collections.Generic;
using System.Text;

namespace ReseauNeuronal.NeuronalNetwork.Reseau
{
    interface INetwork
    {
        IEnumerable<double> Predict(IEnumerable<double> row);
        IEnumerable<IEnumerable<double>> Predict(double[][] dataset);

        IEnumerable<(IEnumerable<double>, IEnumerable<double>)> Learning(double[][] dataset, double[][] labelsVector);
    }
}
