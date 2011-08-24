using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryptanalysisCore
{
    class SplitedNgrams
    {
        public readonly Dictionary<int, int> limits; 

        public Dictionary<int, string[]> Top { get; private set; }
        public Dictionary<int, string[]> Bottom { get; private set; }

        public SplitedNgrams(Dictionary<int, string[]> ngrams)
        {
            limits = new Dictionary<int, int>()
            {
                {Analyse.OneGram, 6},
                {Analyse.Bigram, 20},
                {Analyse.Trigram, 50}
            };

            Top = new Dictionary<int, string[]>();
            Bottom = new Dictionary<int, string[]>();

            foreach (var pair in limits)
                Top[pair.Key] = ngrams[pair.Key].Take(pair.Value).ToArray();

            foreach (var pair in limits)
                Bottom[pair.Key] = ngrams[pair.Key].Reverse().Take(pair.Value * 20).ToArray();
        }
    }
}
