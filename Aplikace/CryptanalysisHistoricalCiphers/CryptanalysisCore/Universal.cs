using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryptanalysisCore
{
    public static class Universal
    {
        public static CrackResult[] Attack(string ciphertext, Cipher[] ciphers, Storage.Languages language)
        {
            var crackResults = GetCrackResults(ciphertext, ciphers, language);

            LangCharacteristic langChar = Storage.GetLangChar(language);
            var results = crackResults.OrderBy(x => Analyse.SimilarityIndex(x.opentext, langChar)).ToArray();


            return results;
        }

        private static List<CrackResult> GetCrackResults(string ciphertext, Cipher[] ciphers, Storage.Languages language)
        {
            List<CrackResult> results = new List<CrackResult>();

            foreach (var cipher in ciphers)
            {
                try
                {
                    var keys = cipher.Crack(ciphertext, 0, language);
                    if (keys.Count > 0)
                    {
                        var result = new CrackResult();
                        result.key = keys[0];
                        result.opentext = cipher.Decrypt(ciphertext, result.key);
                        result.cipher = cipher;
                        results.Add(result);
                    }
                }
                catch (Exceptions.MatchNotFound)
                { }
            }

            return results;
        }
    }
}
