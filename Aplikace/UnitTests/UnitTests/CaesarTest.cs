using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CryptanalysisCore;
using ExtensionMethods;
using System.Threading;

namespace UnitTests
{
    class CaesarTest : CipherTest
    {
        public CaesarTest(Action progress, Action<string> afterFinish, Texts texts)
            :base(progress, afterFinish, texts)
        {
            cipher = new Caesar();
        }
    }
}
