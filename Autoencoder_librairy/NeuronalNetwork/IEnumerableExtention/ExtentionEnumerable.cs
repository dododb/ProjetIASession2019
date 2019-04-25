using System;
using System.Collections.Generic;
using System.Text;

namespace ReseauNeuronal.NeuronalNetwork.IEnumerableExtention
{
    public static class ExtentionEnumerable
    {
        public static IEnumerable<(T1, T2)> ZipIteration<T1, T2>(
            this IEnumerable<T1> col1, IEnumerable<T2> col2)
        {
            if (col1 == null)
                throw new ArgumentNullException("col1");
            if (col2 == null)
                throw new ArgumentNullException("col2");
            
            using (IEnumerator<T1> enumerator1 = col1.GetEnumerator())
                using (IEnumerator<T2> enumerator2 = col2.GetEnumerator())
                    while (enumerator1.MoveNext() && enumerator2.MoveNext())
                    {
                        yield return (enumerator1.Current, enumerator2.Current);
                    }
        }

        public static void Deconstruct<T1,T2>(this KeyValuePair<T1, T2> keyValue, out T1 key, out T2 value)
        {
            key = keyValue.Key;
            value = keyValue.Value;
        }
    }
}
