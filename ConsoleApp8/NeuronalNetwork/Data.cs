using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Perceptron.NeuronalNetwork
{
    class Data
    {
        public double data1 { get; set; }
        public double data2 { get; set; }
        public double label { get; set; }

        public static Data CreateFromArray(double[] array)
        {
            return new Data { data1 = array[0], data2 = array[1], label = array[2] };
        }

        public static List<Data> CreateFromDataSet(double[][] ds)
        {
            var datas = from data in ds
                        select CreateFromArray(data);
            return datas.ToList();

        }
    }
}
