using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReseauNeuronal.NeuronalNetwork.neurone
{
    class PerceptronLayer : Perceptron
    {
        public PerceptronLayer(WeightInitialisation weight) : base(Functions.Sigmoid, weight)
        {
        }

        /// <summary>
        /// petit probleme j'ai besoin des réseau du niveau d'avant (les Sigma k dans la formule pour les non en sortie)
        /// et le reseau est donc récurcif dans le mauvais sens
        /// !!!!!! le learn n'est pas recurcif, il devrai l'etre
        /// </summary>
        /// <param name="realValue"></param>
        public override void Learn(double y)
        {
            CalculateSigma(y);

            foreach (var (sender, link) in dataSenders)
            {
                link.Weight += alpha * sigma * sender.LastCalculateValue;
            }
        }

        protected override double CalculateSigma(double y)
        {
            double sumSigmaWeigth = dataReceivers.Select(x => x.Value.LastWeigth * x.Key.GetSigma(y)).Sum();
            sigma = LastCalculateValue * (1 - LastCalculateValue) * sumSigmaWeigth;
            lastCalculatedSigma = sigma;
            return sigma;
        }
    }
}
