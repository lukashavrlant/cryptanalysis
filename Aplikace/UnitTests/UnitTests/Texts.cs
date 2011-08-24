using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CryptanalysisCore;
using ExtensionMethods;
using System.IO;

namespace UnitTests
{
    public class Texts
    {
        public string Text
        { get; private set; }

        public string[] Words
        { get; private set; }

        private Random random;


        public Texts(string language, string filename)
        {
            string path = Storage.StatsFolderPath + Storage.TextsFolder + language + ".txt";
            Text = File.ReadAllText(path);
            random = new Random();
        }

        public Texts(string language)
            : this(language, "texts.txt")
        { }

        public string RandomParagraph(int length)
        {
            int startIndex = random.Next(0, Text.Length - length - 1);
            return Analyse.NormalizeText(Text.Substring(startIndex, length), Analyse.TextTypes.WithSpacesLower);
        }

        public string RandomSpacesParagraph(int length)
        {
            int startIndex = random.Next(0, Text.Length - length - 1);
            return Analyse.NormalizeText(Text.Substring(startIndex, length), Analyse.TextTypes.WithSpacesLower);
        }

        public string[] RandomParagraphes(int paragraphLength, int paragraphCount)
        {
            string[] paragraphes = new string[paragraphCount];

            for (int i = 0; i < paragraphCount; i++)
                paragraphes[i] = RandomParagraph(paragraphLength);

            return paragraphes;
        }

        /*public string[] RandomWords(int wordCount)
        {
            Random random = new Random();
            int startIndex = random.Next(0, Words.Length - wordCount - 1);
            string[] paragraph = Words.Take(startIndex, wordCount);
            return paragraph;
        }

        public string[][] RandomWords(int paragraphCount, int wordCount)
        {
            string[][] paragraphes = new string[paragraphCount][];

            for (int i = 0; i < paragraphCount; i++)
            {
                paragraphes[i] = RandomWords(wordCount);
            }

            return paragraphes;
        }*/
    }
}
