using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryptanalysisCore.Exceptions
{
    public class CryptanalysisException : ApplicationException
    {
        public CryptanalysisException(string Message)
            : base(Message)
        { }
    }
}
