using ReseauNeuronal.NeuronalNetwork.Lien;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReseauNeuronal.NeuronalNetwork.flux
{
    public interface IDataSender
    {
        /// <summary>
        /// Valeur de sorti
        /// Pour un perceptron cela représente la valeur calculer a partir des valeurs d'entrée et des poids
        /// </summary>
        double Value { get; set; }

        /// <summary>
        /// valeur retenue de la derniere fois qu'on a utiliser Value
        /// utilisé lors du learning
        /// </summary>
        double LastCalculateValue { get; }

        /// <summary>
        /// ne pas utiliser manuellement
        /// est utile uniquement dans les class perceptron
        /// </summary>
        /// <param name="dataReceiver"></param>
        void ReverseConnexion(IDataReceiver dataReceiver, Link l);


        /// <summary>
        /// permet de reaclculer les poid a partir de la valeur réel
        /// </summary>
        /// <param name="realValue"></param>
        void Learn(double realValue);
    }
}
