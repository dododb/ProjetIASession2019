using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using ReseauNeuronal.NeuronalNetwork.flux;

namespace ReseauNeuronal.NeuronalNetwork.Reseau
{
    [JsonObject(MemberSerialization.OptIn)]
    abstract class AbstractLayerReceiver<T> : ILayerReceiver<T>
        where T : IDataReceiver
    {
        public IEnumerable<T> Receivers => layer;

        public ILayerSender Sender { get; private set; }

        [JsonProperty]
        protected List<T> layer = new List<T>();

        public virtual void ConnectTo(ILayerSender sender)
        {
            Sender = sender;
            foreach (var perceptronReceiver in Receivers.AsParallel())
                foreach (var perceptronSender in sender.Senders.AsParallel())
                    perceptronReceiver.ConnectTo(perceptronSender);
        }
    }
}
