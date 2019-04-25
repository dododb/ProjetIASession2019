using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using ReseauNeuronal.NeuronalNetwork.flux;
using ReseauNeuronal.NeuronalNetwork.neurone;

namespace ReseauNeuronal.NeuronalNetwork.Reseau
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class AbstractLayerReceiver<T> : ILayerReceiver<T>
        where T : IDataReceiver
    {
        public IEnumerable<T> Receivers => layer;

        public ILayerSender Sender { get; set; }

        [JsonProperty]
        protected List<T> layer = new List<T>();

        public virtual void ConnectTo(ILayerSender sender)
        {
            Sender = sender;
            if (Sender == null) return;
            foreach (var perceptronReceiver in Receivers)
                foreach (var perceptronSender in sender.Senders)
                    perceptronReceiver.ConnectTo(perceptronSender);
        }

        public void Disconnect()
        {
            foreach (var perceptronReceiver in Receivers)
                perceptronReceiver.ClearBeforeConnexion();
        }
    }
}
