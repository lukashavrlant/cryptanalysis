using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtensionMethods;

namespace CryptanalysisCore
{
    public class UniqueWords
    {
        /// <summary>
        /// Seznam všech unikátních slov ze slovníku
        /// roztříděných do tříd ekvivalence.
        /// </summary>
        private Dictionary<int, List<string>> uniqueWords;

        /// <summary>
        /// Unikátní slova nalezená v předaném textu. Viz metoda Find.
        /// </summary>
        private Dictionary<string, string> uniquePairs;

        /// <summary>
        /// Vyfiltrované páry
        /// </summary>
        private Dictionary<string, string> bestPairs;

        public UniqueWords(string[] uniqueWords)
        {
            this.uniqueWords = EquivalenceClass(uniqueWords);
            uniquePairs = null;
        }

        /// <summary>
        /// Vrátí všechna slova z words, která jsou unikátní
        /// (Tj. která mají vzor v uniqueWords)
        /// </summary>
        /// <param name="words"></param>
        /// <returns></returns>
        public Dictionary<string, string> Find(string[] words)
        {
            int index;
            Dictionary<string, string> unique = new Dictionary<string, string>();

            foreach (string word in words)
            {
                index = GetEqIndex(word);
                if (uniqueWords.ContainsKey(index))
                {
                    string uniqueWord = GetPattern(word, uniqueWords[index]);
                    if (uniqueWord != null)
                    {
                        unique[word] = uniqueWord;
                    }
                }
            }

            uniquePairs = unique;
            return uniquePairs;
        }

        /// <summary>
        /// Vrátí nejpravděpodobnější substituce písmen založené
        /// na unikátních slovech.
        /// </summary>
        /// <param name="words"></param>
        /// <returns></returns>
        public Dictionary<char, char> GetLettersSubst(string[] words)
        {
            Find(words);
            var bestPairs = Filter();
            return bestPairs != null ? TextAnalysis.GetLettersSubst(bestPairs) : null;
        }

        /// <summary>
        /// Projde nalezené unikátní dvojice a pokusí se z 
        /// nich vyfiltrovat ty, které tam nesedí. 
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> Filter()
        {
            WordsFilter uniqueFilter = new WordsFilter(uniquePairs);
            bestPairs = uniqueFilter.Filter(ArePairsMatch);
            return uniqueFilter.Checked ? bestPairs : null;
        }

        /// <summary>
        /// Odpovídají si předané dvojice unikátních slov?
        /// </summary>
        /// <param name="pair1"></param>
        /// <param name="pair2"></param>
        /// <returns></returns>
        private bool ArePairsMatch(KeyValuePair<string, string> pair1, KeyValuePair<string, string> pair2)
        {
            var subst1 = TextAnalysis.GetLettersSubst(pair1);
            var subst2 = TextAnalysis.GetLettersSubst(pair2);
            return TextAnalysis.AreSubstMatch(subst1, subst2);
        }

        /// <summary>
        /// Vrátí to slovo ze seznamu, které má stejný vzor jako předané slovo.
        /// </summary>
        /// <param name="word"></param>
        /// <param name="uniqueWords"></param>
        /// <returns></returns>
        private string GetPattern(string word, List<string> uniqueWords)
        {
            foreach (string uniqueWord in uniqueWords)
                if (SamePattern(word, uniqueWord))
                    return uniqueWord;

            return null;
        }

        /// <summary>
        /// Zjistí, zda předaná slova mají stejný vzor.
        /// </summary>
        /// <param name="word1"></param>
        /// <param name="word2"></param>
        /// <returns>true v případě, že slova mají stejný vzor, false jinak</returns>
        public static bool SamePattern(string word1, string word2)
        {
            if (word1.Length != word2.Length)
                return false;

            var checkedChars = new List<char>();
            var sameChars = new List<List<int>>();
            List<int> temp;
            char currentChar;

            // Nalezneme stejná písmena v prvním slově
            for (int i = 0; i < word1.Length; i++)
            {
                currentChar = word1[i];
                if (checkedChars.Find(currentChar))
                    continue;

                checkedChars.Add(currentChar);
                temp = new List<int>();
                temp.Add(i);

                for (int j = i + 1; j < word1.Length; j++)
                    if (word1[j] == currentChar)
                        temp.Add(j);

                sameChars.Add(temp);
            }

            // Zjistíme, zda druhé slovo odpovídá vzoru 
            List<char> findedChars = new List<char>();
            char findedChar;
            foreach (var same in sameChars)
            {
                findedChar = word2[same[0]];
                if (findedChars.Find(findedChar))
                    return false;

                findedChars.Add(findedChar);
                if (same.Count > 1)
                    foreach (int index in same)
                    {
                        if (word2[index] != word2[same[0]])
                            return false;
                    }
            }

            return true;
        }

        /// <summary>
        /// Zjistí, zda je dané slovo unikátní vzhledem k předaným slovům
        /// </summary>
        /// <param name="testWord"></param>
        /// <param name="words"></param>
        /// <returns></returns>
        private bool IsWordUnique(string testWord, string[] words)
        {
            foreach (string word in words)
            {
                if (SamePattern(word, testWord) && word != testWord)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Od začátku slova počítá písmena, dokud nenarazí na jedno písmeno dvakrát
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        private int CountDiffFirstLetters(string word)
        {
            List<char> finded = new List<char>();

            for (int i = 0; i < word.Length; i++)
                if (finded.Find(word[i]))
                    return i;
                else
                    finded.Add(word[i]);

            return word.Length;
        }

        private int CountDiffZiggzaggLetters(string word)
        {
            List<char> finded = new List<char>();

            for (int i = 0; i < word.Length; i += 2)
                if (finded.Find(word[i]))
                    return finded.Count();
                else
                    finded.Add(word[i]);

            return finded.Count();
        }


        private int CountDiffLastLetters(string word)
        {
            return CountDiffFirstLetters(word.Reverse());
        }

        /// <summary>
        /// Roztřídí předaná slova do tříd ekvivalence,
        /// aby se nemuselo každé slovo prohledávat s každým jiným.
        /// </summary>
        /// <param name="words"></param>
        /// <returns></returns>
        public Dictionary<int, List<string>> EquivalenceClass(string[] words)
        {
            Dictionary<int, List<string>> eqTexts = new Dictionary<int, List<string>>();
            for (int i = 0; i < 100; i++)
                eqTexts[i] = new List<string>();

            foreach (string text in words)
                eqTexts[text.Length].Add(text);

            var wordLengthEq = eqTexts.Where(x => x.Key <= 30 && x.Key > 7 && x.Value.Count > 0).Select(x => x.Value.Distinct().ToArray()).ToArray();
            var difffirstletter = DifferentPosLetters(wordLengthEq);

            return difffirstletter;
        }

        /// <summary>
        /// Rozdělí slova do tříd ekvivalence na základě počtu různých písmen
        /// od začátku, od konce a ob dva.
        /// </summary>
        /// <param name="texts"></param>
        /// <returns></returns>
        private Dictionary<int, List<string>> DifferentPosLetters(string[][] texts)
        {
            Dictionary<int, List<string>> eqTexts = new Dictionary<int, List<string>>();

            int index;

            foreach (string[] text in texts)
            {
                foreach (string word in text)
                {
                    index = GetEqIndex(word);
                    if (eqTexts.ContainsKey(index))
                        eqTexts[index].Add(word);
                    else
                        eqTexts[index] = new List<string>() { word };
                }
            }

            return eqTexts;
        }

        /// <summary>
        /// Spočítá index pro použití v třídách ekvivalence
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        private int GetEqIndex(string word)
        {
            int firstDiff, lastDiff, ziggzaggDiff, index;
            firstDiff = CountDiffFirstLetters(word);
            lastDiff = CountDiffLastLetters(word);
            ziggzaggDiff = CountDiffZiggzaggLetters(word);
            index = firstDiff + 30 * lastDiff + 900 * ziggzaggDiff + 900 * 30 * word.Length;
            return index;
        }

        /// <summary>
        /// Vrátí seznam unikátních slov, použití ve statistice
        /// </summary>
        /// <param name="texts"></param>
        /// <returns></returns>
        public List<string[]> GetUniqueWords()
        {
            // upgrade test
            //var textsArr = texts.Where(x => x.Value.Count < 3000).Select(x => x.Value.ToArray()).ToArray();
            var textsArr = uniqueWords.Where(x => x.Value.Count < 3000).Select(x => x.Value.ToArray()).ToArray();
            List<string[]> uniqueWords2 = new List<string[]>();
            int counter = 1;

            foreach (string[] words in textsArr)
            {
                string[] unique = GetUniqueWordsFromArray(words);
                if (unique.Length > 0)
                    uniqueWords2.Add(unique);

                counter++;
            }

            return uniqueWords2;
        }

        /// <summary>
        /// Vrátí seznam unikátních slov v daném poli
        /// </summary>
        /// <param name="words"></param>
        /// <returns></returns>
        private string[] GetUniqueWordsFromArray(string[] words)
        {
            List<string> uniqueWords = new List<string>();

            foreach (string word in words)
            {
                if (IsWordUnique(word, words))
                    uniqueWords.Add(word);
            }

            return uniqueWords.ToArray();
        }
    }
}
