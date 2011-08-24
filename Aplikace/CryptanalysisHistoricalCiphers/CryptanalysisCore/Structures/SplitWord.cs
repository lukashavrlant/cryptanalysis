using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryptanalysisCore.Structures
{
    struct SplitWord
    {
        public Dictionary<char, int[]> startLetters;
        public Dictionary<char, int[]> lastLetters;

        public int lengthStart;
        public int lengthLast;

        public int totalLength
        {
            get
            {
                return lengthLast + lengthStart;
            }
        }
    }
}
