using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryptanalysisCore
{
    public class CipherPacket
    {
        public CipherPacket() { }

        public CipherPacket(string Key)
        {
            this.Key = Key;
        }

        public CipherPacket(string Key, string Opentext)
            :this(Key)
        {
            this.Opentext = Opentext;
        }

        public CipherPacket(string Key, string Opentext, string Ciphertext)
            :this(Key, Opentext)
        {
            this.Ciphertext = Ciphertext;
        }


        /// <summary>
        /// Otevřený text. Buď vstupuje do algoritmů pro zašifrování 
        /// nebo vystupuje z algoritmů jako dešifrovaný text.
        /// </summary>
        public string Opentext { get; set; }

        /// <summary>
        /// Zašifrovaný text. Buď vystupuje z algoritmů pro zašifrování
        /// nebo vstupuje do algoritmů jako šifrovaný text.
        /// </summary>
        public string Ciphertext { get; set; }

        /// <summary>
        /// Klíč použitý při šifrování
        /// </summary>
        public string Key { get; set; }
    }
}
