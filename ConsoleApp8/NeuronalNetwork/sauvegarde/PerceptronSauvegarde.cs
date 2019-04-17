using Newtonsoft.Json;
using ReseauNeuronal.NeuronalNetwork.flux;
using ReseauNeuronal.NeuronalNetwork.IEnumerableExtention;
using ReseauNeuronal.NeuronalNetwork.neurone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReseauNeuronal.NeuronalNetwork.sauvegarde
{
    abstract class PerceptronFactory
    {
        private static Dictionary<string, Func<double, Perceptron>> perceptronsCreation = new Dictionary<string, Func<double, Perceptron>>
        {
            { "Layer", x => new PerceptronLayer(x) },
            { "Final", x => new PerceptronFinal(x) }
        };

        
        public static Perceptron GetPerceptron(string type, double weightBiais)
        {
            if (perceptronsCreation.TryGetValue(type, out Func<double, Perceptron> create))
            {
                return create(weightBiais);
            }
            return null;
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    class PerceptronSauvegarde
    {
        [JsonProperty]
        string type = "Layer";
        [JsonProperty]
        List<double> senders;

        public PerceptronSauvegarde() { }
        public PerceptronSauvegarde(Perceptron p)
        {
            if (p is PerceptronFinal) type = "Final";
            senders = p.Senders.Select(x => x.Item2).ToList();
        }

        public Perceptron GetPerceptron(List<IDataSender> senders)
        {
            var biais = this.senders.First();
            var sendersWithoutBiais = this.senders.Where((x, y) => y != 0);
            Perceptron perceptron = PerceptronFactory.GetPerceptron(type, biais);
            if(perceptron==null) return null;
            
            foreach (var (weigth, sender) in this.senders.ZipIteration(senders))
            {
                perceptron.ConnectTo(sender, weigth);
            }
            
            return perceptron;
        }
    }
}
