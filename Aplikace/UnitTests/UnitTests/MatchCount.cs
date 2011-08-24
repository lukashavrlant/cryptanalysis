using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CryptanalysisCore;
using ExtensionMethods;
using System.Threading;
using System.IO;

namespace UnitTests
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
        /// Obsahuje pole všech textů, které můžeme používat pro testování
        /// </summary>
        private static string[] texts;

        /// <summary>
        /// Akce, která se spustí po dokončení testu
        /// </summary>
        /// <param name="errors">Počet chybně dešifrovaných textů</param>
        /// <param name="count">Celkový počet šifrovaných textů</param>
        public delegate void RunComplete(int errors, int count);


        static MatchCount()
        {
            coresInComputer = Environment.ProcessorCount;

            texts = File.ReadAllText(Storage.StatsFolderPath + Storage.TextsFile + ".czech.txt")
                .ToLower()
                .RemoveDiacritics()
                .Split('\n')
                .Select(x => x.Filter(y => TextAnalysis.IsEnglishLetter(y) || y == ' '))
                .Shuffle()
                .ToArray();
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
        public static void Test(Cipher cipher, int crackMehod, int count, RunComplete runComplete, System.Windows.Forms.ProgressBar progressBar)
        {
            threads = coresInComputer;
            Thread thread;

            if(progressBar != null)
                progressBar.Maximum = count;

            errors = 0;

            coresInComputer.Times(i =>
                {
                    thread = new Thread(() => DoTest(cipher, crackMehod, count, i, runComplete, progressBar));
                    thread.IsBackground = true;
                    thread.Priority = 0;
                    thread.Start();
                });
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
        public static void Test(Cipher cipher, int crackMehod, int count, RunComplete runComplete)
        {
            Test(cipher, crackMehod, count, runComplete, null);
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
        private static void DoTest(Cipher cipher, int crackMehod, int count, int start, RunComplete runComplete, System.Windows.Forms.ProgressBar progressBar)
        {
            int errorsCounter = 0;

            string[] testStrings = texts.Where(x => x.Length >= 500)
                .Select(x => x.ToCharArray().Take(500).ToList().Implode(""))
                .Where(x => x.Length > 0)
                .Take(count)
                .ToArray();

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

                if (progressBar != null)
                {
                    Action incrementProgressBar = () => progressBar.Value++;
                    if (progressBar.InvokeRequired)
                        progressBar.Invoke(incrementProgressBar);
                    else
                        incrementProgressBar();
                }
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

                    if (progressBar != null)
                    {
                        Action zeroProgressBar = () => progressBar.Value = 0;
                        if (progressBar.InvokeRequired)
                            progressBar.Invoke(zeroProgressBar);
                        else
                            zeroProgressBar();
                    }
                }
            }
        }

        private static string[] GetTestStrings(int length, int count)
        {
            return texts.Where(x => x.Length >= length)
                .Select(x => x.ToCharArray().Take(length).ToList().Implode(""))
                .Where(x => x.Length > 0)
                .Take(count)
                .ToArray();
        }

        /*public static int WithSpaces(Action showDone, Action<string> setTitle)
        {
            Monoalphabetic cipher = new Monoalphabetic();

            int count = 500;
            string[] testTexts = GetTestStrings(1000, count);
            count = testTexts.Length;
            int[] successes = new int[count];
            //var combs = Combinatorics.VariationsWithRepetition(new List<double>() { 0, 1, 2, 3 }, 3);
            List<List<double>> combs = new List<List<double>>();
            combs.Add(new List<double> { 2,0,3 });
            combs.Add(new List<double> { 1,3,2 });
            var resultSuccess = new Dictionary<string, int>();


            List<List<double>>[] splitedCombs = new List<List<double>>[Environment.ProcessorCount];
            splitedCombs[0] = combs.Take(combs.Count / 2).ToList();
            combs.Reverse();
            splitedCombs[1] = combs.Take(combs.Count / 2).ToList();

            Thread[] threads = new Thread[Environment.ProcessorCount];

            for (int i = 0; i < Environment.ProcessorCount; i++)
            {
                List<List<double>> combinations = splitedCombs[i];
                threads[i] = new Thread(() =>
                {
                    int counter = 0;
                    foreach (var comb in combinations)
                    {
     

                        count.Times(x => 
                        {
                            successes[x] = cipher.UnitTextFrequency(testTexts[x], Storage.Languages.czech, comb.ToArray());
                            counter++;
                            setTitle(string.Format("{0}", counter.ToString()));
                        });

                        int success = (int)Math.Round((double)successes.Sum() / (double)count);
                        resultSuccess[comb.Implode("")] = success;
                        SaveResults(resultSuccess);
                    }

                    if (resultSuccess.Count == combs.Count)
                    {
                        showDone();
                    }
                });
            }

            threads.ForEach(x => x.Start());

            return 0;
        }*/

        private static void SaveResults(Dictionary<string, int> results)
        {
            results = results.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            StringBuilder sb = new StringBuilder();
            results.ForEach(x => sb.AppendFormat("Variace: {0}, uspech: {1} %\n", x.Key, x.Value));
            try
            {
                File.WriteAllText(Storage.StatsFolderPath + "combResult.txt", sb.ToString());
            }
            catch (Exception)
            { }
            
        }

        public static int Bigrams()
        {
            int match = 0;
            int count = 500;
            string[] testTexts = GetTestStrings(1000, count);
            count = testTexts.Length;

            foreach (string text in testTexts)
            {
                var bigrams = TextAnalysis.GetOccurrence(text, 2).Where(x => !x.Key.Contains(" ")).Take(15).Select(x => x.Key).ToArray();
                var sym = TextAnalysis.GetSymetricBigrams(bigrams);

                if (sym.Count > 0)
                {
                    if (sym[0][0] == "ne" || sym[0][1] == "en")
                        match++;
                }
            }

            return match;
        }

        public static int WithSpacesClassic()
        {
            /*Monoalphabetic cipher = new Monoalphabetic();

            int count = 500;
            string[] testTexts = GetTestStrings(1000, count);
            count = testTexts.Length;
            int[] successes = new int[count];

            //count.Times(x => successes[x] = cipher.UnitTextFrequency(testTexts[x], Storage.Languages.czech, new double[] { 3, 1, 3, 3, 3 }));
            int success = (int)Math.Round((double)successes.Sum() / (double)count);

            return success;*/
            return 0;
        }
    }
}
