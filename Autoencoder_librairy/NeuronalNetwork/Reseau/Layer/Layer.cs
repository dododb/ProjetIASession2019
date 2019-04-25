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
    public class Layer : AbstractLayerReceiver<Perceptron>, ILayerSender
    {
        const int maxThread = 5;
        public IEnumerable<IDataSender> Senders => layer;

        private List<List<Perceptron>> layerSplited = new List<List<Perceptron>>();
        

        public Layer(int nbPerceptron, Func<Perceptron> func)
        {
            for (int i = 0; i < nbPerceptron; i++) layer.Add(func());

            //int nbPerceptronPerThread = nbPerceptron / maxThread;
            //for (int i = 1; i <= maxThread; i++)
            //{
            //    layerSplited.Add(layer.Where((x, y) => y < i*nbPerceptronPerThread && y >= (i-1)*nbPerceptronPerThread).ToList());
            //}
        }

        public Layer(List<Perceptron> perceptrons)
        {
            layer = perceptrons;
        }

        public void Learn(IEnumerable<double> labels)
        {

            //layerSplited.ZipIteration(SplitLabels(labels)).AsParallel()
            //    .ForAll(x =>
            //    {
            //        foreach (var (y, z) in x.Item1.ZipIteration(x.Item2))
            //        {
            //            y.Learn(z);
            //        }
            //    });
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

        public IEnumerable<IEnumerable<double>> SplitLabels(IEnumerable<double> labels)
        {
            int nbPerceptronPerThread = layer.Count / maxThread;
            for (int i = 1; i <= maxThread; i++)
            {
                yield return labels.Where((x, y) => y < i * nbPerceptronPerThread && y >= (i - 1) * nbPerceptronPerThread).ToList();
            }
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
