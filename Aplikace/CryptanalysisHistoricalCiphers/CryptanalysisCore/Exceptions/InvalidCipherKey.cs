using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryptanalysisCore.Exceptions
{
    public class InvalidCipherKey : CryptanalysisException
    {
        public InvalidCipherKey(string message)
            : base(message)
        { }
    }
}
