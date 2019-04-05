using System;
using System.Collections.Generic;
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

        [JsonProperty]
        protected List<T> layer = new List<T>();

        public virtual void ConnectTo(ILayerSender sender)
        {
            foreach (var perceptronReceiver in Receivers)
                foreach (var perceptronSender in sender.Senders)
                    perceptronReceiver.ConnectTo(perceptronSender);
        }
    }
}
