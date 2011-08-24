using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtensionMethods;
using System.Text.RegularExpressions;

namespace CryptanalysisCore
{
    public class Vigenere : Cipher
    {
        public const int KeyLengthAttack = 1;
        public const int BruteForce = 0;
        public const int MaxKeyLength = 20;

        private Random random = new Random();

        public Vigenere()
            : base()
        {
            ExceptionText = "Klíč musí být dlouhý alespoň dva znaky a musí být sloužen pouze z malých písmen anglické abecedy.";
        }

        public override string Decrypt(string ciphertext, string key)
        {
            base.Decrypt(ciphertext, key);

            char[] opentext = new char[ciphertext.Length];

            int keyIndex = 0;
            for (int i = 0; i < opentext.Length; i++)
            {
                char currCipherChar = ciphertext[i];

                if (currCipherChar == ' ')
                {
                    opentext[i] = ' ';
                }
                else
                {
                    opentext[i] = Analyse.MoveBackCharacter(ciphertext[i], key[keyIndex % key.Length]);
                    keyIndex++;
                }
            }
            return new string(opentext).ToLower();
        }

        protected override bool IsKeyValid(string key)
        {
            return key.Length >= 2 && Regex.IsMatch(key, @"^[a-z\?]+$");
        }

        public override string Encrypt(string opentext, string key)
        {
            base.Encrypt(opentext, key);

            char[] ciphertext = new char[opentext.Length];
            int opentextIndex = 0;
            for (int i = 0; i < ciphertext.Length; i++)
            {
                char currOpenChar = opentext[i];
                if (currOpenChar == ' ')
                {
                    ciphertext[i] = ' ';
                }
                else
                {
                    ciphertext[i] = Analyse.MoveCharacter(opentext[i], key[opentextIndex % key.Length]);
                    opentextIndex++;
                }
                
            }
            return new string(ciphertext).ToUpper();
        }

        public override string RandomKey()
        {
            int length = random.Next(5, MaxKeyLength);
            return Text.RandomString(length);
        }

        protected override void SetCrackMethods()
        {
            CrackMethods = new CrackMethod[] { BruteForceAttack, TestKeyLengthAttack };
        }

        /// <summary>
        /// Crackovací metoda, která se nejprve pokusí zjistit délku klíče
        /// a následně se pokusí prolomit šifru standardně.
        /// </summary>
        /// <param name="ciphertext"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        private List<string> TestKeyLengthAttack(string ciphertext, Storage.Languages language)
        {
            KeyLength keyLength = new KeyLength();
            var keysLength = keyLength.GetKeyLength(ciphertext).Take(6).ToArray();

            if (keysLength.Length == 0)
                keysLength = new int[MaxKeyLength - 2].Fill(x => x + 2);

            return BruteForceAttack(ciphertext, language, keysLength);
        }

        private List<string> BruteForceAttack(string ciphertext, Storage.Languages language, int[] keysLength)
        {
            ciphertext = Analyse.NormalizeText(ciphertext, Analyse.TextTypes.WithoutSpacesLower);
            Caesar caesar = new Caesar();
            Dictionary<int, string> crackedKeys = new Dictionary<int, string>();

            foreach(int keyLength in keysLength)
            {
                char[][] splitedChars = new char[keyLength][];

                for (int i = 0; i < keyLength; i++)
                    splitedChars[i] = GetNthChars(ciphertext, keyLength, i);

                char[] crackKeys = new char[keyLength];

                for (int i = 0; i < keyLength; i++)
                {
                    try
                    {
                        crackKeys[i] = caesar.Crack(Analyse.NormalizeText(new string(splitedChars[i]), Analyse.TextTypes.WithoutSpacesUpper), Caesar.TriangleID, language).First()[0];
                    }
                    catch (Exceptions.MatchNotFound)
                    {
                        crackKeys[i] = '?';
                    }
                }

                crackedKeys[keyLength] = new string(crackKeys);
            }

            var possKeys = crackedKeys.Where(x => x.Value.Length > 0).Select(x => x.Value).ToArray();

            Dictionary<string, double> simIndexes = new Dictionary<string, double>();

            for (int i = 0; i < possKeys.Length; i++)
            {
                string openttext = Decrypt(ciphertext, possKeys[i]);
                double simIndex = Analyse.SimilarityIndex(openttext, Storage.GetLangChar(language));
                simIndexes[possKeys[i]] = simIndex;
            }

            var result = simIndexes.OrderBy(x => x.Value).Select(x => x.Key).ToList();

            return result;
        }

        private List<string> BruteForceAttack(string ciphertext, Storage.Languages language)
        {
            int[] keysLength = new int[MaxKeyLength - 2].Fill(x => x + 2);
            return BruteForceAttack(ciphertext, language, keysLength);
        }

        /************* BRUTE FORCE ATTACK **************/

        private char[] GetNthChars(string ciphertext, int n, int move)
        {
            if (ciphertext.Length < n)
                return new char[0];

            int length = (int)Math.Ceiling((double)(ciphertext.Length - move) / (double)n);
            char[] chars = new char[length];
            int count = 0;

            for (int i = 0; i < ciphertext.Length - move; i += n)
                chars[count++] = ciphertext[i + move];

            return chars;
        }


        /******************* KONEC *********************/

        public int[] GetKey(string ciphertext)
        {
            KeyLength keyLength = new KeyLength();
            return keyLength.GetKeyLength(ciphertext).Where(x => x < 30).Where(x => x > 4).Take(1).ToArray();
        }


        public override string ToString()
        {
            return "Vigenere";
        }
    }
}
