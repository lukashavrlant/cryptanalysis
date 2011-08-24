using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryptanalysisCore
{
    interface ICipher
    {
        /// <summary>
        /// Zašifruje zadaný text.
        /// </summary>
        /// <param name="OpenText">Text, který má být šifrován</param>
        /// <param name="Key">Klíč, pomocí kterého má být text zašifrován</param>
        /// <returns>Zašifrovaný text</returns>
        CipherPacket Encrypt(CipherPacket packet);

        /// <summary>
        /// Dešifruje zadaný text.
        /// </summary>
        /// <param name="CipherText">Text, který má být dešifrován</param>
        /// <param name="Key">Klíč, pomocí kterého bude prováděno dešifrování</param>
        /// <returns>Otevřený text</returns>
        CipherPacket Decrypt(CipherPacket packet);

        /// <summary>
        /// Násilné dešifrování zašifrovaného textu bez znalosti klíče.
        /// </summary>
        /// <param name="CipherText">Text, který se pokoušíme dešifrovat</param>
        /// <returns>Otevřený text</returns>
        CipherPacket Crack(CipherPacket packet, LangCharacteristic langChar);

        /// <summary>
        /// Vrátí náhodný klíč k dané šifře
        /// TODO: Kontrola toho, aby klíč nebyl slabý
        /// </summary>
        /// <returns>Náhodný šifrovací klíč</returns>
        string RandomKey();
    }
}
