using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtensionMethods;
using System.Text.RegularExpressions;

namespace CryptanalysisCore
{
    public class Transposition : Cipher
    {
        private Random random = new Random();
        public const int MaxKeyLength = 12;
        public const int MinKeyLength = 4;
        public const int topWordsCount = 200;

        public const int TestKeyLengthAttack = 0;

        public Transposition()
            : base()
        {
            //ExceptionText = "Klíčem musí být všechny malá písmena anglické abecedy, libovolně rozpřeházená.";
            CipherType = Storage.Ciphers.trans;
        }

        public override string Encrypt(string opentext, string key)
        {
            base.Encrypt(opentext, key);

            opentext = Analyse.NormalizeText(opentext, Analyse.TextTypes.WithoutSpacesLower);

            string[] columns = GetEncryptColumns(opentext, key);
            var orderedColumns = ColumnEncryptOrder(columns, key);
            var result = orderedColumns.Values.Implode();
            return result.ToUpper();
        }

        public override string Decrypt(string ciphertext, string key)
        {
            base.Decrypt(ciphertext, key);

            ciphertext = Analyse.NormalizeText(ciphertext, Analyse.TextTypes.WithoutSpacesLower);

            if (!IsKeylengthValid(ciphertext, key))
                throw new Exceptions.InvalidCipherKey("Délka klíče nesouhlasí s délkou šifrového textu");

            var columns = GetDecryptColumns(ciphertext, key);
            var orderedColumns = ColumnDecryptOrder(columns, key);
            var result = GetOpentext(orderedColumns);
            return result.ToLower();
        }

        private bool IsKeylengthValid(string ciphertext, string key)
        {
            return ciphertext.Length % key.Length == 0;
        }

        /// <summary>
        /// Ze seřazených sloupců vrátí otevřený text
        /// </summary>
        /// <param name="orderedColumns"></param>
        /// <returns></returns>
        private string GetOpentext(string[] orderedColumns)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < orderedColumns[0].Length; i++)
            {
                for (int j = 0; j < orderedColumns.Length; j++)
                {
                    sb.Append(orderedColumns[j][i].ToString());
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Vrátí sloupečky v zašifrovaném textu
        /// </summary>
        /// <param name="ciphertext"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private string[] GetDecryptColumns(string ciphertext, string key)
        {
            return GetDecryptColumns(ciphertext, key.Length);
        }

        private string[] GetDecryptColumns(string ciphertext, int keyLength)
        {
            int columnsLength = (int)((double)ciphertext.Length / (double)keyLength);
            string[] columns = new string[keyLength];

            for (int i = 0; i < keyLength; i++)
            {
                columns[i] = ciphertext.Substring(i * columnsLength, columnsLength);
            }

            return columns;
        }

        /// <summary>
        /// Vrátí pořadí sloupců při dešifrování
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private string[] ColumnDecryptOrder(string[] columns, string key)
        {
            Dictionary<string, int> indexesColumns = new Dictionary<string, int>();

            for (int i = 0; i < key.Length; i++)
            {
                indexesColumns[GetColumnKey(indexesColumns.Keys.ToArray(), key[i])] = i;
            }

            indexesColumns = indexesColumns.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            string[] orderColumns = new string[key.Length];
            int counter = 0;

            foreach (var pair in indexesColumns)
            {
                orderColumns[pair.Value] = columns[counter++];
            }

            return orderColumns;
        }

        /// <summary>
        /// Vrátí pořadí sloupců při šifrování
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private Dictionary<string, string> ColumnEncryptOrder(string[] columns, string key)
        {
            Dictionary<string, string> indexesColumns = new Dictionary<string, string>();

            for (int i = 0; i < key.Length; i++)
            {
                indexesColumns[GetColumnKey(indexesColumns.Keys.ToArray(), key[i])] = columns[i];
            }

            var result = indexesColumns.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            return result;
        }

        /// <summary>
        /// Najde první možný validní klíč sloepce. Pokud existuje "x" a "xx", vrátí "xxx" apod.
        /// </summary>
        /// <param name="indexesColumns"></param>
        /// <param name="currKey"></param>
        /// <returns></returns>
        private string GetColumnKey(string[] indexesColumns, char currKey)
        {
            string modifKey = currKey.ToString();

            while (indexesColumns.Contains(modifKey))
                modifKey += currKey.ToString();

            return modifKey;
        }

        /// <summary>
        /// Vrátí sloupečky v textu podle délky klíče při šifrování
        /// </summary>
        /// <param name="opentext"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private string[] GetEncryptColumns(string opentext, string key)
        {
            return GetEncryptColumns(opentext, key.Length);
        }

        private string[] GetEncryptColumns(string opentext, int keyLength)
        {
            int columnLength = (int)Math.Ceiling((double)opentext.Length / (double)keyLength);
            string[] columns = new string[keyLength];

            for (int i = 0; i < keyLength; i++)
            {
                StringBuilder sb = new StringBuilder();

                for (int j = 0; j < columnLength; j++)
                {
                    int index = j * keyLength + i;
                    if (index < opentext.Length)
                        sb.Append(opentext[index]);
                    else
                        sb.Append(Text.RandomString(1));
                }

                columns[i] = sb.ToString();
            }

            return columns;
        }

        public override bool IsKeyValid(string key)
        {
            return key.Length >= 2 && Regex.IsMatch(key, @"^[a-zA-Z\?]+$");
        }

        protected override void SetCrackMethods()
        {
            CrackMethods = new CrackMethod[] { BigWordAttack };
        }

        /*********************************************/
        /*********** One big word attack *************/
        /*********************************************/

        private List<string> BigWordAttack(string ciphertext, Storage.Languages language)
        {
            ciphertext = Analyse.NormalizeText(ciphertext, Analyse.TextTypes.WithoutSpacesLower);
            var keyLengths = GetKeyLengths(ciphertext);
            LangCharacteristic lang = Storage.GetLangChar(language);
            List<string> possKeys = new List<string>();

            foreach (int keyLength in keyLengths)
            {

                string[] columns = GetDecryptColumns(ciphertext, keyLength);
                string[] rows = GetRows(columns);

                DictionaryAttack attack = new DictionaryAttack();
                string[] keys = attack.GetKeys(rows, lang.TopWords);
                string[] orderedKeys = OrderKeys(keys, ciphertext, lang);
                if (orderedKeys.Length > 0)
                    possKeys.Add(orderedKeys[0]);
            }

            if (possKeys.Count > 0)
            {
                string[] finalKeys = OrderKeys(possKeys.ToArray(), ciphertext, lang);
                return finalKeys.ToList();
            }

            throw new Exceptions.MatchNotFound();
        }

        /// <summary>
        /// Seřadí klíče od nejpravděpodobnějších po nejméně pravděpodobné
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="ciphertext"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        private string[] OrderKeys(string[] keys, string ciphertext, LangCharacteristic lang)
        {
            var opentexts = GetOpentexts(keys, ciphertext);
            var probabilities = GetProbabilities(opentexts, keys, lang);
            var orderedKeys = probabilities.Keys.ToArray();
            return orderedKeys;
        }

        /// <summary>
        /// Zjistí pravděpodobnost jednotlivých klíčů
        /// </summary>
        /// <param name="opentexts"></param>
        /// <param name="keys"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        private Dictionary<string, int> GetProbabilities(string[] opentexts, string[] keys, LangCharacteristic lang)
        {
            string[] topTopWords = lang.TopWords.Take(topWordsCount).ToArray();

            Dictionary<string, int> probabilities = new Dictionary<string, int>();

            for (int i = 0; i < keys.Length; i++)
                probabilities[keys[i]] = Analyse.WordsContains(opentexts[i], topTopWords);

            return probabilities.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        }

        /// <summary>
        /// Vrátí všechny dešifrované texty
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="ciphertext"></param>
        /// <returns></returns>
        private string[] GetOpentexts(string[] keys, string ciphertext)
        {
            var opentexts = new string[keys.Length];

            for (int i = 0; i < keys.Length; i++)
                opentexts[i] = Decrypt(ciphertext, keys[i]);

            return opentexts;
        }

        /*********** First Letters Attack ************/

        // TODO: ostatní jazyky nemají TopWords!!!

        private List<string> FirstLettersAttack(string ciphertext, Storage.Languages language)
        {
            var keyLengths = GetKeyLengths(ciphertext);
            int minWordLength = 3;
            int testStringLength = 20;
            double minSuccess = (double)20 / (double)1300;

            foreach (int keyLength in keyLengths)
            {
                if (keyLength < 10)
                    continue;

                string[] columns = GetDecryptColumns(ciphertext, keyLength);
                string[] rows = GetRows(columns);
                string[] sentence = rows.Take((int)Math.Ceiling((double)testStringLength / (double)keyLength)).ToArray();
                var lang = Storage.GetLangChar(language);
                DictionaryAttack firstAttack = new DictionaryAttack();
                string[] topWords = lang.TopWords.Where(x => x.Length >= minWordLength).ToArray();
                string[] topTopWords = topWords.Take(100).ToArray();
                var keys = firstAttack.CrossFilter(sentence, topWords);
                var opentexts = new string[keys.Length];

                for (int i = 0; i < keys.Length; i++)
                    opentexts[i] = Decrypt(ciphertext, keys[i]);

                Dictionary<string, int> probabilities = new Dictionary<string, int>();

                for (int i = 0; i < keys.Length; i++)
                    probabilities[keys[i]] = Analyse.WordsContains(opentexts[i], topTopWords);

                var resultKeys = probabilities.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                var isthere = opentexts.Where(x => x.Contains("oznamenipodala")).Count() == 1;

                if(resultKeys.Count > 0 && ((double)resultKeys.First().Value / (double)1300) > minSuccess)
                    return resultKeys.Take(10).Select(x => x.Key).ToList();
            }

            throw new Exceptions.MatchNotFound();
        }

        private string[] GetRows(string[] columns)
        {
            string[] rows = new string[columns[0].Length];

            for (int i = 0; i < columns[0].Length; i++)
            {
                StringBuilder sb = new StringBuilder();
                for (int j = 0; j < columns.Length; j++)
                {
                    sb.Append(columns[j][i].ToString());
                }
                rows[i] = sb.ToString();
            }

            return rows;
        }

        private int[] GetKeyLengths(string ciphertext)
        {
            int averageKeyLength = ((MinKeyLength + MaxKeyLength) / 2);
            var keyLengths = Maths.AllDivisors(ciphertext.Length).Where(x => x >= MinKeyLength && x <= MaxKeyLength).ToArray();
            var orderedKeyLengths = keyLengths.OrderBy(x => Math.Abs(x - averageKeyLength)).ToArray();

            return orderedKeyLengths;
        }


        /*********** Brute force attack **************/
        private List<string> BruteForceAttack(string ciphertext, Storage.Languages language)
        {
            int keyLength = 7;
            string[] columns = GetDecryptColumns(ciphertext, keyLength);
            string[] rows = GetRows(columns);
            string firstLine = rows[0];

            var variations = Combinatorics.Permutation(firstLine.ToCharArray().ToList());

            int res = variations.Count(x => x.Implode() == "namstar");

            List<string> canonicalKey = Storage.Letters.Take(keyLength).ToList();
            var permKeys = Combinatorics.Permutation(canonicalKey);
            Dictionary<string, string> possTexts = new Dictionary<string, string>();

            foreach (var key in permKeys)
            {
                string k = key.Implode();
                possTexts[k] = Decrypt(ciphertext, k);
            }

            var langChar = Storage.GetLangChar(language);
            var result = possTexts.OrderBy(x => Analyse.SimilarityIndex(x.Value, langChar)).Take(10).ToDictionary(x => x.Key, x => x.Value);


            throw new Exceptions.MatchNotFound();
        }


        /********************** konec ************************/

        public override string RandomKey()
        {
            int length = random.Next(MinKeyLength, MaxKeyLength);
            return Text.RandomString(length);
        }

        public override string ToString()
        {
            return Storage.Ciphers.trans.ToString();
        }
    }
}
