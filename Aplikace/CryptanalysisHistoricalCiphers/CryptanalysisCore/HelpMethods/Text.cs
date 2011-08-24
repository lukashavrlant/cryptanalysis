using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtensionMethods;

namespace CryptanalysisCore
{
    public static class Text
    {
        private static Random random = new Random();

        public static char RandomChar()
        {
            return (char)('a' + random.Next(0, Analyse.AlphabetLetterCount - 1));
        }

        public static string RandomString(int length)
        {
            char[] key = new char[length];

            for (int i = 0; i < length; i++)
                key[i] = Text.RandomChar();

            return key.Implode();
        }
    }
}
