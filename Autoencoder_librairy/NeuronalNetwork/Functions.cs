using ReseauNeuronal.NeuronalNetwork.IEnumerableExtention;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ReseauNeuronal.NeuronalNetwork
{
    public delegate double ActivateFunction(double x);
    public delegate double WeightInitialisation();
    public abstract class Functions
    {
        static Random rand = new Random(42);
        public static double Sign(double x)
        {
            if (x > 0) return 1;
            else return 0;
        }

        public static double Sigmoid(double x)
        {
            return (1 / (1 + Math.Exp(-x)));
        }

        public static double RandomInit()
        {
            return rand.NextDouble();
        }

        public static double Init0()
        {
            return 0d;
        }

        const int defaultHeigth = 10;
        const int defaultWidth = 10;
        const int defaultImageSize = defaultHeigth * defaultWidth * 3;
        public static IEnumerable<byte[]> SplitBytes(byte[] bytes, int size = defaultImageSize, int splitNumber = 10, int lengthLabel = 1)
        {
            int min = Math.Min(bytes.Length / (size + 1), splitNumber);
            byte[] current = bytes;
            for (int i = 0; i < min; i++)
            {
                current = current.Skip(lengthLabel).ToArray();
                yield return current.Take(size).ToArray();
                current = current.Skip(size).ToArray();
            }
        }

        /// <summary>
        /// number = 0 => first image
        /// number = 1 => second image
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="number"></param>
        /// <param name="size"></param>
        /// <param name="lengthLabel"></param>
        /// <returns></returns>
        public static byte[] GetImageNumber(byte[] bytes, int number, int size = defaultImageSize, int lengthLabel = 1)
        {
            return bytes.Skip(number * (defaultImageSize + lengthLabel) + lengthLabel).Take(size).ToArray();
        }

        /// <summary>
        /// on assum les dimmension etc.. (32*32) 3 couleurs
        /// </summary>
        /// <param name="bytes"></param>
        public static byte[] ShadesOfGray(byte[] bytes)
        {
            var output = new byte[bytes.Length / 3];
            var newSize = defaultImageSize / 3;
            for (int i = 0; i < output.Length; i++)
            {
                output[i] = Convert.ToByte((bytes[i] + bytes[i + newSize] + bytes[i + 2 * newSize]) / 3);
            }
            return output;
        }

        public static IEnumerable<double[]> NormalizeDS(IEnumerable<byte[]> bytes, int maxValue = 255, bool isAlreadyGrey = false)
        {
            foreach (var bytesArray in bytes)
            {
                yield return NormalizeRow(bytesArray, maxValue, isAlreadyGrey);
            }
        }

        public static double[] NormalizeRow(byte[] bytesArray, int maxValue = 255, bool isAlreadyGrey = false)
        {
            byte[] bytes;
            if (isAlreadyGrey)
                bytes = bytesArray;
            else bytes = ShadesOfGray(bytesArray);
            return bytes.Select(x => ((double)x / maxValue)).ToArray();
        }

        public static IEnumerable<byte[]> UnNormalizeDs(double[][] outputs, int maxValue = 255)
        {
            foreach (var output in outputs)
            {
                yield return UnNormalizeRow(output, maxValue);
            }
        }

        public static byte[] UnNormalizeRow(double[] outputs, int maxValue = 255)
        {
            return outputs.Select(x => Convert.ToByte(x * maxValue)).ToArray();
        }

        public static void SaveImgGrey(byte[] bytes, string name = @"output\img.jpg")
        {
            Bitmap empty = new Bitmap(defaultWidth, defaultHeigth);
            for (int x = 0; x < defaultWidth; x++)
            {
                for (int y = 0; y < defaultHeigth; y++)
                {
                    var pos = x + y * defaultWidth;
                    var grey = bytes[pos];
                    Color c = Color.FromArgb(grey, grey, grey);
                    empty.SetPixel(x, y, c);
                }
            }
            empty.Save(name, System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        public static void SaveImg(byte[] bytes, string name = @"output\img.jpg")
        {
            Bitmap empty = new Bitmap(defaultWidth, defaultHeigth);
            const int multiply = defaultWidth * defaultHeigth;
            for (int y = 0; y < defaultWidth; y++)
            {
                for (int x = 0; x < defaultHeigth; x++)
                {
                    var red = bytes[y + x * defaultWidth];
                    var green = bytes[y + x * defaultWidth + multiply];
                    var blue = bytes[y + x * defaultWidth + multiply*2];
                    //var grey = bytes[pos];
                    Color c = Color.FromArgb(red, green, blue);
                    empty.SetPixel(x, y, c);
                }
            }
            empty.Save(name, System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        public static IEnumerable<byte> GetAllPixelGrey(Bitmap bmp)
        {
            for (int i = 0; i < bmp.Height; i++)
                for (int j = 0; j < bmp.Width; j++)
                {
                    var pixel = bmp.GetPixel(j, i);
                    byte greyValue = Convert.ToByte((pixel.R + pixel.G + pixel.B) / 3);
                    yield return greyValue;
                }
        }

        public static IEnumerable<(byte R, byte G, byte B)> GetAllPixelColo(Bitmap bmp)
        {
            for (int i = 0; i < bmp.Height; i++)
                for (int j = 0; j < bmp.Width; j++)
                {
                    var pixel = bmp.GetPixel(j, i);
                    yield return (pixel.R, pixel.G, pixel.B);
                }
        }

        public static IEnumerable<double> MatriceSquareDifference(double[] a, double[] b)
        {
            return a.ZipIteration(b).Select(x => Math.Pow(x.Item1 - x.Item2, 2));
        }
    }
}
