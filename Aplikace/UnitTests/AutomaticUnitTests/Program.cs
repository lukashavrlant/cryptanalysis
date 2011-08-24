using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CryptanalysisCore;
using System.Threading;

namespace AutomaticUnitTests
{
    class Program
    {
        static string Result;

        static void Main(string[] args)
        {
            int numbers = 50;

            try
            {
                numbers = Int32.Parse(args[0]);
            }
            catch { }
            
            Storage.LoadFiles();
            Dictionary<Cipher, int[]> ciphers = new Dictionary<Cipher, int[]>();
            ciphers[new Caesar()] = new int[] { 0, 1 };

            Result = string.Empty;
            try
            {
                DateTime date = DateTime.Now;
                Result = File.ReadAllText(Storage.StatsFolderPath + "log.txt");
                Result += "\n\n\n------------------------------------\n";
                Result += date.ToString();
                Result += "\n------------------------------------\n";
            }
            catch (Exception)
            { }

            foreach(var cipher in ciphers)
            {
                foreach (int attackType in cipher.Value)
                {
                    test(cipher.Key, attackType, numbers);
                }
            }

            Result += "\n------------------------------------\n"; 
            File.WriteAllText(Storage.StatsFolderPath + "log.txt", Result);
        }

        static void test(Cipher cipher, int attackType, int numbers)
        {
            bool wait = true;

            MatchCount.Test(cipher, attackType, numbers, (errors, count) =>
            {
                Result += string.Format("\n{2}: {3}\t\tErrors: {0}, count: {1}.", errors.ToString(), count.ToString(), cipher.ToString(), attackType.ToString());
                wait = false;
            });

            while (wait)
            {
                Thread.Sleep(1000);
            }
        }
    }
}
