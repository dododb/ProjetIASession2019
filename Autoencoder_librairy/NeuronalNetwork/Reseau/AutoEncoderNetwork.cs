using Newtonsoft.Json;
using ReseauNeuronal.NeuronalNetwork.sauvegarde;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ReseauNeuronal.NeuronalNetwork.Reseau
{
    public class AutoEncoderNetwork : AbstractNetwork
    {
        private static readonly string[] extention = { ".aed1", ".aed2" };
        public const string defaultFileName = "autoencoder"; // default name if not set, must contain the path
        public string fileName = defaultFileName;
        private Network leftNetwork;
        private Network rightNetwork;

        
        public AutoEncoderNetwork(Network left, Network right)
        {
            leftNetwork = left;
            rightNetwork = right;
            rightNetwork.starts = leftNetwork.FinalLayer;
        }
        public AutoEncoderNetwork(int nbInput, int nbBottleNeck, int nbHidden)
        {
            leftNetwork = new Network(nbInput, nbBottleNeck, nbHidden, false);
            rightNetwork = new Network(nbBottleNeck, nbInput, nbHidden, leftNetwork.FinalLayer);
            fileName = defaultFileName + $"_{nbInput}_{nbHidden}_{nbBottleNeck}";
        }

        public override double[] Predict(IEnumerable<double> row)
        {
            var leftExit = leftNetwork.Predict(row);
            var rightExit = rightNetwork.Predict(leftExit);
            return rightExit;
        }

        public override void Learn(IEnumerable<double> labels)
        {
            rightNetwork.Learn(labels);
            leftNetwork.Learn(labels);
        }

        public override void Sauvegarde()
        {
            string n1 = leftNetwork.ToString();
            string n2 = rightNetwork.ToString();

            try
            {
                File.WriteAllText($"{fileName}{extention[0]}", n1);
                File.WriteAllText($"{fileName}{extention[1]}", n2);
            }
            catch
            {
                Console.WriteLine("already sauvegarding this file");
            }
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

        public static AutoEncoderNetwork GetAutoEncoder(string fileName)
        {
            return GetAutoEncoder(new[]{
                File.ReadAllText($"{fileName}{extention[0]}"),
                File.ReadAllText($"{fileName}{extention[1]}")
            });
        }

        public static AutoEncoderNetwork GetAutoEncoder()
        {
            return GetAutoEncoder(defaultFileName);
        }
    }
}
