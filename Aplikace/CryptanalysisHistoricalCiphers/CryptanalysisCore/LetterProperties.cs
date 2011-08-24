using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtensionMethods;

namespace CryptanalysisCore
{
    class LetterProperties
    {
        /// <summary>
        /// Symetrické bigramy, které se nachází v nejčastějších bigramech
        /// </summary>
        public readonly string[] SymetricBigramsInTop;

        public readonly string[] BigramsWithLetter;
        public readonly string[] TrigramsWithLetter;

        public readonly int LettersBefore;
        public readonly int LettersAfter;

        public readonly char Letter;

        public int SimilarityIndex(LetterProperties letprop)
        {
            int index = 0;

            index += Math.Abs(SymetricBigramsInTop.Length - letprop.SymetricBigramsInTop.Length);
            /*index += Math.Abs(BigramsWithLetter.Length - letprop.BigramsWithLetter.Length);
            index += Math.Abs(TrigramsWithLetter.Length - letprop.TrigramsWithLetter.Length);*/

            for (int i = 0; i <= 1; i++)
                index += Math.Abs(BigramsWithLetter.Count(x => x[i] == Letter) - letprop.BigramsWithLetter.Count(x => x[i] == letprop.Letter));

            for (int i = 0; i <= 2; i++)
                index += Math.Abs(TrigramsWithLetter.Count(x => x[i] == Letter) - letprop.TrigramsWithLetter.Count(x => x[i] == letprop.Letter));

            return index;
        }

        public LetterProperties(char letter, Dictionary<int, string[]> ngrams, string ciphertext)
        {
            Letter = letter;
            SplitedNgrams splitedNgrams = new SplitedNgrams(ngrams);
            SymetricBigramsInTop = FindSymetricBigrams(letter, splitedNgrams.Top[2]);
            BigramsWithLetter = FindNgramsWithLetter(letter, splitedNgrams.Top[2]);
            TrigramsWithLetter = FindNgramsWithLetter(letter, splitedNgrams.Top[3]);
            LettersBefore = NumbersBeforeLetter(letter, ciphertext);
            LettersAfter = NumbersAfterLetter(letter, ciphertext);
        }

        private int NumbersBeforeLetter(char letter, string ciphertext)
        {
            List<char> foundedLetters = new List<char>();

            for (int i = 1; i < ciphertext.Length; i++)
            {
                if (ciphertext[i] == letter)
                    if (!foundedLetters.Find(ciphertext[i - 1]))
                        foundedLetters.Add(ciphertext[i - 1]);
            }

            return foundedLetters.Count;
        }

        private int NumbersAfterLetter(char letter, string ciphertext)
        {
            List<char> foundedLetters = new List<char>();

            for (int i = 0; i < ciphertext.Length - 1; i++)
            {
                if (ciphertext[i] == letter)
                    if (!foundedLetters.Find(ciphertext[i + 1]))
                        foundedLetters.Add(ciphertext[i + 1]);
            }

            return foundedLetters.Count;
        }

        /// <summary>
        /// Nalezne všechny ngramy, které obsahují dané písmeno
        /// </summary>
        /// <param name="letter">Písmeno, které se má vyskytovat v ngramu</param>
        /// <param name="ngrams">Pole ngramů</param>
        /// <returns>Pole ngramů, které obsahují znak letter</returns>
        private string[] FindNgramsWithLetter(char letter, string[] ngrams)
        {
            return ngrams.Where(x => x.Contains(letter)).ToArray();
        }

        /// <summary>
        /// Nalezne všechny symetrické ngramy. Tedy slova ve tvaru abc-cba
        /// </summary>
        /// <param name="letter">Písmeno, které musí být v ngramu obsaženo</param>
        /// <param name="ngrams">Seznam ngramů, ve kterých se bude vyhledávat</param>
        /// <returns>Symetrické ngramy</returns>
        private string[] FindSymetricBigrams(char letter, string[] ngrams)
        {
            List<string> symetricBigrams = new List<string>();

            foreach (var bigram in ngrams)
                if (bigram.Contains(letter) && ngrams.Find(bigram.Reverse()))
                    symetricBigrams.Add(bigram);

            return symetricBigrams.ToArray();
        }
    }
}
