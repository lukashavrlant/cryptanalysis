using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtensionMethods;

namespace CryptanalysisCore
{
    public class Caesar : Cipher
    {
        public const int BruteForceID = 0;
        public const int TriangleID = 1;

        public Caesar()
            : base() 
        {
            ExceptionText = "Klíčem musí být právě jedno malé písmeno anglické abecedy.";
        }

        protected override void SetCrackMethods()
        {
            CrackMethods = new CrackMethod[] { BruteForceAttack, TriangleAttack };
        }

        /// <summary>
        /// Metoda zašifruje předaný text pomocí Caesarovy šifry. Každé písmeno v řetězci 
        /// bude posunuto o tolik znaků, kolik udává parametr 'Klíč'.
        /// </summary>
        /// <param name="OpenText">Řetězec, který má být zašifrován.</param>
        /// <param name="Key">Znak anglické abecedy, který bude hrát úlohu klíče.</param>
        /// <returns>Zašifrovaný řetězec.</returns>
        public override string Encrypt(string opentext, string key)
        {
            base.Encrypt(opentext, key);

            char[] ciphertext = new char[opentext.Length];

            for (int i = 0; i < ciphertext.Length; i++)
            {
                if (opentext[i] == ' ')
                    ciphertext[i] = ' ';
                else
                    ciphertext[i] = Analyse.MoveCharacter(opentext[i], key[0]);
            }

            return new string(ciphertext).ToUpper();
        }

        /*public override List<string> Crack(string ciphertext, int attackType, Storage.Languages language)
        {
            return CrackMethods[attackType](ciphertext, language);
        }*/

        /// <summary>
        /// Dešifruje zašifrovaný text podle předaného klíče.
        /// </summary>
        /// <param name="CipherText">Text, který má být dešifrován.</param>
        /// <param name="Key">Klíč, podle kterého má probíhat dešifrování.</param>
        /// <returns>Dešifrovaný text.</returns>
        public override string Decrypt(string ciphertext, string key)
        {
            base.Decrypt(ciphertext, key);

            // Transformujeme klic tak, abychom mohli pouzit standardni 
            // funkci na Caesarovu sifru
            int movedKey = (int)'z' - (int)key[0] + (int)'a' + 1;
            if (movedKey > 'z')
                movedKey -= Analyse.AlphabetLetterCount;

            char decryptKey = (char)movedKey;
            string cipherTextLower = ciphertext.ToLower();
            char[] opentext = new char[ciphertext.Length];
            for (int i = 0; i < opentext.Length; i++)
            {
                if (cipherTextLower[i] == ' ')
                    opentext[i] = ' ';
                else
                    opentext[i] = Analyse.MoveCharacter(cipherTextLower[i], decryptKey);
            }

            return new string(opentext).ToLower();
        }

        
        /// <summary>
        /// Ze dvou zadaných znaků vypočítá klíč, kterým byly tyto znaky zašifrovány.
        /// </summary>
        /// <param name="cipherLetter">Znak z otevřeného textu</param>
        /// <param name="openLetter">Znak ze zašifrovaného textu</param>
        /// <returns>Klíč</returns>
        private char GetKeyFromLetters(char cipherLetter, char openLetter)
        {
            int c = (int)cipherLetter - (int)'a';
            int d = (int)openLetter - (int)'a';

            int temp = (cipherLetter - openLetter + Analyse.AlphabetLetterCount);

            return ((char)((temp % Analyse.AlphabetLetterCount) + 'a'));
        }

        private char[] GetPolygonKeys(char[] topTextLetters, char[] topLetters, int complementSize)
        {
            return TextAnalysis.PolygonAttack(topTextLetters, topLetters, complementSize)
                .Select(x => GetKeyFromLetters(x[0], topLetters[0])).ToArray();
        }

        /// <summary>
        /// Provede trojúhelníkový útok na šifru
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        public List<string> TriangleAttack(string ciphertext, Storage.Languages language)
        {
            bool modern = true;

            if (modern)
            {
                ciphertext = Analyse.NormalizeText(ciphertext, Analyse.TextTypes.WithoutSpacesLower);
                int polygonSize = 3;
                int complementSize = 10;
                var topLetters = Storage.GetLangChar(language).Letters.OrderByDescending(x => x.Value).Take(polygonSize).Select(x => x.Key[0]).ToArray();
                var bottomLetters = Storage.GetLangChar(language).Letters.OrderBy(x => x.Value).Take(polygonSize).Select(x => x.Key[0]).ToArray();
                var letters = TextAnalysis.GetOccurrence(ciphertext, 1);
                var topTextLetters = letters.OrderByDescending(x => x.Value).Take(complementSize).Select(x => x.Key[0]).ToArray();
                var bottomTextLetters = letters.OrderBy(x => x.Value).Take(complementSize).Select(x => x.Key[0]).ToArray();

                var keysTop = GetPolygonKeys(topTextLetters, topLetters, complementSize);
                var keysBottom = GetPolygonKeys(bottomTextLetters, bottomLetters, complementSize);

                if (keysTop.Length == 0)
                    throw new Exceptions.MatchNotFound();

                var interKeys = keysBottom.Intersect(keysTop).ToList();

                if (interKeys.Count > 0)
                {
                    return interKeys.Select(x => x.ToString()).ToList();
                }
                else
                {
                    var unionKeys = keysBottom.Union(keysTop).Select(x => x.ToString()).ToList();
                    if (unionKeys.Count > 0)
                    {
                        return unionKeys;
                    }
                    else
                    {
                        throw new Exceptions.MatchNotFound();
                    }
                }
            }
            else
            {

                char[] keysTop = GetTriangle(ciphertext, true, language);
                char[] keysLess = GetTriangle(ciphertext, false, language);

                char[] universalKeys = keysLess.Intersect(keysTop).ToArray();
                string key;

                if (universalKeys.Length == 1)
                {
                    key = universalKeys[0].ToString();
                    return new List<string>() { key };
                }
                else
                {
                    char[] unionsKeys = keysLess.Union(keysTop).ToArray();
                    if (unionsKeys.Length != 0)
                    {
                        key = unionsKeys[0].ToString();
                        return new List<string>() { key };
                    }
                }

                throw new Exceptions.MatchNotFound();
            }
        }

        /// <summary>
        /// Vrátí pole klíčů, které jsou možnými klíči dané šifry
        /// </summary>
        /// <param name="packet">Informace o šifrovém textu</param>
        /// <returns>Pole klíčů</returns>
        private char[] GetTriangle(string ciphertext, bool orderByDescending, Storage.Languages language)
        {
            char[] topCiphertextLetters;
            char[] topLanguagesLetters;
            int max = 9;

            if (orderByDescending)
            {
                topCiphertextLetters = Analyse.CharsRelativeOccurrence(ciphertext.ToLower()).OrderByDescending(x => x.Value).Take(max).Select(x => x.Key.ToString().ToLower()[0]).ToArray();
                topLanguagesLetters = Storage.GetLangChar(language).Letters.Where(x => x.Key != " ").OrderByDescending(x => x.Value).Take(3).Select(x => x.Key.ToLower()[0]).ToArray();
            }
            else
            {
                topCiphertextLetters = Analyse.CharsRelativeOccurrence(ciphertext.ToLower()).OrderBy(x => x.Value).Take(max).Select(x => x.Key.ToString().ToLower()[0]).ToArray();
                topLanguagesLetters = Storage.GetLangChar(language).Letters.Where(x => x.Key != " ").OrderBy(x => x.Value).Take(3).Select(x => x.Key.ToLower()[0]).ToArray();
            }

            List<char[]> triples = FindEquivalentTriple(topLanguagesLetters, topCiphertextLetters);

            if (triples.Count == 0)
                throw new Exceptions.MatchNotFound();

            return GetKeysFromTriples(topLanguagesLetters[0], triples);
        }

        /// <summary>
        /// Vrátí seznam klíčů, kterým byl zašifrován předaný text
        /// </summary>
        /// <param name="topLetter">Nejčastější písmeno v daném jazyce</param>
        /// <param name="triples">Seznam trojic</param>
        /// <returns>Pole klíčů</returns>
        private char[] GetKeysFromTriples(char topLetter, List<char[]> triples)
        {
            List<char> keys = new List<char>();
            triples.ForEach(triple => keys.Add(GetKeyFromLetters(triple[0], topLetter)));
            return keys.ToArray();
        }

        /// <summary>
        /// Zjistí všechny trojice písmen v zašifrovaném textu, které mají stejné vzdálenosti
        /// jako nejčastější písmena v běžném textu.
        /// </summary>
        /// <param name="topLanguagesLetters"></param>
        /// <param name="topCiphertextLetters"></param>
        /// <returns>Seznam odpovídajících trojic písmen.</returns>
        private List<char[]> FindEquivalentTriple(char[] topLanguagesLetters, char[] topCiphertextLetters)
        {
            int[] distances = GetDistances(topLanguagesLetters);
            List<char[]> equivalentTriples = new List<char[]>();


            for (int i = 0; i < topCiphertextLetters.Length; i++)
            {
                for (int j = 0; j < topCiphertextLetters.Length; j++)
                {
                    if (Analyse.MinDistance(topCiphertextLetters[i], topCiphertextLetters[j]) == distances[0])
                    {
                        for (int k = 0; k < topCiphertextLetters.Length; k++)
                        {
                            if (k != i && k != j)
                            {
                                if (Analyse.MinDistance(topCiphertextLetters[j], topCiphertextLetters[k]) == distances[1] &&
                                    Analyse.MinDistance(topCiphertextLetters[i], topCiphertextLetters[k]) == distances[2])
                                {
                                    equivalentTriples.Add(new char[] { topCiphertextLetters[i], topCiphertextLetters[j], topCiphertextLetters[k] });
                                }
                            }
                        }
                    }
                }
            }

            return equivalentTriples;
        }

        /// <summary>
        /// Vrací pole reprezentující vzdálenosti v předaných písmenech
        /// </summary>
        /// <param name="topLanguagesLetters">pole písmen, ve kterých budeme hledat vzdálenosti</param>
        /// <returns>Pole vzdáleností.</returns>
        private int[] GetDistances(char[] topLanguagesLetters)
        {
            int[] distances = new int[3];
            Cycles.From0ToI(3, i => distances[i] = Analyse.MinDistance(topLanguagesLetters[i], topLanguagesLetters[(i + 1) % 3]));
            return distances;
        }

        /// <summary>
        /// Provede útok testováním všech dostupných klíčů 
        /// a následně vybere nejpravděpodobnější variantu.
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        public List<string> BruteForceAttack(string ciphertext, Storage.Languages language)
        {
            Dictionary<char, double> probability = new Dictionary<char, double>();
            Caesar caesar = new Caesar();
            string opentext;

            Analyse.DoAlphabet(letter =>
            {
                opentext = caesar.Decrypt(ciphertext, letter.ToString());
                probability[letter] = Analyse.SimilarityIndex(Analyse.NormalizeText(opentext, Analyse.TextTypes.WithoutSpacesLower), Storage.GetLangChar(language));
            });

            var resultPacket = probability.OrderBy(x => x.Value).ToArray()[0].Key;

            return new List<string>() { resultPacket.ToString() };
        }

        /// <summary>
        /// Zjistí, zda je předaný klíč platný klíč Caesarovy šifry
        /// </summary>
        /// <param name="Key">Šifrovací klíč</param>
        /// <returns>True v případě, že je klíč platný, false jinak</returns>
        protected override bool IsKeyValid(string key)
        {
            return key.Length == 1 && (key[0] >= 'a' && key[0] <= 'z');
        }

        /// <summary>
        /// Vygeneruje náhodný klíč pro Caesarovu šifru
        /// Nevygeneruje špatný klíč 'a'.
        /// </summary>
        /// <returns>Náhodný klíč.</returns>
        public override string RandomKey()
        {
            Random Rand = new Random();
            return ((char)((int)'b' + Rand.Next(24))).ToString();
        }

        public override string ToString()
        {
            return "Caesar";
        }
    }
}
