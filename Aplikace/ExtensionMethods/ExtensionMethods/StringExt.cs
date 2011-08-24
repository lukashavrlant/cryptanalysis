using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtensionMethods
{
    public static class StringExt
    {
        /// <summary>
        /// Zjistí počet znaků v řetězci
        /// </summary>
        /// <param name="String"></param>
        /// <param name="Characters">Znaky, které budou v řetězci vyhledány.</param>
        /// <returns>Počet znaků</returns>
        public static int Count(this String s, params Char[] characters)
        {
            int Counter = 0;

            foreach (char Character in s)
            {
                if (Array.IndexOf(characters, Character) != -1)
                    Counter++;
            }

            return Counter;
        }

        /// <summary>
        /// Metoda umožňuje zjistit počet znaků v řetězci, které odpovídají předanému predikátu
        /// </summary>
        /// <param name="String"></param>
        /// <param name="Predicate"></param>
        /// <returns></returns>
        public static int Count(this String s, Predicate<char> predicate)
        {
            int count = 0;

            foreach (char Character in s)
                if (predicate(Character))
                    count++;

            return count;
        }


        /// <summary>
        /// Odstraní z řetězce diakritiku. Ostatní znaky jsou nezměněny.
        /// </summary>
        /// <param name="String"></param>
        /// <returns>Text bez diakritiky</returns>
        public static string RemoveDiacritics(this String s)
        {
            // oddělení znaků od modifikátorů (háčků, čárek, atd.)  
            s = s.Normalize(System.Text.NormalizationForm.FormD);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            for (int i = 0; i < s.Length; i++)
            {
                // do řetězce přidá všechny znaky kromě modifikátorů  
                if (System.Globalization.CharUnicodeInfo.GetUnicodeCategory(s[i]) != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(s[i]);
                }
            }

            // vrátí řetězec bez diakritiky  
            return sb.ToString();
        }

        /// <summary>
        /// Získá z řetězce jen ta písmena, která odpovídají předanému predikátu
        /// </summary>
        /// <param name="s">Libovolný řetězec</param>
        /// <param name="predicate">Predikát, kterým bude testováno každé písmeno</param>
        /// <returns>Řetězec písmen, které odpovídají danému predikátu</returns>
        public static string Filter(this String s, Predicate<Char> predicate)
        {
            StringBuilder sb = new StringBuilder();

            foreach (char character in s)
            {
                if (predicate(character))
                    sb.Append(character.ToString());
            }

            return sb.ToString();
        }

        /// <summary>
        /// Obrátí řetězec
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Reverse(this string s)
        {
            return s.ToCharArray().Reverse().ToList().Implode("");
        }

        /// <summary>
        /// Odstraní z řetězce všechny výskyty daného znaku
        /// </summary>
        /// <param name="s"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string Delete(this string s, char c)
        {
            return s.Replace(c.ToString(), "");
        }

        public static string Remove(this string input, string old)
        {
            return input.Replace(old, string.Empty);
        }

        public static string Remove(this string input, string[] old)
        {
            string temp = input;
            old.ForEach(s => temp = temp.Remove(s));
            return temp;
        }

        public static string Replace(this string input, string[] oldStrings, string newString)
        {
            string temp = input;
            oldStrings.ForEach(old => temp = temp.Replace(old, newString));
            return temp;
        }

        public static string Replace(this string input, string[] oldString, string[] newString)
        {
            string temp = input;

            for (int i = 0; i < oldString.Length; i++)
                temp = temp.Replace(oldString[i], newString[i]);

            return temp;
        }

        /// <summary>
        /// Vytvoří nový řetězec, ve kterém count-krát vloží za sebe key.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string Repeat(string key, int count)
        {
            StringBuilder sb = new StringBuilder();
            count.Times(x => sb.Append(key));
            return sb.ToString();
        }

        /// <summary>
        /// Vrátí prvních length znaků řetězce
        /// </summary>
        /// <param name="s"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Take(this string s, int length)
        {
            return s.Substring(0, Math.Min(length, s.Length));
        }

        /// <summary>
        /// Náhodně přehází znaky v řetězci
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Shuffle(this string s)
        {
            return s.ToCharArray().Shuffle().Implode();
        }

        /// <summary>
        /// Vrací true, pokud řetězec obsahuje alespoň jeden znak v poli
        /// </summary>
        /// <param name="s"></param>
        /// <param name="chars"></param>
        /// <returns></returns>
        public static bool ContainsSome(this string s, char[] chars)
        {
            foreach (char c in chars)
            {
                if (s.Contains(c))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Vrací true v případě, že řetězec obsahuje všechny znaky v poli
        /// </summary>
        /// <param name="s"></param>
        /// <param name="chars"></param>
        /// <returns></returns>
        public static bool ContainsAll(this string s, char[] chars)
        {
            foreach (char c in chars)
            {
                if (!s.Contains(c))
                    return false;
            }

            return true;
        }
    }
}
