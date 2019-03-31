using ReseauNeuronal.NeuronalNetwork.flux;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReseauNeuronal.NeuronalNetwork.Lien
{
    class Link
    {
        private double weigth;
        private double lastWeigth;

        public IDataSender dataSender;
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
