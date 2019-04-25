using System.Linq;

namespace ReseauNeuronal.NeuronalNetwork.neurone
{
    public class PerceptronMonocouche : Perceptron
    {
        public PerceptronMonocouche(ActivateFunction f, WeightInitialisation weight) : base(f, weight)
        {
        }
        

        public override void Learn(double realValue)
        {
            if (realValue == LastCalculateValue) return;
            int length = dataSenders.Count();
            var senders = dataSenders.Select(x => x.Key).ToList();
            foreach (var dataSender in dataSenders)
            {
                dataSender.Value.Weight += alpha * (realValue - LastCalculateValue) * dataSender.Key.LastCalculateValue;
            }
        }

        protected override double CalculateSigma(double y)
        {
            throw new System.NotImplementedException();
        }
    }
}
