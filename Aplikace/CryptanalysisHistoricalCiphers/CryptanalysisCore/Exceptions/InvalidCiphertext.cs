using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryptanalysisCore.Exceptions
{
    class InvalidCiphertext : CryptanalysisException
    {
        public InvalidCiphertext()
            : base("Šifrový text není validní.")
        { }
    }
}
