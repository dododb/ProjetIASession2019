﻿using Newtonsoft.Json;
using ReseauNeuronal.NeuronalNetwork.neurone;
using ReseauNeuronal.NeuronalNetwork.sauvegarde;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReseauNeuronal.NeuronalNetwork.Reseau
{
    class AutoEncoderNetwork : AbstractNetwork
    {
        private Func<Perceptron> create = () => new PerceptronLayer();
        private Network leftNetwork;
        private Network rightNetwork;

        
        public AutoEncoderNetwork(Network left, Network right)
        {
            leftNetwork = left;
            rightNetwork = right;
            rightNetwork.starts = leftNetwork.FinalLayer;
            //rightNetwork.FirstLayer.Disconnect();
            //rightNetwork.FirstLayer.ConnectTo(leftNetwork.FinalLayer);
        }
        public AutoEncoderNetwork(int nbInput, int nbBottleNeck, int nbHidden)
        {
            leftNetwork = new Network(nbInput, nbBottleNeck, nbHidden, create, create);
            rightNetwork = new Network(nbBottleNeck, nbInput, nbHidden, leftNetwork.FinalLayer);

            //rightNetwork.FirstLayer.Disconnect();
            //rightNetwork.FirstLayer.ConnectTo(leftNetwork.FinalLayer);
        }

        public override IEnumerable<double> Predict(IEnumerable<double> row)
        {
            var leftExit = leftNetwork.Predict(row);
            foreach (var i in rightNetwork.FirstLayer.Predict()) ;
            foreach (var h in rightNetwork.HiddensLayer) foreach (var i in h.Predict()) ;
            var exit = rightNetwork.FinalLayer.Predict().ToArray();
            //var rightExit = rightNetwork.Predict(leftExit);
            return exit;
        }

        public override void Learn(IEnumerable<double> labels)
        {
            rightNetwork.Learn(labels);
            leftNetwork.Learn(labels);
        }

        public string[] Sauvegarde()
        {
            var t = new NetworkSauvegarde(leftNetwork);
            string n1 = JsonConvert.SerializeObject(t);
            string n2 = JsonConvert.SerializeObject(new NetworkSauvegarde(rightNetwork));
            return new[] { n1, n2 };
        }

        public static AutoEncoderNetwork GetAutoEncoder(string[] str)
        {
            if (str.Length != 2) return null;
            NetworkSauvegarde n1 = JsonConvert.DeserializeObject<NetworkSauvegarde>(str[0]);
            NetworkSauvegarde n2 = JsonConvert.DeserializeObject<NetworkSauvegarde>(str[1]);

            Network left = n1.GetNetwork();
            Network right = n2.GetNetwork(left.FinalLayer);
            return new AutoEncoderNetwork(left, right);
        }
    }
}
