using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtensionMethods;

namespace CryptanalysisCore
{
    class DictionaryAttack
    {
        private string[] dictionary;
        private Dictionary<int, string[]> sortedDictionary;

        public DictionaryAttack(string[] dictionary, Dictionary<int, string[]> sortedDictionary)
        {
            this.dictionary = dictionary;
            this.sortedDictionary = sortedDictionary;
        }

        public void Start(string ciphertext, int keyLength)
        {
            string sentence = ciphertext.Take(20);
            char[] letters = sentence.ToCharArray();
            var filteredDic = dictionary.Where(x => IsWordComplete(letters, x)).ToArray();
            
        }

        private bool IsWordComplete(char[] possibleChars, string word)
        {
            char[] temp = (char[]) possibleChars.Clone();
            foreach (char letter in word)
            {
                int index = temp.IndexOf(letter);
                if (index == -1)
                    return false;
                else
                    temp[index] = '?';
            }

            return true;
        }
    }
}
