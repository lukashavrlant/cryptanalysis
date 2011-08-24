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
        public const int MaxKeyLength = 20;
        public const int MinKeyLength = 2;

        public const int TestKeyLengthAttack = 0;

        public override string Encrypt(string opentext, string key)
        {
            base.Encrypt(opentext, key);
            string[] columns = GetEncryptColumns(opentext, key);
            var orderedColumns = ColumnEncryptOrder(columns, key);
            var result = orderedColumns.Values.Implode();
            return result;
        }

        public override string Decrypt(string ciphertext, string key)
        {
            base.Decrypt(ciphertext, key);
            var columns = GetDecryptColumns(ciphertext, key);
            var orderedColumns = ColumnDecryptOrder(columns, key);
            var result = GetOpentext(orderedColumns);
            return result;
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

        protected override bool IsKeyValid(string key)
        {
            return key.Length >= MinKeyLength && Regex.IsMatch(key, @"^[a-z\?]+$");
        }

        protected override void SetCrackMethods()
        {
            CrackMethods = new CrackMethod[] { TestKeyLength };
        }

        /*********** test key length attack *******************/

        private List<string> TestKeyLength(string ciphertext, Storage.Languages language)
        {
            int averageKeyLength = 10;
            int minSentenceLength = 40;
            var keyLengths = Maths.AllDivisors(ciphertext.Length);
            var orderKeyLengths = keyLengths.OrderBy(x => Math.Abs(x - averageKeyLength)).ToArray();

            foreach (int keyLength in orderKeyLengths)
            {
                string[] columns = GetDecryptColumns(ciphertext, keyLength);
                string sentence = GetOpentext(columns).Take(minSentenceLength);
                var lang = Storage.GetLangChar(language);
                DictionaryAttack attack = new DictionaryAttack(lang.Dictionary, lang.SortedDictionary);
                attack.Start(sentence, keyLength);
            }

            return new List<string>() { "abc" };
        }

        private string[] GetColumns(string ciphertext, int keyLength)
        {
            return null;
        }


        /********************** konec ************************/

        public override string RandomKey()
        {
            int length = random.Next(2, MaxKeyLength);
            return Text.RandomString(length);
        }
    }
}
