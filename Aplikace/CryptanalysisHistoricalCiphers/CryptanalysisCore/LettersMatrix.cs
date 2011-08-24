using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtensionMethods;

namespace CryptanalysisCore
{
    class LettersMatrix
    {
        private double[,] matrix;

        public int limit;

        private Dictionary<string, string> idealPairsCache = null;

        struct temp
        {
            public string cipherkey;
            public string maybekey;
            public double probability;

            public temp(string cipherkey, string maybekey, double probability)
            {
                this.cipherkey = cipherkey;
                this.maybekey = maybekey;
                this.probability = probability;
            }
        }

        public Dictionary<string, string> IdealPairs
        {
            get
            {
                if (idealPairsCache == null)
                    idealPairsCache = GetIdealPairs();

                return idealPairsCache;
            }
        }

        public LettersMatrix()
        {
            limit = Analyse.AlphabetLetterCount;
            matrix = new double[limit, limit];
            for (int i = 0; i < limit; i++)
            {
                for (int j = 0; j < limit; j++)
                {
                    matrix[i, j] = 0;
                }
            }
        }

        /// <summary>
        /// Vrátí index výskytu daných dvou písmen v matici
        /// </summary>
        /// <param name="ciphertext">Klíč ze šifrového textu</param>
        /// <param name="langtext">Klíč z vlastnosti jazyka</param>
        /// <returns>Index výskytu</returns>
        private double GetOccurence(string ciphertext, string langtext)
        {
            if (ciphertext == " " || langtext == " ")
                return 0;

            return matrix[(int)(ciphertext.ToLower()[0] - 'a'), (int)(langtext.ToLower()[0] - 'a')];
        }

        /// <summary>
        /// Nastaví index výskytu daných dvou písmen v matici
        /// </summary>
        /// <param name="ciphertext">Klíč ze šifrového textu</param>
        /// <param name="langtext">Klíč z vlastnosti jazyka</param>
        /// <param name="value">Nastavovaná hodnota</param>
        private void SetOccurence(string ciphertext, string langtext, double value)
        {
            if (ciphertext == " " || langtext == " ")
                return;

            matrix[(int)(ciphertext.ToLower()[0] - 'a'), (int)(langtext.ToLower()[0] - 'a')] = value;
        }

        /// <summary>
        /// Připočte k indexu výskytu na daných dvou písmenech předanou hodnotu
        /// </summary>
        /// <param name="ciphertext"></param>
        /// <param name="langtext"></param>
        /// <param name="value"></param>
        public void Add(string ciphertext, string langtext, double value)
        {
            SetOccurence(ciphertext, langtext, value + GetOccurence(ciphertext, langtext));
        }

        /// <summary>
        /// Vrací dvojici nejvíce si odpovídajících znaků
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetIdealPairs()
        {
            Dictionary<string, string> idealPairs = new Dictionary<string, string>();
            Analyse.DoAlphabet(x => idealPairs[x.ToString()] = GetIdealPair(x.ToString()));
            return idealPairs;
        }

        private string GetIdealPair(string key)
        {
            int cipherIndex = (int)key[0] - 'a';
            double minimum = matrix[cipherIndex, 0];
            int langIndex = 0;

            for (int i = 0; i < limit; i++)
            {
                if (matrix[cipherIndex, i] < minimum)
                {
                    minimum = matrix[cipherIndex, i];
                    langIndex = i;
                }
            }

            return ((char)('a' + langIndex)).ToString();
        }

        public double Success()
        {
            return Success(Storage.Letters);
        }

        public double Success(string[] letters)
        {
            double success = 0;
            double fraction = (double) 100 / (double) letters.Length;

            letters.ForEach(x =>
                {
                    if (IdealPairs[x] == x)
                        success += fraction;
                });

            return Math.Round(success);
        }

        public double Success(string[] letters, int toleration)
        {
            double success = 0;
            double fraction = (double)100 / (double)letters.Length;
            var probabilites = GetProbabilites(letters, toleration);

            foreach (var prob in probabilites)
            {
                if (prob.Value.Find(prob.Key))
                {
                    success += fraction;
                }
            }

            return success;
        }

        public int SmartSuccess(string[] letters, int toleration)
        {
            double success = 0;
            double fraction = (double)100 / (double)letters.Length;
            var probabilites = GetProbabilites(letters, toleration);

            var possKeys = probabilites.Select(x => x.Value[0]).Distinct().ToArray();


            success = 100 * ((double)possKeys.Length / (double)letters.Length);

            if (success < 99)
            {
                var doubleLetters = new List<string>();
                var firstChoises = probabilites.Select(x => x.Value[0]).ToArray();
                foreach (string letter in firstChoises)
                {
                    if (firstChoises.Count(x => x == letter) > 1 && !doubleLetters.Contains(letter))
                        doubleLetters.Add(letter);
                }

                var pairDoubleLetters = new Dictionary<string, string[]>();

                doubleLetters.ForEach(x =>
                    {
                        pairDoubleLetters[x] = probabilites.Where(prob => prob.Value[0] == x).Select(prob => prob.Key).ToArray();
                    });

                foreach (var pair in pairDoubleLetters)
                {
                    var dict = new Dictionary<string, double>();

                    double a, b;
                    foreach (var cipherLetter in pair.Value)
                    {
                        a = GetOccurence(cipherLetter, probabilites[cipherLetter][0]);
                        b = GetOccurence(cipherLetter, probabilites[cipherLetter][1]);
                        dict[cipherLetter] = Math.Abs(a - b);
                    }

                    string secondKey = dict.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value).ToArray()[1].Key;

                    string temp = probabilites[secondKey][0];
                    probabilites[secondKey][0] = probabilites[secondKey][1];
                    probabilites[secondKey][1] = temp;
                }
            }

            double matchSucces = 0;
            probabilites.ForEach(x =>
                {
                    if (x.Key == x.Value[0])
                        matchSucces += fraction;
                });

            return (int)Math.Round(matchSucces);
        }

        public int UniqueProb(string[] letters, int toleration)
        {
            var probabilites = GetProbabilites(letters, toleration);

            List<string> union = new List<string>();
            foreach (var pair in probabilites)
                union = union.Union(pair.Value).ToList();

            return (int)Math.Round(100 * ((double)union.Count / (double)(probabilites.Count * toleration)));
        }

        public double Success(int toleration)
        {
            return Success(Storage.Letters, toleration);
        }

        private Dictionary<string, string[]> GetProbabilites(string[] letters, int toleration)
        {
            var probabilities = new Dictionary<string, string[]>();

            letters.ForEach(letter =>
            {
                string tempLetter = letter.ToString();
                probabilities[tempLetter] = GetRow(tempLetter).OrderBy(x => x.Value).Take(toleration).Select(x => x.Key).ToArray();
            });

            return probabilities;
        }

        private KeyValuePair<string, double>[] GetRow(string cipherletter)
        {
            var pairs = new KeyValuePair<string, double>[limit];

            for (int i = 0; i < limit; i++)
            {
                pairs[i] = new KeyValuePair<string,double>(((char)(i + 'a')).ToString(), 
                    GetOccurence(cipherletter, ((char)('a' + i)).ToString()));
            }

            return pairs;
        }

        public void MergeMatrix(Dictionary<string, double> ciphertext, Dictionary<string, double> langtext)
        {
            MergeMatrix(ciphertext, langtext, 1);
        }

        public void MergeMatrix(Dictionary<string, double> ciphertext, Dictionary<string, double> langtext, double coefficient)
        {
            foreach (var cipherPair in ciphertext)
            {
                foreach (var langPair in langtext)
                {
                    Add(cipherPair.Key, langPair.Key, Math.Abs(cipherPair.Value - langPair.Value) * coefficient);
                }
            }
        }
    }
}
