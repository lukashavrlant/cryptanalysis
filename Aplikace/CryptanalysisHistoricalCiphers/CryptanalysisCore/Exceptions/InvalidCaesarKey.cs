using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryptanalysisCore.Exceptions
{
    public class InvalidCaesarKey : InvalidCipherKey
    {
        public InvalidCaesarKey(string Message)
            : base(Message)
        { }

        public InvalidCaesarKey()
            : this("Klíčem musí být právě jedno malé písmeno anglické abecedy.")
        { }
    }
}
