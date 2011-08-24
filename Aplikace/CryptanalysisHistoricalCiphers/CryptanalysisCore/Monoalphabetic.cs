using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtensionMethods;
using System.Threading;
using System.Diagnostics;
using System.Text.RegularExpressions;


namespace CryptanalysisCore
{
    public class Monoalphabetic : Cipher
    {
        public Monoalphabetic()
            : base()
        {
            ExceptionText = "Klíčem musí být všechny malá písmena anglické abecedy, libovolně rozpřeházená.";
        }

        protected override bool IsKeyValid(string key)
        {
            return key.Length == Analyse.AlphabetLetterCount && Regex.IsMatch(key, @"^[a-z\?]+$");
        }

        protected override void SetCrackMethods()
        {
            CrackMethods = new CrackMethod[] { FrequencyAnalysis };
        }

        public override string Decrypt(string ciphertext, string key)
        {
            base.Decrypt(ciphertext, key);

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
                throw new Exceptions.MatchNotFound();
            }
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

        /*public int UnitTextFrequency(string ciphertext, Storage.Languages language)
        {
            var lettersOccurence = TextAnalysis.GetOccurrence(ciphertext.Delete(' '), Analyse.OneGram);
            string[] words = TextAnalysis.ToWords(ciphertext);
            var splitedArray = words.Distinct().ToArray().Split(Environment.ProcessorCount);
            Dictionary<string, string>[] decihpers = new Dictionary<string, string>[Environment.ProcessorCount];

            //Parallel.For(i => decihpers[i] = GetUniqueWords(splitedArray[i], language));

            var startLetters = TextAnalysis.StartLetters(words);
            var endLetters = TextAnalysis.EndLetters(words);
            var letters = TextAnalysis.GetOccurrence(ciphertext, 1);
            var bigrams = TextAnalysis.GetOccurrence(ciphertext, 2).Where(x => !x.Key.Contains(' ')).ToDictionary(x => x.Key, x => x.Value);
            var nearbyLetters = TextAnalysis.NearbyLetters(words);
            var nextLetters = nearbyLetters.NextLetters;
            var prevLetters = nearbyLetters.PreviousLetters;

            LettersMatrix matrix = new LettersMatrix();
            /*LangCharacteristic langChar = Storage.GetLangChar(language);
            int index = 0;
            matrix.MergeMatrix(startLetters, langChar.StartLetters, coeffs[index++]);
            matrix
            //MergeMatrix(letters, langChar.Letters, matrix, coeffs[0]);
            //MergeMatrix(endLetters, langChar.EndLetters, matrix, coeffs[1]);
            MergeMatrix(startLetters, langChar.StartLetters, matrix, coeffs[index++]);
            MergeMatrix(nextLetters, langChar.NextLetters, matrix, coeffs[index++]);
            MergeMatrix(prevLetters, langChar.PrevLetters, matrix, coeffs[index++]);
            var testLetters = langChar.Letters.Take(6).Where(x => x.Key != " ").Select(x => x.Key).ToArray();
            //return (int)Math.Round(matrix.Success(testLetters, 1));
            //return matrix.SmartSuccess(testLetters, 2);
            return 0;
        }*/

        private string GetKey(Dictionary<char, char> substitutions)
        {
            char[] key = new char[Analyse.AlphabetLetterCount].Fill('?');

            foreach (var pair in substitutions)
                key[(int)pair.Value - (int)'a'] = pair.Key;


            return new string(key);
        }

        public override string RandomKey()
        {
            return Storage.Letters.Select(x => x[0]).ToList().Shuffle().Implode("");
        }

        public override string ToString()
        {
            return "Monoalphabetic";
        }
    }
}
