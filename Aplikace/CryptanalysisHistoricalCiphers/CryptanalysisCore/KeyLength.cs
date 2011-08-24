using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtensionMethods;

namespace CryptanalysisCore
{
    public class KeyLength
    {
        public int[] GetKeyLength(string ciphertext)
        {
            ciphertext = Analyse.NormalizeText(ciphertext, Analyse.TextTypes.WithoutSpacesLower);
            Dictionary<string, List<int>> indexes = new Dictionary<string, List<int>>();

            for (int length = Vigenere.MaxKeyLength; length > 5; length--)
                indexes = FindSameStrings(ciphertext, length, indexes);

            var divisors = GetDivisors(indexes);
            var topKeys = GetTopLengthsKey(divisors);

            return topKeys;
        }

        private int[] GetTopLengthsKey(Dictionary<string, List<int>> divisors)
        {
            Dictionary<int, int> counter = new Dictionary<int, int>();

            foreach (var pair in divisors)
            {
                foreach (var divisor in pair.Value)
                {
                    if (counter.ContainsKey(divisor))
                        //counter[divisor] += pair.Key.Length;
                        counter[divisor]++;
                    else
                        //counter[divisor] = pair.Key.Length;
                        counter[divisor] = 1;
                }
            }

            var result = counter.OrderByDescending(x => (25 + x.Key) * x.Value).Select(x => x.Key).ToArray();
            return result;
        }

        private Dictionary<string, List<int>> GetDivisors(Dictionary<string, List<int>> indexes)
        {
            Dictionary<string, List<int>> divisors = new Dictionary<string, List<int>>();

            foreach (var index in indexes)
            {
                int[] inter = Maths.AllDivisors(index.Value[1] - index.Value[0]);
                for (int i = 1; i < index.Value.Count - 1; i++)
                {
                    inter = inter.Intersect(Maths.AllDivisors(Math.Abs(index.Value[i] - index.Value[i + 1]))).ToArray();
                }

                var resInter = inter.Where(x => x < 30).ToList();
                divisors[index.Key] = resInter;
            }

            return divisors;
        }

        private Dictionary<string, List<int>> FindSameStrings(string text, int length, Dictionary<string, List<int>> indexes)
        {
            for (int startIndex = 0; startIndex < text.Length - length; startIndex++)
            {
                string cutString = text.Substring(startIndex, length);
                indexes.SafeAdd(cutString, startIndex);
            }

            indexes = indexes.Where(x => x.Value.Count > 1).ToDictionary(x => x.Key, x => x.Value);
            return indexes;
        }

        private int GreatestCommonDivisor(int a, int b)
        {
            int Remainder;

            while (b != 0)
            {
                Remainder = a % b;
                a = b;
                b = Remainder;
            }

            return a;
        }
    }
}
