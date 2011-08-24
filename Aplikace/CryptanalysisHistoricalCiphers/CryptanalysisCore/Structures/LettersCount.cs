using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryptanalysisCore.Structures
{
    class LettersCount
    {
        private Dictionary<char, int> lettersCount;

        public int this[char key]
        {
            get { return lettersCount[key]; }
        }

        public LettersCount(string word)
        {
            SetLettersCount(word);
        }

        /// <summary>
        /// Zjistí, zda slovo obsahuje dané písmen
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsLetter(char key)
        {
            return lettersCount.ContainsKey(key);
        }


        /// <summary>
        /// Rozhoduje, zda je dané slovo složeno z předaných písmen
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public bool MatchLetters(LettersCount testWord)
        {
            foreach (var pair in lettersCount)
            {
                if (!(testWord.ContainsLetter(pair.Key) && testWord[pair.Key] == pair.Value))
                    return false;
            }

            return true;
        }

        /*public bool MatchLetters(LettersCount word)
        {
            foreach (var pair in lettersCount)
            {
                if (!(word.ContainsLetter(pair.Key) && pair.Value <= lettersCount[pair.Key]))
                    return false;
            }

            return true;
        }*/


        /// <summary>
        /// Zjistí počty písmen ve slově a nastaví do slovníku
        /// </summary>
        /// <param name="text"></param>
        private void SetLettersCount(string text)
        {
            lettersCount = new Dictionary<char, int>();

            foreach (char letter in text)
            {
                if (lettersCount.ContainsKey(letter))
                    lettersCount[letter]++;
                else
                    lettersCount[letter] = 1;
            }
        }
    }
}
