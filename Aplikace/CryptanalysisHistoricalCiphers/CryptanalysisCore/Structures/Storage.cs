using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CryptanalysisCore
{
    public static class Storage
    {
        public static string StatsFolderName = "LangStats/";

        public static string StatsFolderPath;

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
        /// Nápověda pro validní klíč pro danou šifru
        /// </summary>
        public static Dictionary<Storage.Ciphers, string> keyHelp
        { private set; get; }

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

        static Storage()
        {
            SetCiphers();
            SetLanguages();
            SetCrackAlgorithms();
            SetLetters();
            InitKeyHelp();
        }

        private static void InitKeyHelp()
        {
            keyHelp = new Dictionary<Storage.Ciphers, string>();
            keyHelp[Storage.Ciphers.caesar] = "Klíčem musí být jedno písmeno anglické abecedy";
            keyHelp[Storage.Ciphers.monoalphabetic] = "Klíčem musí být libovolná posloupnost anglických písmen o délce alespoň jedna (zbytek se dopočítá automaticky)";
            keyHelp[Storage.Ciphers.trans] = "Klíčem musí být libovolná posloupnost anglických písmen o délce alespoň dva";
            keyHelp[Storage.Ciphers.vigenere] = "Klíčem musí být libovolná posloupnost anglických písmen o délce alespoň dva";
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
        public static void LoadFiles(Action progressAction)
        {
            bool filesLoaded = true;


            StatsFolderPath = GetTargetFolder();
            //StatsFolderPath = StatsFolderName;
            langChars = new Dictionary<Languages, LangCharacteristic>();
            var names = Enum.GetNames(typeof(Languages));

            foreach (string name in names)
            {
                try
                {
                    progressAction();
                    Languages l = (Languages)Enum.Parse(typeof(Languages), name);
                    langChars[l] = new LangCharacteristic(StatsFolderPath + l.ToString() + "/");
                }
                catch (Exception)
                {
                    filesLoaded = false;
                }
            }

            if (!filesLoaded)
            {
                throw new FileNotFoundException();
            }
        }

        public static void LoadFiles()
        {
            LoadFiles(ExtensionMethods.CommonMethods.NothingAction);
        }

        private static string GetTargetFolder()
        {
            string checkFile = "czech/dictionary.dic";
            string path = StatsFolderName;

            for (int i = 0; i < 10; i++)
            {
                try
                {
                    var stream = File.Open(path + checkFile, FileMode.Open);
                    stream.Close();
                    return path;
                }
                catch (Exception)
                {
                    path = "../" + path;
                    continue;
                }
            }

            throw new FileNotFoundException();
        }

        private static void SetCrackAlgorithms()
        {
            CrackAlgorithms = new Dictionary<Ciphers, string[]>();

            string[] caesarAlgs = new string[] { "Útok hrubou silou", "Trojúhelníkový útok" };
            string[] monoalphabeticAlgs = new string[] { "Nalezení unikátních slov" };
            string[] vigenereAlgs = { "Trojúhelníkový útok", "Útok odhadnutím délky klíče" };
            string[] transAlgs = { "Nalezení slov v řádku" };

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
        /// 
        /// </summary>
        /// <param name="cipher"></param>
        /// <returns></returns>
        public static string GetCipherName(Ciphers cipher)
        {
            return CiphersID.ToDictionary(x => x.Value, x => x.Key)[cipher];
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

        public static Ciphers GetCiphersType(Cipher cipher)
        {
            return Ciphers.caesar;
        }
    }
}
