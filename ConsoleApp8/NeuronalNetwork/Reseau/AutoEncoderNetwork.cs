using System;
using System.Collections.Generic;
using System.Text;

namespace ReseauNeuronal.NeuronalNetwork.Reseau
{
    class AutoEncoderNetwork : AbstractNetwork
    {
        private Network leftNetwork;
        private Network rightNetwork;

        public AutoEncoderNetwork(int nbInput, int nbBottleNeck, int nbHidden)
        {
            leftNetwork = new Network(nbInput, nbBottleNeck, nbHidden);
            rightNetwork = new Network(nbBottleNeck, nbInput, nbHidden);

            rightNetwork.FirstLayer.ConnectTo(leftNetwork.FinalLayer);
        }

        public override double[] Predict(double[] row)
        {
            var leftExit = leftNetwork.Predict(row);
            var rightExit = rightNetwork.Predict(leftExit);
            return rightExit;
        }

        public override void Learn(IEnumerable<double> labels)
        {
            rightNetwork.Learn(labels);
        }
    }
}
