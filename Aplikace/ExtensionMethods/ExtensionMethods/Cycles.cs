using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtensionMethods
{
    public static class Cycles
    {
        public static void From0ToI(int to, Action<int> action)
        {
            for (int i = 0; i < to; i++)
            {
                action(i);
            }
        }
    }
}
