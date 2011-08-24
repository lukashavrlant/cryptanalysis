
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryptanalysisCore
{
    class WordsFilter
    {
        public delegate bool FilterFunction(KeyValuePair<string, string> word1, KeyValuePair<string, string> word2);

        /// <summary>
        /// Obsahuje nalezené unikátní dvojice
        /// </summary>
        private Dictionary<string, string> uniquePairs;

        /// <summary>
        /// Obsahuje nejlepší dvojice
        /// </summary>
        public Dictionary<string, string> BestPairs
        { get; private set; }

        /// <summary>
        /// True v případě, že bylo nalezeno více odpovídajících si dvojic
        /// False v případě, že byla nalezena jen jedna.
        /// </summary>
        public bool Checked
        { get; private set; }

        public WordsFilter(Dictionary<string, string> uniquePairs)
        {
            this.uniquePairs = uniquePairs;
        }

        /// <summary>
        /// Vyfiltruje z unikátních dvojic ty dvojice, které nesedí s ostatními.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> Filter(FilterFunction filterFunction)
        {
            var pairsCount = CountMatchesPairs(filterFunction);
            var bestCountPairs = TakeBestCountPairs(pairsCount);
            var bestPair = TakeBestPairs(bestCountPairs);
            return bestPair;
        }

        /*public Dictionary<string, string> Filter()
        {
            return Filter(ArePairsMatch);
        }*/

        /// <summary>
        /// Na základě předaných indexů vybere z unikátních dvojic ty nejlepší.
        /// </summary>
        /// <param name="bestIndexes"></param>
        /// <returns></returns>
        private Dictionary<string, string> TakeBestPairs(int[] bestIndexes)
        {
            int counter = 0;
            Dictionary<string, string> bestPairs = new Dictionary<string,string>();

            foreach (var pair in uniquePairs)
            {
                if (bestIndexes.Contains(counter))
                    bestPairs[pair.Key] = pair.Value;

                counter++;
            }

            BestPairs = bestPairs;
            return bestPairs;
        }

        /// <summary>
        /// Vrátí indexy nejlepších dvojic
        /// </summary>
        /// <param name="pairsCount"></param>
        /// <returns></returns>
        private int[] TakeBestCountPairs(Dictionary<int, int> pairsCount)
        {
            var taken = pairsCount.OrderByDescending(x => x.Value).Take(1).ToArray();
            int max = taken.Length > 0 ? taken[0].Value : 0;
            Checked = max < 2 ? false : true;
            return pairsCount.Where(x => x.Value == max).Select(x => x.Key).ToArray();
        }

        /// <summary>
        /// Spočítá, kolik unikátních dvojic si navzájem neodporuje.
        /// </summary>
        /// <returns></returns>
        private Dictionary<int, int> CountMatchesPairs(FilterFunction filterFunction)
        {
            Dictionary<int, int> pairsCounter = new Dictionary<int, int>();
            int index = 0, counter;

            foreach (var pair in uniquePairs)
            {
                counter = 0;

                foreach (var testPair in uniquePairs)
                {
                    if (filterFunction(pair, testPair))
                        counter++;
                }

                pairsCounter[index++] = counter;
            }

            return pairsCounter;
        }
    }
}
