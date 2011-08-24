using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryptanalysisCore.Exceptions
{
    public class MatchNotFound : ApplicationException
    {
        public MatchNotFound(string Message)
            : base(Message)
        { }

        public MatchNotFound()
            : this("Shoda nebyla nalazena")
        { }
    }
}
