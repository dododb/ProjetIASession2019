using System;
using System.Collections.Generic;
using System.Text;
using ReseauNeuronal.NeuronalNetwork.flux;

namespace ReseauNeuronal.NeuronalNetwork.Reseau
{
    abstract class AbstractLayerReceiver<T> : ILayerReceiver<T>
        where T : IDataReceiver
    {
        public IEnumerable<T> Receivers => layer;

        protected List<T> layer = new List<T>();

        public virtual void ConnectTo(ILayerSender sender)
        {
            foreach (var perceptronReceiver in Receivers)
                foreach (var perceptronSender in sender.Senders)
                    perceptronReceiver.ConnectTo(perceptronSender);
        }
    }
}
