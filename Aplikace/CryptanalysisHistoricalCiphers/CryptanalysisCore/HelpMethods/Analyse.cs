using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtensionMethods;

namespace CryptanalysisCore
{
    public static class Analyse
    {
        public enum TextTypes
        {
            WithSpacesLower,
            WithSpacesUpper,
            WithoutSpacesLower,
            WithoutSpacesUpper
        }

        public const int OneGram = 1;
        public const int Bigram = 2;
        public const int Trigram = 3;

        public const char Unknown = '?';

        /// <summary>
        /// Počet písmen v abecedě
        /// </summary>
        public const int AlphabetLetterCount = 26;


        /// <summary>
        /// Zjistí, jak moc daný text odpovídá zvyklostem daného jazyka.
        /// </summary>
        /// <param name="text">Jakýkoliv text daného jazyka</param>
        /// <param name="lang">Vlastnosti a charakteristiky jazyka</param>
        /// <returns>Procentuální vyjádření shody s vlastnostmi jazyka</returns>
        public static double SimilarityIndex(string text, LangCharacteristic lang)
        {
            double similarity = 0;

            similarity += 3 * SumSimilarity(text, lang.Letters);
            similarity += SumSimilarity(text, lang.Bigrams);
            similarity += SumSimilarity(text, lang.Trigrams);

            return similarity;
        }

        /// <summary>
        /// Vrátí součet rozdílů očekávaných a nalezených procentuálních výskytů znaků
        /// </summary>
        /// <param name="text"></param>
        /// <param name="occurrence"></param>
        /// <returns></returns>
        private static double SumSimilarity(string text, Dictionary<string, double> occurrence)
        {
            Dictionary<string, double> standardLettersOccurrence =
                occurrence.Take(6).ToDictionary(x => x.Key, x => x.Value);

            Dictionary<string, double> inTextLettersOcc = TextAnalysis.GetOccurrence(text, occurrence.Keys.Take(1).ToArray()[0].Length);

            double sum = 0;

            foreach (KeyValuePair<string, double> occ in standardLettersOccurrence)
            {
                if (inTextLettersOcc.ContainsKey(occ.Key))
                {
                    sum += Math.Abs(occ.Value - inTextLettersOcc[occ.Key]);
                }
                else
                {
                    sum += 10;
                }
            }

            return sum;
        }

        /// <summary>
        /// Vrací vzdálenost dvou písmen
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>Vzdálenost dvou písmen</returns>
        public static int Distance(char a, char b)
        {
            return Math.Abs((int)a - (int)b);
        }

        /// <summary>
        /// Vrací minimální vzdálenost, to jest minimální počet
        /// záporných nebo kladných posunů z písmene a do b.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>Minimální vzdálenost dvou písmen</returns>
        public static int MinDistance(char a, char b)
        {
            int diff = Math.Abs((int)a - (int)b);
            return diff < (double)AlphabetLetterCount / 2 ? diff : AlphabetLetterCount - diff;
        }


        /// <summary>
        /// Převede text do formy vhodné k šifrování a dešifrování
        /// Odstraní diakritiku, převede text na malá/velká písmena, 
        /// odstraní všechna nepísmena včetně/mimo mezer.
        /// </summary>
        /// <param name="text">Text, který chceme normalizovat</param>
        /// <param name="textType">Typ normalizace</param>
        /// <returns>Normalizovaný řetězec</returns>
        public static string NormalizeText(string text, TextTypes textType)
        {
            string normText = text.RemoveDiacritics().Replace(new string[] { ".", ",", "!", "?", "\n", "\r", ";", " " }, " ");

            switch (textType)
            {
                case TextTypes.WithoutSpacesLower:
                    return normText.Filter(c => TextAnalysis.IsEnglishLetter(c)).ToLower();
                case TextTypes.WithoutSpacesUpper:
                    return normText.Filter(c => TextAnalysis.IsEnglishLetter(c)).ToUpper();
                case TextTypes.WithSpacesLower:
                    return TextAnalysis.GetLetters(normText).ToLower();
                case TextTypes.WithSpacesUpper:
                    return TextAnalysis.GetLetters(normText).ToUpper();
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Předanou metodu spustí se všemi písmeny abecedy
        /// </summary>
        /// <param name="action">Metoda, která se bude aplikovat na písmena</param>
        public static void DoAlphabet(Action<char> action)
        {
            for (int i = 0; i < AlphabetLetterCount; i++)
            {
                action((char)(i + 'a'));
            }
        }

        /// <summary>
        /// Zamění písmena v otevřeném textu za jejich ekvivalenty
        /// </summary>
        /// <param name="opentext">Text, ve kterém budou provedeny záměny</param>
        /// <param name="lettersSubstitution">Definice záměn</param>
        /// <returns>Nový řetězec se změněnými znaky</returns>
        public static string SwitchLetters(string opentext, Dictionary<char, char> lettersSubstitution)
        {
            opentext = opentext.ToLower();
            char[] ciphertext = new char[opentext.Length];

            for (int i = 0; i < opentext.Length; i++)
                if (lettersSubstitution.ContainsKey(opentext[i]))
                    ciphertext[i] = lettersSubstitution[opentext[i]];
                else
                    ciphertext[i] = opentext[i];

            return new string(ciphertext);
        }

        /// <summary>
        /// Vrátí relativní (procentuální) výskyt znaků v řetězci.
        /// </summary>
        /// <param name="s">Libovolný řetězec</param>
        /// <param name="predicate">Predikát, kterým budou testovány jednotlivé znaky</param>
        /// <returns>Slovník ve tvaru znak - počet výskytů vzhledem k celkovému počtu znaků</returns>
        public static Dictionary<char, double> CharsRelativeOccurrence(String s, Predicate<char> predicate)
        {
            Dictionary<char, int> occurrences = CharsOccurrence(s, predicate);
            Dictionary<char, double> relativeOccurences = new Dictionary<char, double>();

            foreach (KeyValuePair<char, int> occurrence in occurrences)
            {
                relativeOccurences[occurrence.Key] = (double)occurrence.Value / (double)s.Length * 100;
            }

            DoAlphabet(letter =>
                {
                    if (!relativeOccurences.ContainsKey(letter))
                        relativeOccurences[letter] = 0;
                });

            relativeOccurences = relativeOccurences.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, y => y.Value);
            return relativeOccurences;
        }

        /// <summary>
        /// Vrátí relativní (procentuální) výskyt znaků v řetězci.
        /// </summary>
        /// <param name="s">Libovolný řetězec</param>
        /// <returns>Slovník ve tvaru znak - počet výskytů vzhledem k celkovému počtu znaků</returns>
        public static Dictionary<char, double> CharsRelativeOccurrence(String s)
        {
            return CharsRelativeOccurrence(s, x => true);
        }

        /// <summary>
        /// Zjistí počet jednotlivých znaků v řetězci
        /// </summary>
        /// <param name="s">Libovolný řetězec</param>
        /// <returns>Slovník ve tvaru znak - počet výskytů</returns>
        public static Dictionary<char, int> CharsOccurrence(String s)
        {
            return CharsOccurrence(s, (x) => true);
        }

        /// <summary>
        /// Zjistí počet jednotlivých znaků v řetězci, které odpovídají předanému predikátu
        /// </summary>
        /// <param name="s">Libovolný řetězec</param>
        /// <param name="predicate">Predikát, kterým budou testovány jednotlivé znaky</param>
        /// <returns>Slovník ve tvaru znak - počet výskytů</returns>
        public static Dictionary<char, int> CharsOccurrence(String s, Predicate<char> predicate)
        {
            Dictionary<char, int> occurrence = new Dictionary<char, int>();

            foreach (char character in s)
            {
                if (occurrence.ContainsKey(character))
                {
                    occurrence[character]++;
                }
                else
                {
                    if (predicate(character))
                    {
                        occurrence[character] = 1;
                    }
                }
            }

            return occurrence;
        }

        /// <summary>
        /// Posune předaný znak v abecedě podle parametru 'IndikátorPosunu'.
        /// Pokud bude například Znak = 'e' a IndikátorPosunu = 'c', bude písmeno
        /// 'e' posunuto o dvě pozice na 'g'.
        /// </summary>
        /// <param name="character">Znak, který má být posunut.</param>
        /// <param name="offset">Znak který určuje jak dlouhé má být posunutí.</param>
        /// <returns>Posunutý znak.</returns>
        public static char MoveCharacter(char character, char offset)
        {
            if (char.IsLower(character) && char.IsUpper(offset) ||
                char.IsUpper(character) && char.IsLower(offset))
                throw new ArgumentException("Písmena musí být buď obě velká nebo obě malá");

            // Sečteme ASCI hodnoty obou znaků
            int MovedCharacter = (int)character + (int)offset;
            MovedCharacter -= char.IsLower(character) ? (int)'a' : (int)'A';

            // Jestli jsme se dostali za konec abecedy, začneme počítat od začátku abecedy
            if ((char.IsLower(character) && MovedCharacter > (int)'z') || (char.IsUpper(character) && MovedCharacter > (int)'Z'))
                MovedCharacter -= 26;

            // Převedeme číslo zpět na char a vrátíme
            return (char)MovedCharacter;
        }

        public static char MoveBackCharacter(char character, char offset)
        {
            if (offset == Analyse.Unknown)
                return Analyse.Unknown;

            int MovedCharacter = (int)character - (int)offset;
            MovedCharacter += char.IsLower(character) ? (int)'a' : (int)'A';

            if ((char.IsLower(character) && MovedCharacter < (int)'a') || (char.IsUpper(character) && MovedCharacter < (int)'A'))
                MovedCharacter += 26;

            return (char)MovedCharacter;
        }

        /// <summary>
        /// Zjistí, zda záměny písmen definované v letters odpovídají 
        /// transformaci openWord->cipherWord
        /// </summary>
        /// <param name="openWord"></param>
        /// <param name="cipherWord"></param>
        /// <param name="letters"></param>
        /// <returns></returns>
        public static bool MatchLetters(string openWord, string cipherWord, Dictionary<char, char> letters)
        {
            for (int i = 0; i < openWord.Length; i++)
            {
                if (letters.ContainsKey(openWord[i]))
                {
                    if (letters[openWord[i]] != cipherWord[i])
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Zjistí, kolik slov se nachází v textu
        /// </summary>
        /// <param name="text"></param>
        /// <param name="words"></param>
        /// <returns></returns>
        public static int WordsContains(string text, string[] words)
        {
            int counter = 0;

            foreach (string word in words)
                if (text.Contains(word))
                    counter++;

            return counter;
        }
    }
}
