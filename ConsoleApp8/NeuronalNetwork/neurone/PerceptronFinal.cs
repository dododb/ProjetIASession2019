using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReseauNeuronal.NeuronalNetwork.neurone
{
    class PerceptronFinal : Perceptron
    {
        public PerceptronFinal(WeightInitialisation weight) : base(Functions.Sigmoid, weight)
        {
        }

        /// <summary>
        /// ça a l'air de marcher
        /// </summary>
        /// <param name="realValue"></param>
        public override void Learn(double realValue)
        {
            if (hasLearn) return;
            if (realValue == LastCalculateValue) return;
            CalculateSigma(realValue);
            foreach (var (sender, link) in dataSenders)
            {
                link.Weight += alpha * sigma * sender.LastCalculateValue;
            }
            hasLearn = true;
            foreach (var (sender, link) in dataSenders)
            {
                sender.Learn(realValue);
            }
        }

        protected override double CalculateSigma(double realValue)
        {
            if (!isSigmaCalculated)
            {
                isSigmaCalculated = true;
                sigma = (realValue - LastCalculateValue) * LastCalculateValue * (1 - LastCalculateValue);
            }
            return sigma;
        }
    }
}
