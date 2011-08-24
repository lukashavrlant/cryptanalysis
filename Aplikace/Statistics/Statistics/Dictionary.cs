using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using CryptanalysisCore;
using ExtensionMethods;

namespace Statistics
{
    class LangDictionary
    {
        public void Save(string language, string oldDic)
        {
            string oldDicPath = Storage.ConfPath + Storage.TextsFolder + oldDic;
            string newDicPath = Storage.ConfPath + language + "/" + "dictionary.dic";

            string oldDicText = Analyse.NormalizeText(File.ReadAllText(oldDicPath), Analyse.TextTypes.WithSpacesLower);
            string[] oldDicWords = TextAnalysis.ToWords(oldDicText);
            string orderedWords = oldDicWords.OrderBy(x => x).Implode(" ");
            File.WriteAllText(newDicPath, orderedWords);
        }
    }
}
