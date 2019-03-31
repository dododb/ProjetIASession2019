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
            if (hasLearn) return;
            if (y == LastCalculateValue) return;
            double simga = CalculateSigma(y);
            //verifier que c'est le bon calcul mais ça a l'ai pas trop mal
            foreach (var (sender, link) in dataSenders)
            {
                link.Weight += alpha * sigma * sender.LastCalculateValue;
            }
            hasLearn = true;
            foreach (var (sender, link) in dataSenders)
            {
                sender.Learn(y);
            }
        }

        protected override double CalculateSigma(double y)
        {
            if (!isSigmaCalculated)
            {
                isSigmaCalculated = true;
                double sumSigmaWeigth = dataReceivers.Select(x => x.Value.LastWeigth * x.Key.GetSigma(y)).Sum();
                sigma = LastCalculateValue * (1 - LastCalculateValue) * sumSigmaWeigth;
            }
            return sigma;
        }
    }
}
