using ReseauNeuronal.NeuronalNetwork.flux;
using ReseauNeuronal.NeuronalNetwork.IEnumerableExtention;
using ReseauNeuronal.NeuronalNetwork.neurone;
using ReseauNeuronal.NeuronalNetwork.sauvegarde;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;

namespace ReseauNeuronal.NeuronalNetwork.Reseau
{
    class Layer : AbstractLayerReceiver<Perceptron>, ILayerSender
    {
        public IEnumerable<IDataSender> Senders => layer;

        public Layer(int nbPerceptron, Func<Perceptron> func)
        {
            for (int i = 0; i < nbPerceptron; i++) layer.Add(func());
        }

        public Layer(List<Perceptron> perceptrons)
        {
            layer = perceptrons;
        }

        public void Learn(IEnumerable<double> labels)
        {
            //foreach (var (perceptron, label) in layer.ZipIteration(labels).AsParallel())
            //{
            //    perceptron.Learn(label);
            //    //ThreadPool.QueueUserWorkItem(new WaitCallback(perceptron.Learn), label);
            //}

            layer.ZipIteration(labels).AsParallel().ForAll(x => x.Item1.Learn(x.Item2));
            //using (var countdownEvent = new CountdownEvent(threadCount))
            //{
            //    foreach (var (perceptron, label) in layer.ZipIteration(labels).AsParallel())
            //    {
            //        //perceptron.Learn(label);
            //        ThreadPool.QueueUserWorkItem(perceptron.Learn, label);
            //    }

            //    countdownEvent.Wait();
            //}
            //if (Sender is Layer l) l.Learn(labels);
        }

        public double[] Predict()
        {
            return layer.AsParallel().Select(x => x.Value).ToArray();
            //return layer.Select(x => x.Value).ToArray();
        }

        public static void Join(IEnumerable<Layer> layers)
        {
            var layerPre = layers.First();
            foreach(var layer in layers)
            {
                if (layer == layerPre) continue;
                layer.ConnectTo(layerPre);
                layerPre = layer;
            }
        }

        public LayerSauvegarde Sauvegarde()
        {
            return new LayerSauvegarde(this);
        }
    }
}
