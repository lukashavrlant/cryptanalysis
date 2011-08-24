using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtensionMethods;

namespace CryptanalysisCore.Filters
{
    class SamePatternAttack
    {
        public Dictionary<char, char> GetKey(string ciphertext, LangCharacteristic lang, Dictionary<char, char> topLetters)
        {
            int wordLength = 12;
            string[] testWords = lang.TopWords.Where(x => x.Length == wordLength).ToArray();
            string[] templates = GetTemplates(ciphertext, wordLength).ToArray();

            var testWordsHashes = GetWordsHashes(testWords);
            var templatesHashes = GetWordsHashes(templates);

            var samePattern = GetWordsWithSamePatterns(testWordsHashes, templatesHashes); //1599
            var substitutions = GetSubstitutions(samePattern); // 5119
            var filteredSubs = FilterSubstitutions(substitutions, topLetters); // 373
            var matchSubs = GetMatchSubs(filteredSubs);
            var maxMatch = matchSubs.Select(x => x.Value.Count).Max();
            var finalSubs = matchSubs.Where(x => x.Value.Count == maxMatch).Select(x => x.Value).ToArray().First().ToArray();
            var finalSubstitution = TextAnalysis.MergeSubstitutions(finalSubs);
            return finalSubstitution;
        }

        public Dictionary<Dictionary<char, char>, List<Dictionary<char, char>>> GetMatchSubs(Dictionary<char, char>[] substitutions)
        {
            Dictionary<Dictionary<char, char>, List<Dictionary<char, char>>> matchSubs = new Dictionary<Dictionary<char, char>, List<Dictionary<char, char>>>();

            foreach (var subs1 in substitutions)
            {
                foreach (var subs2 in substitutions)
                {
                    if (TextAnalysis.AreSubstMatch(subs1, subs2))
                    {
                        matchSubs.SafeAdd(subs1, subs2);
                    }
                }
            }

            return matchSubs;
        }

        /// <summary>
        /// Vyfiltruje substituce podle předaných nejčastějších písmen
        /// </summary>
        /// <param name="substitutions"></param>
        /// <param name="topLetters"></param>
        /// <returns></returns>
        private Dictionary<char, char>[] FilterSubstitutions(Dictionary<char, char>[] substitutions, Dictionary<char, char> topLetters)
        {
            List<Dictionary<char, char>> filteredSubs = new List<Dictionary<char, char>>();

            foreach (var subs in substitutions)
            {
                if (TextAnalysis.AreSubstMatch(subs, topLetters))
                    filteredSubs.Add(subs);
            }

            return filteredSubs.ToArray();
        }



        /*private Dictionary<char, Dictionary<char, char>[]> DivideSubstitutions(Dictionary<char, char>[] substitutions)
        {
            char key = 'e';

            foreach (var subs in substitutions)
            {
                if (subs.ContainsKey(key))
                {

                }
            }

            return null;
        }*/

        /// <summary>
        /// Vrátí substitce všech dvojic
        /// </summary>
        /// <param name="samePatterns"></param>
        /// <returns></returns>
        private Dictionary<char, char>[] GetSubstitutions(Dictionary<string, List<string>> samePatterns)
        {
            List<Dictionary<char, char>> substitutions = new List<Dictionary<char, char>>();

            foreach (var dictWord in samePatterns)
            {
                foreach (var pattern in dictWord.Value)
                {
                    substitutions.Add(TextAnalysis.GetLettersSubst(dictWord.Key, pattern));
                }
            }

            return substitutions.ToArray();
        }

        /// <summary>
        /// Vrátí hashe ke všem slovům
        /// Dvojice slovo->hash
        /// </summary>
        /// <param name="words"></param>
        /// <returns></returns>
        private Dictionary<string, string> GetWordsHashes(string[] words)
        {
            var hashes = new Dictionary<string, string>();

            foreach (string word in words)
            {
                hashes[word] = GetWordHash(word);
            }

            return hashes;
        }

        /// <summary>
        /// Vrátí jednoduchý hash slova, později použitelné u porovnávání vzorů
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        private string GetWordHash(string word)
        {
            int currSubst = 10;
            Dictionary<char, int> subst = new Dictionary<char, int>();
            StringBuilder sb = new StringBuilder();

            foreach (char c in word)
            {
                if (subst.ContainsKey(c))
                {
                    sb.Append(subst[c]);
                }
                else
                {
                    subst[c] = currSubst;
                    sb.Append(currSubst);
                    currSubst++;
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Vrátí slova, která mají stejný vzor
        /// </summary>
        /// <param name="testWords"></param>
        /// <param name="templates"></param>
        /// <returns></returns>
        private Dictionary<string, List<string>> GetWordsWithSamePatterns(Dictionary<string, string> testWordsHashes, Dictionary<string, string> templatesHashes)
        {
            Dictionary<string, List<string>> samePattern = new Dictionary<string, List<string>>();

            foreach (var testWordHash in testWordsHashes)
            {
                foreach (var templateHash in templatesHashes)
                {
                    if (testWordHash.Value == templateHash.Value)
                    {
                        samePattern.SafeAdd(testWordHash.Key, templateHash.Key);
                    }
                }
            }

            return samePattern;
        }

        /// <summary>
        /// Vrátí všechna slova o dané délce
        /// </summary>
        /// <param name="ciphertext"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private string[] GetTemplates(string ciphertext, int length)
        {
            string[] templates = new string[ciphertext.Length - length];

            for (int i = 0; i < ciphertext.Length - length; i++)
            {
                templates[i] = ciphertext.Substring(i, length);
            }

            return templates;
        }

        /*private string[] GetTemplates(string ciphertext, int length)
        {
            int number = (int)Math.Floor((double)ciphertext.Length / (double)length);
            string[] templates = new string[number];

            for (int i = 0; i < number; i++)
            {
                templates[i] = ciphertext.Substring(i * length, length);
            }

            return templates;
        }*/
    }
}
