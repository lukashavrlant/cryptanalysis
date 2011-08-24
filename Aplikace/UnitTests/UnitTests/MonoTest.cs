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
    class MonoTest : CipherTest
    {
        private string[] uniqueWords;

        public MonoTest(Action progress, Action<string> afterFinish, Texts texts)
            :base(progress, afterFinish, texts)
        {
            uniqueWords = File.ReadAllText(Storage.StatsFolderPath + "czech/" + "unique.txt").Split(' ');
            cipher = new Monoalphabetic();
        }

        public void DoTest(int paragraphCount, int letterCount)
        {
            InitializeArrays(paragraphCount);
            int part = paragraphCount / Environment.ProcessorCount;

            Environment.ProcessorCount.Times(i =>
                {
                    Thread thread = new Thread(() => DoTestHelp(part, letterCount, i));
                    thread.Priority = ThreadPriority.BelowNormal;
                    thread.Start();
                });
        }

        public void DoTestHelp(int paragraphCount, int letterCount, int turn)
        {
            CryptanalysisCore.Monoalphabetic cipher = new CryptanalysisCore.Monoalphabetic();

            for(int i = 0; i < paragraphCount; i++)
            {
                string par = texts.RandomParagraph(letterCount);
                string key = cipher.RandomKey();
                string ciphertext = cipher.Encrypt(par, key);
                try
                {
                    string crackKey = cipher.UnitTextFrequency(ciphertext, Form1.currentLanguage);
                    int similarity = KeySimilarity(key, crackKey);
                    success[paragraphCount * turn + i] = similarity;
                    notSuccess[paragraphCount * turn + i] = KeyNotSimilarity(key, crackKey);
                }
                catch (CryptanalysisCore.Exceptions.MatchNotFound)
                {
                    success[paragraphCount * turn + i] = -1;
                }
                
                progress();
            }

            done[turn] = 1;

            if (done.Sum() == Environment.ProcessorCount)
                afterFinish(String.Format("průměr: {0}, neprolomeno: {1} %, počet chybných substitucí: {2}.",
                    ((int)success.Where(x => x > -1).Average()).ToString(),
                    (int)((double)success.Where(x => x == -1).Count() / (double)success.Length * 100),
                    ((int)notSuccess.Sum()).ToString()));

        }

        private int KeySimilarity(string originKey, string crackKey)
        {
            int success = 0;

            for (int i = 0; i < originKey.Length; i++)
            {
                if (originKey[i] == crackKey[i])
                    success++;
            }

            return success;
        }

        private int KeyNotSimilarity(string originKey, string crackKey)
        {
            int notSuccess = 0;

            for (int i = 0; i < originKey.Length; i++)
            {
                if (crackKey[i] != '?' && crackKey[i] != originKey[i])
                    notSuccess++;
            }

            return notSuccess;
        }
    }
}
