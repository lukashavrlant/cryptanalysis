using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtensionMethods;
using CryptanalysisCore.Structures;

namespace CryptanalysisCore
{
    class DictionaryAttack
    {
        /// <summary>
        /// Neobsazené políčko v možnsotech permutace klíče
        /// </summary>
        private const int Empty = -1;

        /// <summary>
        /// Uchováva nalezené transpoziční tabulky
        /// </summary>
        private Dictionary<string, List<int[]>> crossTables;

        /// <summary>
        /// Vyfiltruje slova podle toho, jestli dané slovo lze poskládat
        /// z předaných písmen. Kontroluje i platnost sloupců
        /// </summary>
        /// <param name="ciphertext"></param>
        /// <param name="currWordIndex"></param>
        /// <param name="keyLength"></param>
        /// <returns></returns>
        public string[] CrossFilter(string[] ciphertext, string[] dictionary)
        {
            crossTables = new Dictionary<string, List<int[]>>();
            var lettersCount = GetLettersCount(ciphertext.Implode());
            var matchWords = GetMatchWords(lettersCount, dictionary);
            var crossMatchWords = GetCrossMatchWords(ciphertext, matchWords);
            return crossMatchWords.ToArray();
        }

        /// <summary>
        /// Vyfiltruje ta slova, která lze poskládat z více sloupců textu
        /// </summary>
        /// <param name="words"></param>
        /// <returns></returns>
        private string[] GetCrossMatchWords(string[] ciphertext, string[] words)
        {
            int wordsLimit = 500;

            List<string>[] lineMatch = new List<string>[ciphertext.Length];
            List<string> crossLineMatch = new List<string>();

            var lettersCount = GetLettersCount(ciphertext);
            var lettersPositions = GetLettersPositions(ciphertext);

            for (int i = 0; i < lineMatch.Length; i++)
                lineMatch[i] = new List<string>();

            foreach (string word in words)
            {
                if (lettersCount[0].Count < wordsLimit && LettersMatch(lettersCount[0], word))
                {
                    lineMatch[0].Add(word);
                    continue;
                }

                /*if (LettersMatch(lettersCount[1], word))
                {
                    lineMatch[1].Add(word);
                    continue;
                }*/

                if (crossLineMatch.Count < (wordsLimit * 2) && CrossMatch(ciphertext, word, lettersPositions))
                {
                    crossLineMatch.Add(word);
                    continue;
                }
            }

            

            bool res1 = lineMatch[0].Contains("policiste");
            bool res2 = lineMatch[0].Contains("podala");
            bool res3 = crossLineMatch.Contains("zatim");
            bool controlResult = res1 && res2 && res3;

            var lineTables = GetLineTables(lineMatch[0].ToArray(), lettersPositions[0]);
            var result = GetNeighWords(lineMatch[0].ToArray(), lineTables);
            var oneMistakeTables = result.Where(x => x.Count(y => y == -1) == 1).ToArray();
            var noMistakeTables = result.Where(x => x.Count(y => y == -1) == 0).ToArray();
            RepairTables(oneMistakeTables);
            var keys = GetKeysFromTables(oneMistakeTables.Union(noMistakeTables).ToArray());
            return keys.Distinct().ToArray();
            //return lineMatch.ToArray();
        }

        /// <summary>
        /// Vytvoří z tabulek klíče
        /// </summary>
        /// <param name="tables"></param>
        /// <returns></returns>
        private string[] GetKeysFromTables(int[][] tables)
        {
            List<string> keys = new List<string>();

            foreach (int[] table in tables)
            {
                keys.Add(GetKeyFromTable(table));
            }

            return keys.ToArray();
        }

        /// <summary>
        /// Vytvoří z tabulky šifrový klíč
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        private string GetKeyFromTable(int[] table)
        {
            var res = table.Select(x => (char)(x + 'a')).ToArray();
            return new string(res);
        }

        /// <summary>
        /// Doplní do tabulky chybející index
        /// </summary>
        /// <param name="errorTables"></param>
        private void RepairTables(int[][] errorTables)
        {
            foreach (int[] table in errorTables)
            {
                int missing = Empty;

                for (int j = 0; j < table.Length; j++)
                {
                    if (!table.Contains(j))
                    {
                        missing = j;
                        break;
                    }
                }

                table[Array.IndexOf(table, Empty)] = missing;
            }
        }

        /// <summary>
        /// Vrátí všechny permutace, kterými lze složit slovo v jednom řádku
        /// </summary>
        /// <param name="lineWords"></param>
        /// <param name="positions"></param>
        /// <returns></returns>
        private Dictionary<string, List<List<int>>> GetLineTables(string[] lineWords, Dictionary<char, int[]> positions)
        {
            var lineTables = new Dictionary<string, List<List<int>>>();

            foreach (string word in lineWords)
                lineTables[word] = GetAllPerms(positions, word, word.Length);

            return lineTables;
        }

        private List<int[]> GetNeighWords(string[] oneLineWords, Dictionary<string, List<List<int>>> linePermutations)
        {
            Dictionary<string, List<string>> neighWords = new Dictionary<string,List<string>>();
            List<int[]> tables = new List<int[]>();

            foreach (var crossWord in crossTables)
            {
                foreach (string lineWord in oneLineWords)
                {
                    foreach (var crossTable in crossWord.Value)
                    {
                        bool done = false;

                        if (crossWord.Key == "nebudeme" && lineWord == "mozna")
                            ToString();

                        foreach (var linePerm in linePermutations[lineWord])
                        {
                            var modifTable = TwoWordsMatch(crossTable, linePerm.ToArray());
                            if (modifTable != null)
                            {
                                if(neighWords.ContainsKey(crossWord.Key))
                                    neighWords[crossWord.Key].Add(lineWord);
                                else
                                    neighWords[crossWord.Key] = new List<string>() { lineWord };

                                done = true;
                                tables.Add(modifTable);
                                break;
                            }
                        }

                        if (done)
                            break;
                    }
                }
            }

            //return neighWords;
            return tables;
        }

        /// <summary>
        /// Vrátí upravenou tabulku crossTable, do které vloží permutaci tak, aby to sedělo
        /// </summary>
        /// <param name="crossTable"></param>
        /// <param name="permutation"></param>
        /// <returns></returns>
        private int[] TwoWordsMatch(int[] crossTable, int[] permutation)
        {
            // Zjistíme si index prvního písmene
            int startIndex = 0;
            for (int i = 0; i < crossTable.Length; i++)
            {
                if (crossTable[i] == -1 || crossTable[i] == permutation[0])
                {
                    startIndex = i;
                    break;
                }
            }

            int stopIndex = Array.LastIndexOf(crossTable, -1);
            if (stopIndex == -1)
                stopIndex = crossTable.Length;


            // Vleze se tam permutace?
            if (startIndex + permutation.Length > stopIndex)
                return null;

            // Zjistíme, jestli se tam ta permutace vleze
            for (int j = 0; j < permutation.Length; j++)
            {
                int index = j + startIndex;

                if (!((crossTable[index] == Empty && !crossTable.Contains(permutation[j])) || crossTable[index] == permutation[j]))
                {
                    return null;
                }
            }

            return AddToTable(crossTable, permutation, startIndex);
        }

        /// <summary>
        /// Přidá do tabulky na určené místo permutaci
        /// </summary>
        /// <param name="table"></param>
        /// <param name="perm"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private int[] AddToTable(int[] table, int[] perm, int index)
        {
            int[] modifTable = (int[])table.Clone();

            for (int i = index; i < perm.Length + index; i++)
            {
                modifTable[i] = perm[i - index];
            }

            return modifTable;
        }

        /// <summary>
        /// Zjistí, zda lze dané slovo poskládat z daných sloupců
        /// </summary>
        /// <param name="ciphertext"></param>
        /// <param name="word"></param>
        /// <param name="lettersPositions"></param>
        /// <returns></returns>
        private bool CrossMatch(string[] ciphertext, string word, Dictionary<char, int[]>[] lettersPositions)
        {
            int startLength = GetStartLetters(lettersPositions[0], word);
            int lastLength = GetStartLetters(lettersPositions[1], word.Reverse());

            if ((startLength + lastLength) < word.Length)
                return false;
            else
            {
                var match = SplitMatch(ciphertext, word, startLength, lastLength, lettersPositions);
                if (match.Count > 0)
                {
                    crossTables[word] = match;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Zjistí, zda lze dané slovo poskládat z daných sloupců
        /// </summary>
        /// <param name="ciphertext"></param>
        /// <param name="word"></param>
        /// <param name="startLength"></param>
        /// <param name="lastLength"></param>
        /// <param name="lettersPositions"></param>
        /// <returns></returns>
        private List<int[]> SplitMatch(string[] ciphertext, string word, int startLength, int lastLength, Dictionary<char, int[]>[] lettersPositions)
        {
            bool firstLeft = startLength < lastLength;
            int minLetters = Math.Min(lastLength, startLength) - ((startLength + lastLength) - word.Length);
            List<List<int>> first, last;

            if (firstLeft)
            {
                first = GetAllPerms(lettersPositions[0], word, minLetters);
                last = GetAllPerms(lettersPositions[1], word.Substring(minLetters), word.Length - minLetters);
            }
            else
            {
                first = GetAllPerms(lettersPositions[0], word, word.Length - minLetters);
                last = GetAllPerms(lettersPositions[1], word.Substring(word.Length - minLetters), minLetters);
            }

            return SplitMatchLists(first, last, ciphertext[0].Length);
        }

        /// <summary>
        /// Kontroluje, zda je některé permutace platné přeházení slova
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="keyLength"></param>
        /// <returns></returns>
        private List<int[]> SplitMatchLists(List<List<int>> left, List<List<int>> right, int keyLength)
        {
            List<int[]> match = new List<int[]>();

            foreach (var l in left)
            {
                foreach (var r in right)
                {
                    var table = SplitMatchLists(l, r, keyLength);
                    if (table != null)
                    {
                        match.Add(table);
                    }
                }
            }

            return match;
        }

        /// <summary>
        /// Kontroluje, zda je daná permutace platné přehození slova
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="keyLength"></param>
        /// <returns></returns>
        private int[] SplitMatchLists(List<int> first, List<int> last, int keyLength)
        {
            int[] checker = new int[keyLength].Fill(Empty);

            for (int i = 0; i < last.Count; i++)
                checker[i] = last[i];

            int indexArr = keyLength - 1;
            for (int i = first.Count - 1; i >= 0; i--)
            {
                if (checker[indexArr] == Empty)
                    checker[indexArr] = first[i];
                else
                    if (checker[indexArr] != first[i])
                        return null;

                indexArr--;
            }

            bool res = IsTableValid(checker);

            return res ? checker : null;
        }

        /// <summary>
        /// Zjistí, jestli předaná tabulka neobsahuje žádný index dvakrát
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        private bool IsTableValid(int[] table)
        {
            var tester = table.Where(x => x != Empty).ToArray();
            bool res = tester.Length == tester.Distinct().Count();
            return res;
        }

        /// <summary>
        /// Vrátí všechny permutace, jak lze slovo poskládat
        /// </summary>
        /// <param name="positions"></param>
        /// <param name="word"></param>
        /// <param name="lettersCount"></param>
        /// <returns></returns>
        private List<List<int>> GetAllPerms(Dictionary<char, int[]> positions, string word, int lettersCount)
        {
            List<List<int>> routeResults = new List<List<int>>();
            Route(new List<int>(), word, 0, lettersCount, positions, routeResults);
            return routeResults;
        }

        private List<List<int>> GetAllPerms(Dictionary<char, int[]> positions, string word)
        {
            return GetAllPerms(positions, word, word.Length);
        }

        /// <summary>
        /// Pomocná funkce pro sestavení všech permutací
        /// </summary>
        /// <param name="visited"></param>
        /// <param name="word"></param>
        /// <param name="currPosition"></param>
        /// <param name="length"></param>
        /// <param name="positions"></param>
        /// <param name="routeResults"></param>
        private void Route(List<int> visited, string word, int currPosition, int length, Dictionary<char, int[]> positions, List<List<int>> routeResults)
        {
            if (currPosition == length)
            {
                routeResults.Add(visited);
            }
            else
            {
                char key = word[currPosition];
                if (positions.ContainsKey(key))
                {
                    foreach (int pos in positions[key])
                    {
                        if (!visited.Contains(pos))
                        {
                            var next = new List<int>(visited);
                            next.Add(pos);
                            Route(next, word, currPosition + 1, length, positions, routeResults);
                        }
                    }
                }
                else
                {
                    routeResults.Clear();
                }
            }
        }

        /// <summary>
        /// Metoda vrátí všechna písmena včetně pozic, ze kterých lze
        /// poskládat libovolně dlouhý začátek slova.
        /// </summary>
        /// <param name="lettersPositions"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        private int GetStartLetters(Dictionary<char, int[]> lettersPositions, string word)
        {
            int wordLengthCounter = 0;
            Dictionary<char, int> letters = new Dictionary<char, int>();

            foreach (char letter in word)
            {
                if (lettersPositions.ContainsKey(letter))
                {
                    if (letters.ContainsKey(letter))
                    {
                        if (lettersPositions[letter].Length > letters[letter])
                        {
                            letters[letter]++;
                            wordLengthCounter++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        letters[letter] = 1;
                        wordLengthCounter++;
                    }
                }
                else
                {
                    break;
                }
            }

            return wordLengthCounter;
        }


        /// <summary>
        /// Vrátí pozice jednotlivých písmen ve slově
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private Dictionary<char, int[]> GetLettersPositions(string text)
        {
            Dictionary<char, List<int>> positions = new Dictionary<char, List<int>>();

            for (int i = 0; i < text.Length; i++)
            {
                char currLetter = text[i];
                if (positions.ContainsKey(currLetter))
                {
                    positions[currLetter].Add(i);
                }
                else
                {
                    positions[currLetter] = new List<int>() { i };
                }
            }

            return positions.ToDictionary(x => x.Key, x => x.Value.ToArray());
        }

        private Dictionary<char, int[]>[] GetLettersPositions(string[] text)
        {
            var positions = new Dictionary<char, int[]>[text.Length];
            for (int i = 0; i < text.Length; i++)
            {
                positions[i] = GetLettersPositions(text[i]);
            }
            return positions;
        }

        /// <summary>
        /// Vrátí všechna slova, která lze vyskládat z daných písmen
        /// </summary>
        /// <param name="lettersCount"></param>
        /// <returns></returns>
        private string[] GetMatchWords(Dictionary<char, int> lettersCount, string[] dictionary)
        {
            List<string> result = new List<string>();

            foreach (string dicWord in dictionary)
            {
                if (LettersMatch(lettersCount, dicWord))
                    result.Add(dicWord);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Rozhoduje, zda je dané slovo složeno z předaných písmen
        /// </summary>
        /// <param name="lettersCount"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        private bool LettersMatch(Dictionary<char, int> lettersCount, string word)
        {
            var wordLetters = GetLettersCount(word);

            foreach (var pair in wordLetters)
            {
                if (!(lettersCount.ContainsKey(pair.Key) && pair.Value <= lettersCount[pair.Key]))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// rozdohuje, zda je dané slovo složeno z některých předaných slovníků písmen
        /// </summary>
        /// <param name="lettersCount"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        private bool LettersMatch(Dictionary<char, int>[] lettersCount, string word)
        {
            for (int i = 0; i < lettersCount.Length; i++)
            {
                if (LettersMatch(lettersCount[i], word))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Vrátí počet písmen ve slovech
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private Dictionary<char, int>[] GetLettersCount(string[] text)
        {
            Dictionary<char, int>[] lettersCount = new Dictionary<char,int>[text.Length];
            text.Length.Times(i => lettersCount[i] = GetLettersCount(text[i]));
            return lettersCount;
        }

        /// <summary>
        /// Vrátí počet písmen ve slově
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private Dictionary<char, int> GetLettersCount(string text)
        {
            Dictionary<char, int> lettersCount = new Dictionary<char, int>();

            foreach (char letter in text)
            {
                if (lettersCount.ContainsKey(letter))
                    lettersCount[letter]++;
                else
                    lettersCount[letter] = 1;
            }

            return lettersCount;
        }

        /// <summary>
        /// Vrátí pole polí seřazených písmen
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private char[][] GetOrderedLetters(string[] text)
        {
            char[][] letters = new char[text.Length][];

            for (int i = 0; i < text.Length; i++)
                letters[i] = text[i].ToCharArray().OrderBy(x => x).ToArray();

            return letters;
        }



        /*****************************************************************************/
        /************************* One Big Word Attack  ******************************/
        /*****************************************************************************/

        public string[] GetKeys(string[] rows, string[] topWords)
        {
            int keyLength = rows[0].Length;
            string[] wordSameLength = topWords.Where(x => x.Length == keyLength).ToArray();
            Dictionary<string, int> passedRows = GetPassedRows(wordSameLength, rows);
            string[] possKeys = new string[0];

            foreach (var pair in passedRows)
            {
                var positions = GetLettersPositions(rows[pair.Value]);
                var perms = GetAllPerms(positions, pair.Key);
                var keys = GetKeysFromTables(perms.Select(x => x.ToArray()).ToArray());
                possKeys = possKeys.Union(keys).ToArray();
            }

            possKeys = possKeys.Distinct().ToArray();

            return possKeys;
        }

        /// <summary>
        /// Vrátí slova, která lze poskládat v některém řádku
        /// </summary>
        /// <param name="lettersCount"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        private Dictionary<string, int> GetPassedRows(string[] wordSameLength, string[] rows)
        {
            Dictionary<string, int> passedRows = new Dictionary<string, int>();
            Dictionary<string, LettersCount> lettersCount = new Dictionary<string, LettersCount>();
            wordSameLength.ForEach(x => lettersCount[x] = new LettersCount(x));

            LettersCount[] rowsLetterCount = new LettersCount[rows.Length];
            rows.Length.Times(i => rowsLetterCount[i] = new LettersCount(rows[i]));

            for (int i = 0; i < rows.Length; i++)
            {
                foreach (var word in lettersCount)
                {
                    if (word.Value.MatchLetters(rowsLetterCount[i]))
                    {
                        passedRows[word.Key] = i;
                        break;
                    }
                }
            }

            return passedRows;
        }
    }
}
