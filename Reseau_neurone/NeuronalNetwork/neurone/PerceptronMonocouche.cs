using System.Linq;

namespace ReseauNeuronal.NeuronalNetwork.neurone
{
    class PerceptronMonocouche : Perceptron
    {
        public PerceptronMonocouche(ActivateFunction f, WeightInitialisation weight) : base(f, weight)
        {
        }
        

        public override void Learn(double realValue)
        {
            if (realValue == LastCalculateValue) return;
            int length = dataSenders.Count();
            var senders = dataSenders.Select(x => x.Key).ToList();
            foreach (var (sender, link) in dataSenders)
            {
                link.Weight += alpha * (realValue - LastCalculateValue) * sender.LastCalculateValue;
            }
        }

        protected override double CalculateSigma(double y)
        {
            throw new System.NotImplementedException();
        }
    }
}
