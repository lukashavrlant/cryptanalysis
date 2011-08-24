using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryptanalysisCore.Exceptions
{
    public class InvalidOpentext : CryptanalysisException
    {
        public InvalidOpentext()
            : base("Vstupní text není validní.")
        { }
    }
}
