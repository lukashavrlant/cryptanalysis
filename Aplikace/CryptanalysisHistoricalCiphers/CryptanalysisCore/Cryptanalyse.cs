using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ExtensionMethods;

namespace CryptanalysisCore
{
    public class Cryptanalyse
    {
        public Action<CrackResult> FinishFunction
        { get; set; }

        public Action ProgressFunction
        { get; set; }

        public Action<Thread> AddThreadFunction
        { get; set; }

        public Cryptanalyse()
        {
            FinishFunction = null;
            ProgressFunction = null;
        }


        public void Attack(string ciphertext, Cipher[] ciphers, Storage.Languages language)
        {
            GetCrackResults(ciphertext, ciphers, language, crackResults =>
                {

                    LangCharacteristic langChar = Storage.GetLangChar(language);
                    var results = crackResults.Where(x => x != null)
                        .OrderBy(x => Analyse.SimilarityIndex(x.opentext, langChar))
                        .ToArray();

                    results = FixVigenere(results);

                    finish(results.First());
                });
        }

        private CrackResult[] FixVigenere(CrackResult[] results)
        {
            double min = 0.3;
            string vigenereKey = string.Empty;

            if (results.Length > 0 && results.First().cipher is Vigenere)
            {
                vigenereKey = results.First().key;

                double distinct = (double)vigenereKey.ToCharArray().Distinct().Count();
                double origin = (double)vigenereKey.Length;
                double poss = distinct / origin;

                if (poss < min)
                {
                    return results.Where(x => x.cipher is Caesar).ToArray();
                }
            }

            return results;
        }

        private void GetCrackResults(string ciphertext, Cipher[] ciphers, Storage.Languages language, Action<CrackResult[]> afterCrack)
        {
            int threadsCount = ciphers.Length;
            CrackResult[] results = new CrackResult[threadsCount];
            bool[] syncArray = new bool[threadsCount].Fill(false);
            object progressLockObject = new object();
            object endTestLockObject = new object();


            threadsCount.Times(i =>
                {
                    Thread thread = new Thread(() => 
                        {
                            try
                            {
                                var currentCipher = ciphers[i];
                                var keys = currentCipher.Crack(ciphertext, 0, language);
                                if (keys.Count > 0)
                                {
                                    var crackResult = new CrackResult();
                                    crackResult.key = keys[0];
                                    crackResult.opentext = currentCipher.Decrypt(ciphertext, crackResult.key);
                                    crackResult.cipher = currentCipher;
                                    results[i] = crackResult;
                                }
                                else
                                {
                                    throw new Exceptions.MatchNotFound();
                                }

                                lock (progressLockObject)
                                {
                                    progress();
                                }
                            }
                            catch (Exceptions.MatchNotFound)
                            {
                                lock (progressLockObject)
                                {
                                    progress();
                                }
                            }

                            lock (endTestLockObject)
                            {
                                syncArray[i] = true;
                                if (syncArray.All(x => x))
                                {
                                    afterCrack(results);
                                }
                            }
                        });

                    thread.Priority = ThreadPriority.Lowest;
                    thread.IsBackground = true;
                    addThread(thread);
                    thread.Start();
                });
        }

        private void progress()
        {
            if (ProgressFunction != null)
                ProgressFunction();
        }

        private void finish(CrackResult crackResult)
        {
            if (FinishFunction != null)
                FinishFunction(crackResult);
        }

        private void addThread(Thread thread)
        {
            if (AddThreadFunction != null)
                AddThreadFunction(thread);
        }
    }
}
