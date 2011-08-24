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
    class CipherTest
    {
        public delegate bool KeyPredicate(string originKey, string crackKey);

        protected Action progress;
        protected Action<string> afterFinish;
        protected Cipher cipher;
        protected int[] success;
        protected int[] notSuccess;
        protected int[] done;
        protected Texts texts;
        protected object lockObject;

        public CipherTest(Action progress, Action<string> afterFinish, Texts texts)
        {
            this.progress = progress;
            this.afterFinish = afterFinish;
            this.texts = texts;
            lockObject = new object();
        }

        protected void InitializeArrays(int length)
        {
            success = new int[length].Fill(0);
            notSuccess = new int[length].Fill(0);
            done = new int[Environment.ProcessorCount].Fill(0);
        }

        protected bool AreThreadsDone()
        {
            return done.All(x => x == 1);
        }

        protected void SetDone(int turn)
        {
            done[turn] = 1;
        }

        public void Attack(int paragraphCount, int letterCount, int attackType, KeyPredicate keyPred)
        {
            InitializeArrays(paragraphCount);
            int part = paragraphCount / Environment.ProcessorCount;

            Environment.ProcessorCount.Times(i =>
            {
                Thread thread = new Thread(() => Attack(part, letterCount, attackType, i, keyPred));
                thread.Priority = ThreadPriority.BelowNormal;
                thread.Start();
            });
        }

        protected void Attack(int paragraphCount, int letterCount, int attackType, int turn, KeyPredicate keyPred)
        {
            for (int i = 0; i < paragraphCount; i++)
            {
                string opentext;

                if(cipher is Monoalphabetic)
                    opentext = texts.RandomSpacesParagraph(letterCount);
                else
                    opentext = texts.RandomParagraph(letterCount);

                string key = cipher.RandomKey();
                string ciphertext = cipher.Encrypt(opentext, key);
                try
                {
                    var crackKeys = cipher.Crack(ciphertext, attackType, Form1.currentLanguage);
                    success[paragraphCount * turn + i] = keyPred(key, crackKeys.First()) ? 1 : 0;

                }
                catch (CryptanalysisCore.Exceptions.MatchNotFound)
                {

                }

                progress();
            }

            SetDone(turn);

            lock (lockObject)
            {
                if (AreThreadsDone())
                {
                    afterFinish(string.Format("Úspěšnost: {0} %.",
                        (int)Math.Round(((double)success.Sum() / (double)success.Length * 100))));
                }
            }
        }
    }
}
