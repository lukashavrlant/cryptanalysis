using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryptanalysisCore
{
    public class NearbyLetters
    {
        public Dictionary<string, double> PreviousLetters
        { get; private set; }

        public Dictionary<string, double> NextLetters
        { get; private set; }

        public NearbyLetters(Dictionary<string, double> previousLetters, Dictionary<string, double> nextLetters)
        {
            PreviousLetters = previousLetters;
            NextLetters = nextLetters;
        }
    }
}
