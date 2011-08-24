using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtensionMethods
{
    public static class DoubleExt
    {
        /// <summary>
        /// Zjistí, zda se zadané číslo nachází v intervalu [min, max] (uzavřený interval).
        /// </summary>
        /// <param name="d">Libovolné číslo</param>
        /// <param name="min">Dolní hranice intervalu</param>
        /// <param name="max">Horní hranice intervalu</param>
        /// <returns>True pokud se nachází v intervalu, false jinak</returns>
        public static bool InInterval(this double d, double min, double max)
        {
            return d >= min && d <= max;
        }
    }
}
