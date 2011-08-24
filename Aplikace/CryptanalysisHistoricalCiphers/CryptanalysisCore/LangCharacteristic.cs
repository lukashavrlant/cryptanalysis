using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using ExtensionMethods;

namespace CryptanalysisCore
{
    public class LangCharacteristic
    {
        public LangCharacteristic(Storage.Languages language)
        {
            string targetFolder = Storage.ConfPath + language.ToString() + "/";
            UniqueWords = File.ReadAllText(targetFolder + Storage.UniqueFile).Split(' ');
            Dictionary = File.ReadAllText(targetFolder + Storage.DictionaryFile).Split(' ');
            SortedDictionary = ExtensionMethods.Dictionary.Create(Dictionary, x => x.Length);

            XmlDocument lang = new XmlDocument();
            lang.Load(targetFolder + Storage.FrequencyFile);
            Letters = GetOccurrance(lang.SelectNodes("//letters/info"));
            Bigrams = GetOccurrance(lang.SelectNodes("//bigrams/info"));
            Trigrams = GetOccurrance(lang.SelectNodes("//trigrams/info"));
            StartLetters = GetOccurrance(lang.SelectNodes("//startLetters/info"));
            EndLetters = GetOccurrance(lang.SelectNodes("//endLetters/info"));
            NextLetters = GetOccurrance(lang.SelectNodes("//nextLetters/info"));
            PrevLetters = GetOccurrance(lang.SelectNodes("//prevLetters/info"));
        }

        private Dictionary<string, double> GetOccurrance(XmlNodeList occurrances)
        {
            Dictionary<string, double> occurrence = new Dictionary<string, double>();

            foreach(XmlNode occ in occurrances)
                occurrence[occ.Attributes["letters"].InnerText] = double.Parse(occ.InnerText);

            return occurrence;
        }

        /// <summary>
        /// Obsahuje relativní počet výskytů písmene v textu
        /// </summary>
        public Dictionary<string, double> Letters
        { get; private set; }

        /// <summary>
        /// Obsahuje relativní počet výskytů bigramů v textu
        /// </summary>
        public Dictionary<string, double> Bigrams
        { get; private set; }

        /// <summary>
        /// Obsahuje relativní počet výskytů trigramů v textu
        /// </summary>
        public Dictionary<string, double> Trigrams
        { get; private set; }

        /// <summary>
        /// Obsahuje relativní počet výskytů písmen na začátku slova
        /// </summary>
        public Dictionary<string, double> StartLetters
        { get; private set; }

        /// <summary>
        /// Obsahuje relativní počet výskytů písmen na konci slova
        /// </summary>
        public Dictionary<string, double> EndLetters
        { get; private set; }

        /// <summary>
        /// Obsahuje průměrný počet výskytů daného písmene za nějakým jiným písmenem
        /// </summary>
        public Dictionary<string, double> NextLetters
        { get; private set; }

        /// <summary>
        /// Obsahuje průměrný počet výskytů daného písmene před nějakým jiným písmenem
        /// </summary>
        public Dictionary<string, double> PrevLetters
        { get; private set; }

        /// <summary>
        /// Obsahuje unikátní slova
        /// </summary>
        public string[] UniqueWords
        { get; private set; }

        /// <summary>
        /// Obsahuje nesetříděný seznam všech slov
        /// </summary>
        public string[] Dictionary
        { get; private set; }

        /// <summary>
        /// Obsahuje setříděný seznam všech slov podle délky slova
        /// </summary>
        public Dictionary<int, string[]> SortedDictionary
        { get; private set; }
    }
}
