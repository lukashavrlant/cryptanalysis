using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CryptanalysisCore;
using ExtensionMethods;
using System.Threading;

namespace UnitTests
{
    class VigenereTest : CipherTest
    {
        public VigenereTest(Action progress, Action<string> afterFinish, Texts texts)
            :base(progress, afterFinish, texts)
        {
            cipher = new Vigenere();
        }


        public void TestKeysLength(int paragraphCount, int letterCount, int take)
        {
            InitializeArrays(paragraphCount);

            int part = paragraphCount / Environment.ProcessorCount;

            Environment.ProcessorCount.Times(i =>
            {
                Thread thread = new Thread(() => TestKeyLengthHelp(part, letterCount, i, take));
                thread.Priority = ThreadPriority.BelowNormal;
                thread.Start();
            });
        }

        private void TestKeyLengthHelp(int parCount, int lettCount, int turn, int take)
        {
            KeyLength keyLength = new KeyLength();

            for (int i = 0; i < parCount; i++)
            {
                string opentext = texts.RandomParagraph(lettCount);
                string key = cipher.RandomKey();
                string ciphertext = cipher.Encrypt(opentext, key);
                try
                {
                    /*var crackKeys = keyLength.GetKeyLength(ciphertext).Take(take);
                    success[parCount * turn + i] = crackKeys.Contains(key.Length) ? 1 : 0;*/
                    var crackKeys = cipher.Crack(ciphertext, Vigenere.BruteForce, Form1.currentLanguage);
                    success[parCount * turn + i] = crackKeys.First().Length == key.Length ? 1 : 0;
                }
                catch (Exception)
                {

                }

                progress();
            }

            lock (lockObject)
            {
                SetDone(turn);
                if (AreThreadsDone())
                    afterFinish("Úspěšnost: " + ((int)(success.Average() * 100)).ToString() + "%");
            }
        }

        private bool KeyEqualsWithError(string key1, string key2, int maxErrors)
        {
            if (key1.Length != key2.Length)
                return false;

            int foundErrors = 0;

            for (int i = 0; i < key2.Length; i++)
            {
                if (key2[i] != key1[i])
                    foundErrors++;
            }

            return foundErrors <= maxErrors;
        }
    }
}
