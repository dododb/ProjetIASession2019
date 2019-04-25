using Newtonsoft.Json;
using ReseauNeuronal.NeuronalNetwork.flux;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ReseauNeuronal.NeuronalNetwork.Lien
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Link
    {
        [JsonProperty]
        private double weigth;
        private double lastWeigth;
        
        
        [JsonProperty]
        public IDataSender dataSender;
        [JsonProperty]
        public IDataReceiver dataReceiver;
        public double Weight { get => weigth;
            set
            { 
                lastWeigth = weigth;
                weigth=value;
            }
        }

        public double LastWeigth => lastWeigth;
    }
}
