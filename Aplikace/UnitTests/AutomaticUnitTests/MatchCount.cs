using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CryptanalysisCore;
using ExtensionMethods;
using System.Threading;
using System.IO;

namespace AutomaticUnitTests
{
    class MatchCount
    {
        /// <summary>
        /// Počet nesprávně dešifrovaných textů.
        /// </summary>
        private static int errors;

        /// <summary>
        /// Počet běžících vláken.
        /// </summary>
        private static int threads;

        /// <summary>
        /// Počet jader v počítači -- na základě tohoto počtu 
        /// se rozhodneme, kolik vláken spustíme.
        /// </summary>
        private static int coresInComputer;

        /// <summary>
        /// Zámek použitý při inkrementaci výsledného počtu chyb.
        /// </summary>
        private static object incrementResultsLock = new object();

        /// <summary>
        /// Akce, která se spustí po dokončení testu
        /// </summary>
        /// <param name="errors">Počet chybně dešifrovaných textů</param>
        /// <param name="count">Celkový počet šifrovaných textů</param>
        public delegate void RunComplete(int errors, int count);


        static MatchCount()
        {
            coresInComputer = Environment.ProcessorCount;
        }

        /// <summary>
        /// Metoda vyzkouší dešifrovat několik textů a po skončení zavolá 
        /// předanou metodu runComplete s argumentem počet nalezených neshod.
        /// Metoda funguje paralelně, testovací kód je spouštěn v tolika
        /// vláknech, kolik má počítač jader. 
        /// </summary>
        /// <param name="cipher">Šifra, nad kterou budeme Unit test provádět</param>
        /// <param name="crackMehod">Identifikátor útočící metody.</param>
        /// <param name="count">Počet textů, které chceme zkoušet.</param>
        /// <param name="runComplete">Jaká akce má být provedena po dokončení testu.</param>
        /// <param name="progressBar">Do kterého progressBaru chceme zobrazovat průběh</param>
        public static void Test(Cipher cipher, int crackMehod, int count, RunComplete runComplete)
        {
            threads = coresInComputer;
            Thread thread;

            errors = 0;

            coresInComputer.Times(i =>
                {
                    thread = new Thread(() => DoTest(cipher, crackMehod, count, i, runComplete));
                    thread.IsBackground = true;
                    thread.Priority = 0;
                    thread.Start();
                });
        }

        /// <summary>
        /// Provádí samotný test dešifrování. 
        /// </summary>
        /// <param name="cipher">Šifra, nad kterou budeme Unit test provádět</param>
        /// <param name="crackMehod">Identifikátor útočící metody.</param>
        /// <param name="count">Počet textů, které chceme zkoušet.</param>
        /// <param name="start">Index prvního testovaného řetězce</param>
        /// <param name="runComplete">Jaká akce má být provedena po dokončení testu.</param>
        /// <param name="progressBar">Do kterého progressBaru chceme zobrazovat průběh</param>
        private static void DoTest(Cipher cipher, int crackMehod, int count, int start, RunComplete runComplete)
        {
            int errorsCounter = 0;

            string[] testStrings = File.ReadAllText(Storage.StatsFolderPath + Storage.TextsFile + ".czech.txt")
                .ToLower()
                .Split('\n')
                .Select(x => x.Filter(y => TextAnalysis.IsEnglishLetter(y)))
                .Where(x => x.Length >= 500)
                .Select(x => x.ToCharArray().Take(500).ToList().Implode(""))
                .Where(x => x.Length > 0)
                .Take(count)
                .ToArray();

            //File.WriteAllText("delky.txt", testStrings.ToList().Select(x => x.Length).ToList().Implode("\n"));

            List<string> keys;
            string opentext = string.Empty;
            string ciphertext;
            for (int i = start; i < testStrings.Length; i += coresInComputer)
            {
                ciphertext = cipher.Encrypt(testStrings[i], cipher.RandomKey());

                try
                {
                    keys = cipher.Crack(ciphertext, crackMehod, Storage.Languages.czech);
                    opentext = cipher.Decrypt(ciphertext, keys[0]);
                }
                catch (CryptanalysisCore.Exceptions.MatchNotFound)
                {
                    errorsCounter++;
                }

                if (opentext != testStrings[i])
                    errorsCounter++;
            }

            /*
             * Atomicky přičteme počet zjištěných neshod, snížíme počet běžících vláken
             * o jedničku a následně zjistíme, jestli je toto vlákno poslední běžící vlákno.
             */
            lock (incrementResultsLock)
            {
                errors += errorsCounter;
                threads--;

                if (threads == 0)
                {
                    runComplete(errors, count);
                }
            }
        }
    }
}
