using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReseauNeuronal.NeuronalNetwork
{
    class Data
    {
        public double Data1 { get; set; }
        public double Data2 { get; set; }
        public double Label { get; set; }

        public static Data CreateFromArray(double[] array)
        {
            return new Data { Data1 = array[0], Data2 = array[1], Label = array[2] };
        }

        public static List<Data> CreateFromDataSet(double[][] ds)
        {
            var datas = from data in ds
                        select CreateFromArray(data);
            return datas.ToList();

        }
    }
}
