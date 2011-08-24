using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using CryptanalysisCore;
using ExtensionMethods;

namespace Statistics
{
    public static class Frequency
    {
        public static void SaveData(string language)
        {
            string fileSavePath = Storage.ConfPath + language + "/" + Storage.FrequencyFile;
            string fileTextsPath = Storage.ConfPath + language + "/" + Storage.TextsFile;

            string normText = Analyse.NormalizeText(File.ReadAllText(fileTextsPath), Analyse.TextTypes.WithSpacesLower);
            /*XmlDocument xmldoc = GetAnalyse(normText);
            xmldoc.Save(fileSavePath);*/

            File.WriteAllText(Storage.ConfPath + language + "/" + "topwords.txt",  TopWords(TextAnalysis.ToWords(normText), 1).Implode(" "));
        }

        public static string[] TopWords(string[] words, int minOccCount)
        {
            Dictionary<string, int> occur = new Dictionary<string, int>();

            foreach (string word in words)
            {
                if (occur.ContainsKey(word))
                    occur[word]++;
                else
                    occur[word] = 1;
            }

            var result = occur.Where(x => x.Value >= minOccCount).OrderByDescending(x => x.Value).Select(x => x.Key).ToArray();
            return result;
        }

        private static XmlDocument GetAnalyse(string text)
        {
            string[] words = TextAnalysis.ToWords(text);
            string textWithoutSpaces = text.Remove(" ");
            var startLetters = TextAnalysis.StartLetters(words);
            var endLetters = TextAnalysis.EndLetters(words);
            var letters = TextAnalysis.GetOccurrence(textWithoutSpaces, 1);
            var bigrams = TextAnalysis.GetOccurrence(textWithoutSpaces, 2);
            var trigrams = TextAnalysis.GetOccurrence(textWithoutSpaces, 3);
            var nearbyLetters = TextAnalysis.NearbyLetters(words);
            var nextLetters = nearbyLetters.NextLetters;
            var prevLetters = nearbyLetters.PreviousLetters;

            XmlDocument xmldoc = new XmlDocument();
            XmlElement root = xmldoc.CreateElement(Storage.FrequencyFile);

            root.AppendChild(GetPart(startLetters, "startLetters", xmldoc));
            root.AppendChild(GetPart(endLetters, "endLetters", xmldoc));
            root.AppendChild(GetPart(letters, "letters", xmldoc));
            root.AppendChild(GetPart(bigrams, "bigrams", xmldoc));
            root.AppendChild(GetPart(trigrams, "trigrams", xmldoc));
            root.AppendChild(GetPart(nextLetters, "nextLetters", xmldoc));
            root.AppendChild(GetPart(prevLetters, "prevLetters", xmldoc));

            xmldoc.AppendChild(root);
            return xmldoc;
        }

        /*private static Dictionary<string, double> GetNearbyLetters(string[] words, Func<string[], Dictionary<string, double>> function)
        {
            Dictionary<string, double> nextLetters = new Dictionary<string, double>();
            Analyse.DoAlphabet(l => nextLetters[l.ToString()] = 0);
            int paragraphNumbers = (int)((double)words.Length / 1000);
            var splittedArray = words.Split(paragraphNumbers);

            Dictionary<string, double> temp;
            foreach (string[] sArray in splittedArray.Take(paragraphNumbers - 1).ToArray())
            {
                temp = function(sArray);
                temp.ForEach(pair => nextLetters[pair.Key] += pair.Value);
            }

            Analyse.DoAlphabet(l => nextLetters[l.ToString()] = nextLetters[l.ToString()] / ((double)paragraphNumbers - 1));

            return nextLetters;
        }*/

        private static XmlElement GetPart(Dictionary<string, double> stats, string rootElementName, XmlDocument xmldoc)
        {
            XmlElement root = xmldoc.CreateElement(rootElementName);
            XmlElement info;

            foreach (var pair in stats)
            {
                info = xmldoc.CreateElement("info");
                info.SetAttribute("letters", pair.Key.ToString());
                info.InnerText = pair.Value.ToString();
                root.AppendChild(info);
            }

            return root;
        }
    }
}
