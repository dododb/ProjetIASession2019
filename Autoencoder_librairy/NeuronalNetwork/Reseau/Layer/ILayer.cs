﻿using ReseauNeuronal.NeuronalNetwork.flux;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReseauNeuronal.NeuronalNetwork.Reseau
{
    public interface ILayerReceiver<T> 
        where T : IDataReceiver
    {
        IEnumerable<T> Receivers { get; }
        void ConnectTo(ILayerSender sender);

        ILayerSender Sender { get; }
    }

    public interface ILayerSender
    {
        IEnumerable<IDataSender> Senders { get; }
    }
}
