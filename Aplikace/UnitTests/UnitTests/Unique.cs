using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTests
{
    public static class Unique
    {
        private static Texts texts = new Texts();

        public static int TestUnique(int parCount, int wordCount)
        {
            int success = 0;
            for (int i = 0; i < parCount; i++)
            {
                var paragraph = texts.RandomParagraph(wordCount);
                foreach (string word in paragraph)
                {
                    if (IsUniqueWord(word))
                    {
                        success++;
                        //break;
                    }
                }
            }

            return success;
        }

        private static bool IsUniqueWord(string testWord)
        {
            return texts.UniqueWords.Where(x => x == testWord).Count() != 0;
        }
    }
}
