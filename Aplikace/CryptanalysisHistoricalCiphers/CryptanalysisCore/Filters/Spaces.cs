using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtensionMethods;

namespace CryptanalysisCore
{
    public static class Spaces
    {
        private static Dictionary<int, string[]> dictionary;

        public static string Add(string s, Dictionary<int, string[]> dic)
        {
            dictionary = dic;
            string normalizeString = Analyse.NormalizeText(s, Analyse.TextTypes.WithoutSpacesLower);
            var indexes = GetIndexes(normalizeString);
            var words = GetWords(normalizeString, indexes);
            //var fixedWords = FixWords(words);
            return words.Implode(" ");
        }

        private static List<string> FixWords(List<string> words)
        {
            List<string> fixedWords = words.ToList();
            for (int i = 3; i >= 2; i--)
                fixedWords = FixWords(fixedWords.ToArray(), i);

            return fixedWords;
        }

        private static List<string> FixWords(string[] words, int count)
        {
            List<string> fixWords = new List<string>();
            for (int startIndex = 0; startIndex < words.Length; startIndex++)
            {
                string testWord = words.Take(startIndex, count).Implode("");

                if (dictionary.ContainsKey(testWord.Length) && dictionary[testWord.Length].Contains(testWord))
                {
                    fixWords.Add(testWord);
                    startIndex += 2;
                }
                else
                {
                    fixWords.Add(words[startIndex]);
                }
            }

            return fixWords;
        }

        private static List<string> GetWords(string normalizeString, Dictionary<int, int> indexes)
        {
            List<string> words = new List<string>();
            int last = 0;
            foreach (var fromTo in indexes)
            {
                if (last != fromTo.Key)
                    words.Add(normalizeString.Substring(last, fromTo.Key - last));

                words.Add(normalizeString.Substring(fromTo.Key, fromTo.Value));
                last = fromTo.Value + fromTo.Key;
            }
            return words;
        }

        private static Dictionary<int, int> GetIndexes(string normalizeString)
        {
            Dictionary<int, int> identified = new Dictionary<int, int>();
            for (int startIndex = 0; startIndex < normalizeString.Length; startIndex++)
            {
                for (int length = 15; length > 2; length--)
                {
                    string cutWord = normalizeString.Substring(startIndex, Math.Min(length, normalizeString.Length - startIndex));
                    if (dictionary[cutWord.Length].Find(cutWord))
                    {
                        //words.Add(cutWord);
                        identified[startIndex] = Math.Min(length, normalizeString.Length - startIndex);
                        startIndex += length - 1;
                        break;
                    }
                }
            }

            return identified;
        }
    }
}
