using Newtonsoft.Json;
using ReseauNeuronal.NeuronalNetwork.flux;
using ReseauNeuronal.NeuronalNetwork.Lien;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ReseauNeuronal.NeuronalNetwork.neurone
{
    [JsonObject(MemberSerialization.OptIn)]
    abstract class Perceptron : IDataReceiver, IDataSender
    {
        protected const float alpha = 1; //sera definit dans une variable plus global...

        protected double lastCalculateValue;

        /// <summary>
        /// peut etre mettre une variable global qui definit si le system est en apprentissage ou pas
        /// pour ne pas a avoir la property LastCalculateValue
        /// </summary>
        public double Value
        {
            get
            {
                lastCalculateValue = activateFunction(SumOfEntree());
                return lastCalculateValue;
            }
            set
            {
                lastCalculateValue = value;
            }
        }
        public double LastCalculateValue => lastCalculateValue;

        //represente les neurone precedent
        //[JsonProperty]
        protected Dictionary<IDataSender, Link> dataSenders = new Dictionary<IDataSender, Link>();

        //represente les neurone suivant
        //[JsonProperty]
        protected Dictionary<IDataReceiver, Link> dataReceivers = new Dictionary<IDataReceiver, Link>();
        
        //represente les neurone suivant
        //protected List<IDataReceiver> dataReceivers = new List<IDataReceiver>();

        private ActivateFunction activateFunction;
        private WeightInitialisation weightInitialisation;

        /// <summary>
        /// faudra changer cette methode pour utiliser le bon calcule d'apprentissage
        /// car ici j'utilise celui du réseau monocouche
        /// </summary>
        /// <param name="realValue"></param>
        public abstract void Learn(double realValue);

        public Perceptron(ActivateFunction f, WeightInitialisation weight)
        {   
            activateFunction = f;
            weightInitialisation = weight;
            ConnectTo(Biais.GetInstance());
        }

        public void ConnectTo(IDataSender dataSender)
        {
            Link newLink = new Link() { Weight = weightInitialisation(), dataReceiver = this, dataSender = dataSender };
            dataSenders[dataSender] = newLink;
            dataSender.ReverseConnexion(this, newLink);
        }   

        public void ReverseConnexion(IDataReceiver dataReceiver, Link l)
        {
            dataReceivers[dataReceiver] = l;
        }

        protected double sigma;
        protected double lastCalculatedSigma;
        protected abstract double CalculateSigma(double y);

        public double GetSigma(double y)
        {
            return lastCalculatedSigma;
        }

        /// <summary>
        /// pas super safe mais osef
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        public double GetWeigth(IDataSender sender)
        {
            return dataSenders[sender].Weight;
        }

        public double SumOfEntree()
        {
            double result = 0;
            foreach (var ent in dataSenders.AsParallel()) result += ent.Key.Value * ent.Value.Weight;
            return result;
        }
    }
}
