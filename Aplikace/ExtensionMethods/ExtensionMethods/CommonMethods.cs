using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtensionMethods
{
    public class CommonMethods
    {
        public static void NothingAction()
        { }

        public static R Identity<R>(R item)
        {
            return item;
        }

        public static bool True(Object item)
        {
            return true;
        }

        public static bool False(Object item)
        {
            return false;
        }

        public static Func<A, bool> Complement<A>(Func<A, bool> predicate)
        {
            return (i) => !predicate(i);
        }
    }
}
