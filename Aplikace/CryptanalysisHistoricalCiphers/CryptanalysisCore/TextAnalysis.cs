using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ExtensionMethods;

namespace CryptanalysisCore
{
    public static class TextAnalysis
    {
        /// <summary>
        /// Vrátí dvojice ve tvaru znak:počet výskytů na začátku slova
        /// </summary>
        /// <param name="words">seznam slov</param>
        /// <returns></returns>
        public static Dictionary<string, double> StartLetters(string[] words)
        {
            return SomeLetters(words, word => word[0]);
        }

        /// <summary>
        /// Vrátí dvojice ve tvaru znak:počet výskytů na konci slova
        /// </summary>
        /// <param name="words">seznam slov</param>
        /// <returns></returns>
        public static Dictionary<string, double> EndLetters(string[] words)
        {
            return SomeLetters(words, word => word[word.Length - 1]);
        }

        /// <summary>
        /// Najde ve slově písmeno, které vrátí předaná selektivní funkce
        /// a spočítá, kolikrát se toto písmeno vyskytuje ve všech slov.
        /// Vrátí jako dvojice písmeno:počet výskytů
        /// </summary>
        /// <param name="words">seznam slov</param>
        /// <param name="selectLetter">Selektivní funkce vracející písmeno ze slova</param>
        /// <returns></returns>
        public static Dictionary<string, double> SomeLetters(string[] words, Func<string, char> selectLetter)
        {
            var letters = new Dictionary<char, int>();
            Analyse.DoAlphabet(l => letters[l] = 0);
            words.ForEach(word => letters[selectLetter(word)]++);
            int count = 0;
            letters.ForEach(x => count += x.Value);
            return letters.OrderByDescending(x => x.Value)
                .ToDictionary(x => x.Key.ToString(), x => ((double)x.Value / (double)count) * 100);
        }

        /// <summary>
        /// Vrátí relativní výskyt jednotlivých ngramů v textu.
        /// </summary>
        /// <param name="text">Text, ve kterém se budou hledat ngramy</param>
        /// <param name="length">Délka ngramu. 1+</param>
        /// <returns></returns>
        public static Dictionary<string, double> GetOccurrence(string text, int length)
        {
            Dictionary<string, int> occurrence = new Dictionary<string, int>();
            string ngram;

            for (int i = 0; i < text.Length - length + 1; i++)
            {
                ngram = text.Substring(i, length); 

                if (occurrence.ContainsKey(ngram))
                    occurrence[ngram]++;
                else
                    occurrence[ngram] = 1;
            }

            double multiply = (double)text.Length / 100;

            Dictionary<string, double> relativeOccurrence = new Dictionary<string, double>();
            foreach (KeyValuePair<string, int> occPair in occurrence)
                relativeOccurrence[occPair.Key] = (double)occPair.Value / multiply;

            if (length == 1)
            {
                Analyse.DoAlphabet(x =>
                    {
                        if (!relativeOccurrence.ContainsKey(x.ToString()))
                            relativeOccurrence[x.ToString()] = 0;
                    });
            }

            return relativeOccurrence.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, y => y.Value);
        }

        /// <summary>
        /// Zjistí, zda je dané písmeno z anglické abecedy [a-zA-Z]
        /// </summary>
        /// <param name="character">Jakýkoliv znak</param>
        /// <returns>true: znak anglické abecedy, false: jinak</returns>
        public static bool IsEnglishLetter(char character)
        {
            return (character >= 'a' && character <= 'z') || (character >= 'A' && character <= 'Z');
        }

        /// <summary>
        /// Vrátí řetězec, který obsahuje pouze písmena a mezery,
        /// přičemž nedovolí mít dvě mezery za sebou.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string GetLetters(string s)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in s)
            {
                if (IsEnglishLetter(c))
                    sb.Append(c);
                else
                {
                    if (c == ' ')
                    {
                        if (sb.Length == 0 || sb[sb.Length - 1] != ' ')
                            sb.Append(' ');
                    }
                }
            }

            return sb.ToString();
        }


        /// <summary>
        /// Vrátí průměrný počet výskytů písmen před a po nějakém písmenu.
        /// </summary>
        /// <param name="words"></param>
        /// <returns></returns>
        public static NearbyLetters NearbyLetters(string[] words)
        {
            return NearbyLetters(words, 150);
        }

        /// <summary>
        /// Vrátí průměrný počet výskytů písmen před a po nějakém písmenu.
        /// </summary>
        /// <param name="words"></param>
        /// <param name="partSize">Kolik slov mají mít jednotlivé části,
        /// ve kterých se to bude počítat.</param>
        /// <returns></returns>
        public static NearbyLetters NearbyLetters(string[] words, int partSize)
        {
            Dictionary<char, int> nextLetters = new Dictionary<char, int>();
            Dictionary<char, int> prevLetters = new Dictionary<char, int>();
            List<char> foundNextLetters = new List<char>();
            List<char> foundPrevLetters = new List<char>();
            char letter;

            for (int j = (int)'a'; j <= (int)'z'; j++)
            {
                nextLetters[(char)j] = 0;
                prevLetters[(char)j] = 0;
            }

            for (int part = 0; part <= words.Length / partSize; part++)
            {
                var takenarr = words.Take(part * partSize, partSize);
                for (int j = (int)'a'; j <= (int)'z'; j++)
                {
                    letter = (char)j;

                    foundNextLetters.Clear();
                    foundPrevLetters.Clear();
                    
                    foreach (string word in takenarr)
                    {
                        for (int i = 0; i < word.Length; i++)
                        {
                            if (word[i] == letter)
                            {
                                if ((i < word.Length - 1) && !foundNextLetters.Find(word[i + 1]))
                                    foundNextLetters.Add(word[i + 1]);

                                if ((i > 0) && !foundPrevLetters.Find(word[i - 1]))
                                    foundPrevLetters.Add(word[i - 1]);
                            }
                        }
                    }
                    nextLetters[letter] += foundNextLetters.Count();
                    prevLetters[letter] += foundPrevLetters.Count();
                }
            }

            int count = 0;
            nextLetters.ForEach(x => count += x.Value);
            double multiply = (double)count / 100;
            var resNext = nextLetters.ToDictionary(x => x.Key.ToString(), x => (double)x.Value / multiply);

            count = 0;
            nextLetters.ForEach(x => count += x.Value);
            multiply = (double)count / 100;
            var resPrev = prevLetters.ToDictionary(x => x.Key.ToString(), x => (double)x.Value / multiply);

            return new NearbyLetters(resPrev, resNext);
        }



        /// <summary>
        /// Zjišťuje, jestli je daný otevřený text validní vstup do šifrovací funkce
        /// </summary>
        /// <param name="opentext">Otevřený text</param>
        /// <returns>true - text je v pořádku; false - v textu je nějaká chyba</returns>
        public static bool IsOpentextValid(string opentext)
        {
            return Regex.IsMatch(opentext, @"^[a-z ]*$");
        }

        /// <summary>
        /// Zjišťuje, jestli je daný šifrový text validní vstup do dešifrovací funkce
        /// </summary>
        /// <param name="ciphertext">Šifrový text</param>
        /// <returns>true - text je v pořádku; false - v textu je nějaká chyba</returns>
        public static bool IsCiphertextValid(string ciphertext)
        {
            return Regex.IsMatch(ciphertext, @"^[A-Z ]*$");
        }

        /// <summary>
        /// Převede obyčejný string na pole slov.
        /// </summary>
        /// <param name="text">Libovolný text</param>
        /// <returns>Pole slov.</returns>
        public static string[] ToWords(string text)
        {
            return Analyse.NormalizeText(text, Analyse.TextTypes.WithSpacesLower).Split(' ').Where(x => x.Length > 0).ToArray();
        }

        /// <summary>
        /// Najde ty bigramy, pro které můžeme v seznamu bigramů najít i jejich převrácený tvar
        /// </summary>
        /// <param name="bigrams">Pole bigramů</param>
        /// <returns></returns>
        public static List<string[]> GetSymetricBigrams(string[] bigrams)
        {
            List<string[]> symBigrams = new List<string[]>();

            foreach (string bigram in bigrams)
            {
                if ((Array.Find(bigrams, x => bigram[0] == x[1] && bigram[1] == x[0]) != null) &&
                    symBigrams.Find(pair => pair[0] == bigram || pair[1] == bigram) == null)
                    symBigrams.Add(new string[] { bigram, bigram.ToCharArray().Reverse().ToList().Implode("") });
            }

            return symBigrams;
        }

        /// <summary>
        /// Vrátí slovník reprezentující substituce mezi slovy
        /// </summary>
        /// <param name="wordPairs"></param>
        /// <returns></returns>
        public static Dictionary<char, char> GetLetterSubst(Dictionary<string, string> wordPairs)
        {
            Dictionary<char, char> substitutions = new Dictionary<char, char>();

            foreach (var pair in wordPairs)
            {
                for (int i = 0; i < pair.Key.Length; i++)
                {
                    substitutions[pair.Key[i]] = pair.Value[i];
                }
            }

            return substitutions;
        }

        /// <summary>
        /// Vrátí slovník reprezentující substituce mezi slovy 
        /// sloučený z již existujícími substitucemi
        /// </summary>
        /// <param name="wordPairs"></param>
        /// <param name="substitutions"></param>
        /// <returns></returns>
        public static Dictionary<char, char> GetLetterSubst(Dictionary<string, string> wordPairs, Dictionary<char, char> substitutions)
        {
            Dictionary<char, char> subsPairs = GetLetterSubst(wordPairs);

            substitutions.ForEach(pair => subsPairs[pair.Key] = pair.Value);

            return subsPairs;
        }

        /// <summary>
        /// Zjistí, zda dané dva slovníky představují stejný vzor substitucí.
        /// Kontroluje oba směry. (TODO: Je to vůbec nutné?)
        /// </summary>
        /// <param name="subst1"></param>
        /// <param name="subst2"></param>
        /// <returns></returns>
        public static bool AreSubstMatch(Dictionary<char, char> subst1, Dictionary<char, char> subst2)
        {
            bool res = AreSubstsMatchHelp(subst1, subst2);

            if (res)
                return AreSubstsMatchHelp(subst2, subst1);
            else
                return false;
        }

        /// <summary>
        /// Zjistí, zda dané dva slovníky představují stejný vzor substitucí
        /// </summary>
        /// <param name="subst1"></param>
        /// <param name="subst2"></param>
        /// <returns></returns>
        private static bool AreSubstsMatchHelp(Dictionary<char, char> subst1, Dictionary<char, char> subst2)
        {
            foreach (var pair in subst1)
            {
                if (subst2.ContainsKey(pair.Key) && pair.Value != subst2[pair.Key])
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Vrátí slovník reprezentující substituce ve slovech
        /// </summary>
        /// <param name="pair"></param>
        /// <returns></returns>
        public static Dictionary<char, char> GetLettersSubst(KeyValuePair<string, string> pair)
        {
            Dictionary<char, char> subst = new Dictionary<char, char>();

            for (int i = 0; i < pair.Key.Length; i++)
                subst[pair.Key[i]] = pair.Value[i];

            return subst;
        }

        public static List<string> PolygonAttack(char[] topTextLetters, char[] topLetters, int complementSize)
        {
            var variations = Combinatorics.Variations(topTextLetters.ToList(), topLetters.Length);
            variations.Reverse();
            List<string> matchLetters = new List<string>();
            List<string> notMatch = new List<string>();

            foreach (var variation in variations)
            {
                string cut = variation[0].ToString() + variation[1].ToString();

                var permutations = Combinatorics.Permutation(variation);

                foreach (var permutation in permutations)
                {
                    if (permutation[0] == 'f' && permutation[1] == 'b' && permutation[2] == 'p')
                        complementSize++;

                    if (LettersDiff(permutation.ToArray(), topLetters))
                    {
                        matchLetters.Add(new string(permutation.ToArray()));
                        break;
                    }
                }
            }

            return matchLetters;
        }

        public static bool LettersDiff(char[] letters, char[] topLetters)
        {
            int[,] lettersMatrix = GetDiffMatrix(letters);
            int[,] topLettersMatrix = GetDiffMatrix(topLetters);


            return AreMatrixEqual(lettersMatrix, topLettersMatrix);
        }

        public static bool AreMatrixEqual(int[,] matrix1, int[,] matrix2)
        {
            for (int i = 0; i < matrix1.GetLength(0); i++)
            {
                for (int j = 0; j < matrix1.GetLength(1); j++)
                {
                    if (matrix1[i, j] != matrix2[i, j])
                        return false;
                }
            }

            return true;
        }

        public static int[,] GetDiffMatrix(char[] letters)
        {
            int[,] matrix = new int[letters.Length, letters.Length];

            for (int i = 0; i < letters.Length; i++)
            {
                for (int j = 0; j < letters.Length; j++)
                {
                    matrix[i, j] = Analyse.MinDistance(letters[i], letters[j]);
                }
            }

            return matrix;
        }
    }
}
