using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CryptanalysisCore;
using ExtensionMethods;

namespace Statistics
{
    public static class UniqueWords
    {
        /// <summary>
        /// Najde všechny unikátní slova a uloží je
        /// </summary>
        public static void Save(string language)
        {
            string targetFolder = Storage.ConfPath + language + "/";
            string fileSavePath = targetFolder + "unique.txt";
            string fileTextsPath = targetFolder + "texts.txt";

            string text = File.ReadAllText(fileTextsPath);
            string[] texts = TextAnalysis.ToWords(text);
            CryptanalysisCore.UniqueWords uniqueWords = new CryptanalysisCore.UniqueWords(texts);
            var uniqueWordsList = uniqueWords.GetUniqueWords();

            StringBuilder sb = new StringBuilder();
            foreach (var words in uniqueWordsList)
            {
                foreach (var word in words)
                {
                    sb.AppendFormat("{0} ", word);
                }
            }
            File.WriteAllText(fileSavePath, sb.ToString());
        }

        
    }
}
