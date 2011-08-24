using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryptanalysisCore
{
    public static class Maths
    {
        public static int[] AllDivisors(int n)
        {
            List<int> divisors = new List<int>() { n };
            int limit = (int)Math.Sqrt(n);

            for (int i = 2; i <= limit; i++)
            {
                if (n % i == 0)
                {
                    divisors.Add(i);
                    divisors.Add((int)((double)n / (double)i));
                }
            }
            return divisors.Distinct().ToArray();
        }
    }
}
