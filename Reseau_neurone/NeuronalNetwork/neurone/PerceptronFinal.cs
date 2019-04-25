using System;
using ReseauNeuronal.NeuronalNetwork.IEnumerableExtention;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReseauNeuronal.NeuronalNetwork.neurone
{
    class PerceptronFinal : Perceptron
    {
        //public PerceptronFinal() : base(Functions.Sigmoid, Functions.RandomInit) { }
        public PerceptronFinal(double weigthBiais) : base(Functions.Sigmoid, Functions.RandomInit)
        {
            ConnectTo(Biais.GetInstance(), weigthBiais);
        }
        public PerceptronFinal(WeightInitialisation weight) : base(Functions.Sigmoid, weight)
        {
        }

        /// <summary>
        /// ça a l'air de marcher
        /// </summary>
        /// <param name="realValue"></param>
        public override void Learn(double realValue)
        {
            CalculateSigma(realValue);
            foreach (var dataSender in dataSenders)
            {
                dataSender.Value.Weight += alpha * sigma * dataSender.Key.LastCalculateValue;
            }
        }

        protected override double CalculateSigma(double realValue)
        {
            sigma = (realValue - LastCalculateValue) * LastCalculateValue * (1 - LastCalculateValue);
            lastCalculatedSigma = sigma;
            return sigma;
        }
    }
}
