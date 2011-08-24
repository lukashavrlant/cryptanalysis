using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtensionMethods;
using System.Threading;
using System.Diagnostics;
using System.Text.RegularExpressions;
using CryptanalysisCore.Filters;


namespace CryptanalysisCore
{
    public class Monoalphabetic : Cipher
    {
        public Monoalphabetic()
            : base()
        {
            //ExceptionText = "Klíčem musí být všechny malá písmena anglické abecedy, libovolně rozpřeházená.";
            CipherType = Storage.Ciphers.monoalphabetic;
        }

        public override bool IsKeyValid(string key)
        {
            //return key.Length == Analyse.AlphabetLetterCount && Regex.IsMatch(key, @"^[a-z\?]+$");
            return Regex.IsMatch(key, @"^[a-zA-Z\?]+$");
        }

        protected override void SetCrackMethods()
        {
            CrackMethods = new CrackMethod[] { FrequencyAnalysis };
        }

        public override string Decrypt(string ciphertext, string key)
        {
            base.Decrypt(ciphertext, key);
            key = NormalizeKey(key);

            Dictionary<char, char> lettersSubstitution = new Dictionary<char, char>();
            int i = 0;

            Analyse.DoAlphabet(letter =>
            {
                char l = key[i++];
                if (l != Analyse.Unknown)
                    lettersSubstitution[l] = letter;
            });

            var missing = Storage.Letters.Select(x => x[0]).Except(lettersSubstitution.Keys).Select(x => x.ToString()).ToArray();
            string modifCiphertext = ciphertext.Replace(missing, Analyse.Unknown.ToString());
            return Analyse.SwitchLetters(modifCiphertext, lettersSubstitution).ToLower();
        }

        public override string Encrypt(string opentext, string key)
        {
            base.Encrypt(opentext, key);
            key = NormalizeKey(key);

            Dictionary<char, char> lettersSubstitution = new Dictionary<char, char>();
            int i = 0;

            Analyse.DoAlphabet(letter =>
                {
                    lettersSubstitution[letter] = key[i++];
                });

            string ciphertext = Analyse.SwitchLetters(opentext, lettersSubstitution);
            return ciphertext.ToUpper();
        }

        
        /// <summary>
        /// Provede útok na šifru pomocí frekvenční analýzy. 
        /// </summary>
        /// <param name="packet">Balíček obsahující zašifrovaný text</param>
        /// <returns>Balíček obsahující dešifrovaný text a klíč</returns>
        private List<string> FrequencyAnalysis(string ciphertext, Storage.Languages language)
        {
            int spacesCount = ciphertext.Count(' ');
            if (spacesCount > 0 && (ciphertext.Length / spacesCount < 15))
            {
                return FrequencyAnalysisWithSpaces(ciphertext, language);
            }
            else
            {
                /*var lang = Storage.GetLangChar(language);
                TopLetters topLettersAttack = new TopLetters();
                var topLetters = topLettersAttack.Get(ciphertext, lang);

                SamePatternAttack samePattern = new SamePatternAttack();
                var substitutions = samePattern.GetKey(ciphertext, lang, topLetters);
                var key = GetReverseKey(substitutions);
                return new List<string>() { key };*/

                throw new Exceptions.MatchNotFound();
            }
        }

        private string GetReverseKey(Dictionary<char, char> substitutions)
        {
            char[] key = new char[Analyse.AlphabetLetterCount].Fill('?');

            foreach (var pair in substitutions)
                key[(int)pair.Key - (int)'a'] = pair.Value;


            return new string(key);
        }


        /// <summary>
        /// Provede frekvenční analýzu na slovech, tj. text musí obsahovat mezery
        /// jako hranice pro slova.
        /// </summary>
        /// <param name="ciphertext">Šifrový text</param>
        /// <param name="language">Jazyk, který byl text zašifrován</param>
        /// <returns>Seznam možných klíčů</returns>
        public List<string> FrequencyAnalysisWithSpaces(string ciphertext, Storage.Languages language)
        {
            string key;
            string[] words = TextAnalysis.ToWords(ciphertext);
            UniqueWords uniqueWords = new UniqueWords(Storage.GetLangChar(language).UniqueWords);

            var result = uniqueWords.GetLettersSubst(words);

            if (result != null)
            {
                WordsCompleter completer = new WordsCompleter(words, result, language);
                var subst = completer.Complete();
                key = GetKey(subst);
            }
            else
            {
                throw new Exceptions.MatchNotFound();
            }


            return new List<string>() { key };
        }

        /// <summary>
        /// Metoda použitá v testování
        /// </summary>
        /// <param name="ciphertext"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public string UnitTextFrequency(string ciphertext, Storage.Languages language)
        {
            return FrequencyAnalysisWithSpaces(ciphertext, language)[0];
        }

        private string GetKey(Dictionary<char, char> substitutions)
        {
            char[] key = new char[Analyse.AlphabetLetterCount].Fill('?');

            foreach (var pair in substitutions)
                key[(int)pair.Value - (int)'a'] = pair.Key;


            return new string(key);
        }

        /// <summary>
        /// Doplní do klíče všechny chybějící písmena
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string NormalizeKey(string key)
        {
            string distKey = new string(key.ToCharArray().Distinct().ToArray());
            StringBuilder sb = new StringBuilder();
            sb.Append(distKey);

            Analyse.DoAlphabet(l =>
            {
                if (!distKey.Contains(l))
                {
                    sb.Append(l);
                }
            });

            return sb.ToString();
        }

        public override string RandomKey()
        {
            return Storage.Letters.Select(x => x[0]).ToList().Shuffle().Implode("");
        }

        public override string ToString()
        {
            return Storage.Ciphers.monoalphabetic.ToString();
        }
    }
}
