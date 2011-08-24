using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtensionMethods;

namespace CryptanalysisCore
{
    public static class Combinatorics
    {
        /// <summary>
        /// Vrací permutaci všech prvků seznamu
        /// Nekontrolují se nijak duplicity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="set">Jakákoliv množina prvků</param>
        /// <returns>Seznam permutací</returns>
        public static List<List<T>> Permutation<T>(List<T> set)
        {
            if (set.Count == 1)
                return new List<List<T>>() { set };

            List<List<T>> result = new List<List<T>>();

            foreach (List<T> l in Permutation(set.Rest()))
                result.AddRange(l.Insert(set[0]));

            return result;
        }

        /// <summary>
        /// Vloží prvek na všechny pozice seznamu
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="set"></param>
        /// <param name="item">Prvek, který chceme vložit do seznamu</param>
        /// <returns>Seznam seznamů s vloženým prvkem</returns>
        private static List<List<T>> Insert<T>(this List<T> set, T item)
        {
            List<List<T>> newSet = new List<List<T>>();

            for (int i = 0; i < set.Count + 1; i++)
            {
                newSet.Add(new List<T>(set));
                newSet[i].Insert(i, item);
            }

            return newSet;
        }

        /// <summary>
        /// Vrátí variace k-tého řádu z daného seznamu prvků
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="set">Libovolná množina prvků</param>
        /// <param name="k">Kolikati prvkové mají být jednotlivé variace</param>
        /// <returns>Seznam variací</returns>
        public static List<List<T>> Variations<T>(List<T> set, int k)
        {
            if (set.Count == k)
                return Permutation(set);

            List<List<T>> variations = new List<List<T>>();
            List<T> var;

            string binary = new string('0', set.Count);

            while (binary.Count(x => x == '1') < set.Count)
            {
                binary = BinaryAdd(binary);
                if (binary.Count(x => x == '1') == k)
                {
                    var = new List<T>();
                    for (int i = 0; i < set.Count; i++)
                    {
                        if (binary[i] == '1')
                            var.Add(set[i]);
                    }
                    variations.Add(var);
                }
            }

            return variations;
        }

        /// <summary>
        /// Vrátí seznam variací s opakováním
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="set"></param>
        /// <param name="k">Kolikati prvkové mají být jednotlivé variace</param>
        /// <returns>Seznam variací</returns>
        public static List<List<T>> VariationsWithRepetition<T>(List<T> set, int k)
        {
            List<List<T>> list = new List<List<T>>();
            List<T> var;

            if (k == 0)
            {
                list.Add(new List<T>());
            }
            else
            {
                List<List<T>> l = VariationsWithRepetition(set, k - 1);
                foreach (T c in set)
                {
                    foreach (List<T> s in l)
                    {
                        var = new List<T>() { c };
                        var.AddRange(s);
                        list.Add(var);
                    }
                }
            }

            return list;
        }


        /// <summary>
        /// Přičte k binárnímu číslu jedničku
        /// </summary>
        /// <param name="s">Řetězec reprezentující binární číslo</param>
        /// <returns>Binární číslo zvýšené o jedničku</returns>
        private static string BinaryAdd(string s)
        {
            char[] binary = s.ToCharArray();

            for (int i = s.Length - 1; i >= 0; i--)
            {
                if (binary[i] == '1')
                    binary[i] = '0';
                else
                {
                    binary[i] = '1';
                    break;
                }
            }

            return new string(binary);
        }
    }
}
