﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ReseauNeuronal.NeuronalNetwork.Reseau
{
    interface INetwork
    {
        double[] Predict(double[] row);
        IEnumerable<double[]> Predict(double[][] dataset);

        IEnumerable<(double[], double[])> Learning(double[][] dataset, double[][] labelsVector);
    }
}