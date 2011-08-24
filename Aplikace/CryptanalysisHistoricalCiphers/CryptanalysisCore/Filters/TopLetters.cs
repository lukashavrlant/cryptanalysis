using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtensionMethods;

namespace CryptanalysisCore
{
    class TopLetters
    {
        public Dictionary<char, char> Get(string ciphertext, LangCharacteristic lang)
        {
            ciphertext = ciphertext.Delete(' ');
            var letters = TextAnalysis.GetOccurrence(ciphertext, 1);
            var topLetters = letters.Take(2).Select(x => x.Key[0]).ToArray();

            Dictionary<char, char> res = new Dictionary<char, char>();
            res['e'] = topLetters[0];
            res['a'] = topLetters[1];

            return res;
        }
    }
}
