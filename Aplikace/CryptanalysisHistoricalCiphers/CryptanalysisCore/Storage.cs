using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CryptanalysisCore
{
    public static class Storage
    {
        public static string ConfPath;

        public const string FrequencyFile = "frequency.xml";

        public const string UniqueFile = "unique.txt";

        public const string DictionaryFile = "dictionary.dic";

        public const string TextsFile = "texts.txt";

        public const string TextsFolder = "texts/";

        public static string[] Letters
        { get; private set; }

        public static string[] Vowels
        { get; private set; }

        /// <summary>
        /// Seznam podporovaných jazyků
        /// </summary>
        public enum Languages
        {
            czech,
            english,
            germany,
            french
        }

        /// <summary>
        /// Seznam podporovaných šifer
        /// </summary>
        public enum Ciphers
        {
            caesar,
            monoalphabetic,
            vigenere,
            trans
        }

        /// <summary>
        /// Dvojice jazyk : charakteristiky daného jazyka
        /// </summary>
        private static Dictionary<Languages, LangCharacteristic> langChars;

        /// <summary>
        /// Uchovává seznamy crackovacích algoritmů pro jednotlivé šifry
        /// </summary>
        public static Dictionary<Ciphers, string[]> CrackAlgorithms
        {
            get;
            private set;
        }

        static Storage()
        {
            //if (Environment.MachineName == "LUKASHAVRLANT")
                ConfPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Cryptanalysis\\";
            //else
              //  ConfPath = "items\\";

            SetCiphers();
            SetLanguages();
            SetCrackAlgorithms();
            SetLetters();
        }

        private static void SetLetters()
        {
            Letters = LettersToArray("abcdefghijklmnopqrstuvwxyz");
            Vowels = LettersToArray("aeuioy");
        }

        private static string[] LettersToArray(string letters)
        {
            return letters.ToCharArray().Select(x => x.ToString()).ToArray();
        }

        /// <summary>
        /// Načte z disku požadovaná data
        /// </summary>
        public static void LoadFiles(Action actionIfNotSucces)
        {
            try
            {
                langChars = new Dictionary<Languages, LangCharacteristic>();

                var names = Enum.GetNames(typeof(Languages));

                foreach (string name in names)
                {
                    Languages l = (Languages)Enum.Parse(typeof(Languages), name);
                    langChars[l] = new LangCharacteristic(l);
                }
            }
            catch (Exception)
            {
                actionIfNotSucces();
            }
        }

        private static void SetCrackAlgorithms()
        {
            CrackAlgorithms = new Dictionary<Ciphers, string[]>();

            string[] caesarAlgs = new string[] { "Útok hrubou silou", "Trojúhelníkový útok" };
            string[] monoalphabeticAlgs = new string[] { "Frekvenční analýza" };
            string[] vigenereAlgs = { "Útok hrubou silou",  };
            string[] transAlgs = { "Útok odhadnutím délky klíče" };

            CrackAlgorithms[Ciphers.caesar] = caesarAlgs;
            CrackAlgorithms[Ciphers.monoalphabetic] = monoalphabeticAlgs;
            CrackAlgorithms[Ciphers.vigenere] = vigenereAlgs;
            CrackAlgorithms[Ciphers.trans] = transAlgs;
        }

        /// <summary>
        /// Naplnění tabulky šifrových jmen
        /// </summary>
        private static void SetCiphers()
        {
            CiphersID = new Dictionary<string, Ciphers>();
            CiphersID["Caesarova šifra"] = Ciphers.caesar;
            CiphersID["Monoalfabetická šifra"] = Ciphers.monoalphabetic;
            CiphersID["Vigenérova šifra"] = Ciphers.vigenere;
            CiphersID["Transpoziční šifra"] = Ciphers.trans;
        }

        /// <summary>
        /// Naplnění tabulky jazyků
        /// </summary>
        private static void SetLanguages()
        {
            LanguagesID = new Dictionary<string, Languages>();
            LanguagesID["čeština"] = Languages.czech;
            LanguagesID["angličtina"] = Languages.english;
            LanguagesID["němčina"] = Languages.germany;
            LanguagesID["francouzština"] = Languages.french;
        }

        /// <summary>
        /// Vrátí charakteristicky jazyka (tohle je ale blbost, to se přepíše)
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static LangCharacteristic GetLangChar(Languages language)
        {
            return langChars[language];
        }

        /// <summary>
        /// Slovník název šifry : enum ty šifry
        /// </summary>
        public static Dictionary<string, Ciphers> CiphersID;

        /// <summary>
        /// Vrátí seznam všech podporovaných šifer
        /// </summary>
        public static string[] CiphersNames
        {
            get
            {
                return CiphersID.Keys.ToArray();
            }
        }

        /// <summary>
        /// Slovník název jazyka : enum jazyka
        /// </summary>
        public static Dictionary<string, Languages> LanguagesID;

        /// <summary>
        /// Obsahuje seznam všech podporovaných jazyků
        /// </summary>
        public static string[] LanguagesNames
        {
            get
            {
                return LanguagesID.Keys.ToArray();
            }
        }
    }
}
