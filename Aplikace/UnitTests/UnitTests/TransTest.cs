using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTests
{
    class TransTest : CipherTest
    {
        public TransTest(Action progress, Action<string> afterFinish, Texts texts)
            :base(progress, afterFinish, texts)
        {
            cipher = new CryptanalysisCore.Transposition();
        }
    }
}
