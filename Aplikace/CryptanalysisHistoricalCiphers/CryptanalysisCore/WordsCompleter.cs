using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtensionMethods;

namespace CryptanalysisCore
{
    class WordsCompleter
    {
        /// <summary>
        /// Původní nemodifikovaná zašifrovaná slova.
        /// </summary>
        private string[] originWords;

        /// <summary>
        /// Původní nemodifikované substituce.
        /// </summary>
        private Dictionary<char, char> originSubs;

        /// <summary>
        /// Slovník slov, nad kterým pracujeme.
        /// </summary>
        private Dictionary<int, string[]> dictionary;

        /// <summary>
        /// Slova s vyznačením již nalezených substitucí
        /// </summary>
        private string[] modifWords;

        /// <summary>
        /// Modifikované substituce.
        /// </summary>
        private Dictionary<char, char> substitutions;

        public WordsCompleter(string[] words, Dictionary<char, char> substitutions, Storage.Languages language)
        {
            this.originWords = words;
            this.originSubs = substitutions;
            this.substitutions = substitutions;
            dictionary = Dictionary.Create(Storage.GetLangChar(language).Dictionary, x => x.Length);
            
        }

        public Dictionary<char, char> Complete()
        {
            while (true)
            {
                modifWords = GetSubsWords(originWords, substitutions);
                var testWords = GetWords(30);
                var simWords = GetSimilarWords(testWords);
                var onePossPairs = GetOnePossPairs(simWords);

                if (onePossPairs.Count == 0)
                    break;

                var certainPairs = GetCertainPairs(onePossPairs);
                substitutions = TextAnalysis.GetLetterSubst(certainPairs, substitutions);
            }
            
            return substitutions;
        }

        private Dictionary<string, string> GetCertainPairs(Dictionary<string, string> pairs)
        {
            WordsFilter filter = new WordsFilter(pairs);
            var res = filter.Filter(AreWordsSimilar);
            return res;
        }

        private bool AreWordsSimilar(KeyValuePair<string, string> pair1, KeyValuePair<string, string> pair2)
        {
            var subst1 = TextAnalysis.GetLettersSubst(pair1);
            var subst2 = TextAnalysis.GetLettersSubst(pair2);
            return TextAnalysis.AreSubstMatch(subst1, subst2);
        }

        /// <summary>
        /// Vrátí dvojice slov, pro které existuje jen jedno podobné slovo
        /// </summary>
        /// <param name="similarWords"></param>
        /// <returns></returns>
        private Dictionary<string, string> GetOnePossPairs(Dictionary<string, string[]> similarWords)
        {
            var result = similarWords.Where(x => x.Value.Length == 1).ToDictionary(x => x.Key, x => x.Value[0]);
            return result;
        }

        /// <summary>
        /// Pro všechna slova vrátí pole podobných slov
        /// </summary>
        /// <param name="words"></param>
        /// <returns></returns>
        private Dictionary<string, string[]> GetSimilarWords(Dictionary<string, string> words)
        {
            Dictionary<string, string[]> simWords = new Dictionary<string, string[]>();

            foreach (var pair in words)
                simWords[pair.Value] = GetSimilarWords(pair.Key);


            return simWords;
        }

        /// <summary>
        /// Vrátí všechna slova, která mohou odpovídat předanému slovu
        /// s některými neidentifikovanými písmeny
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        private string[] GetSimilarWords(string word)
        {
            if (!dictionary.ContainsKey(word.Length))
                return new string[] { };

            List<string> similarWords = new List<string>();
            string[] testWords = dictionary[word.Length];  

            foreach (string testWord in testWords)
            {
                if (AreWordsSimilar(testWord, word))
                    similarWords.Add(testWord);
            }

            return similarWords.ToArray();
        }  
      

        /// <summary>
        /// Otestuje, zda se může nekompletní slovo doplnit na kompletní
        /// </summary>
        /// <param name="completeWord"></param>
        /// <param name="incompleteWord"></param>
        /// <returns></returns>
        private bool AreWordsSimilar(string completeWord, string incompleteWord)
        {
            for (int i = 0; i < completeWord.Length; i++)
            {
                char currChar = incompleteWord[i];
                if (currChar != Analyse.Unknown && completeWord[i] != currChar)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Vrátí slova s požadovanou minimální délkou a minimálním 
        /// počtem neidentifikovaných písmen. Zároveň odstraní slova,
        /// která jsou identifikována úplně. 
        /// </summary>
        /// <param name="minLength"></param>
        /// <param name="minLetterIdentified"></param>
        /// <returns></returns>
        private Dictionary<string, string> GetWords(int maximum)
        {
            Dictionary<string, string> resultWords = new Dictionary<string, string>();

            for (int i = 0; i < modifWords.Length; i++)
            {
                string modifWord = modifWords[i];

                int unknownCount = modifWord.Count(Analyse.Unknown);
                if (unknownCount > 0 && unknownCount < (modifWord.Length / 2))
                {
                    resultWords[modifWord] = originWords[i];
                }
            }

            resultWords = resultWords.OrderBy(x => x.Key.Count(Analyse.Unknown)).Take(maximum).ToDictionary(x => x.Key, x => x.Value);

            return resultWords;
        }

        /// <summary>
        /// Pomocí substitucí zamění písmena ve slovech, pokud substituce dané 
        /// písmeno neobsahuje, vloží na jeho místo zástupný symbol.
        /// </summary>
        /// <param name="words"></param>
        /// <param name="substitutions"></param>
        /// <returns></returns>
        private string[] GetSubsWords(string[] words, Dictionary<char, char> substitutions)
        {
            string[] modifWords = new string[words.Length];

            for (int i = 0; i < words.Length; i++)
            {
                string currWord = words[i];
                char[] modifWord = new char[currWord.Length];

                for (int j = 0; j < currWord.Length; j++)
                {
                    char currChar = currWord[j];
                    if (substitutions.ContainsKey(currChar))
                    {
                        modifWord[j] = substitutions[currChar];
                    }
                    else
                    {
                        modifWord[j] = Analyse.Unknown;
                    }
                    modifWords[i] = new string(modifWord);
                }
            }

            return modifWords;
        }
    }
}
