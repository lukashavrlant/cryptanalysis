using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtensionMethods;

namespace CryptanalysisCore
{
    public delegate List<string> CrackMethod(string ciphertext, Storage.Languages language);

    abstract public class Cipher
    {
        protected string ExceptionText;

        /// <summary>
        /// Vytvoříme novou šifru
        /// </summary>
        /// <param name="language">Jazyk, nad kterým bude šifra pracovat</param>
        public Cipher()
        {
            SetCrackMethods();
        }

        protected abstract bool IsKeyValid(string key);

        /// <summary>
        /// Zašifruje zadaný text podle předaného klíče.
        /// </summary>
        /// <param name="opentext">Text, který bude šifrován</param>
        /// <param name="key">Šifrový klíč</param>
        /// <returns>Zašifrovaný text</returns>
        public virtual string Encrypt(string opentext, string key)
        {
            if (!IsKeyValid(key))
                throw new Exceptions.InvalidCipherKey(ExceptionText);

            if (!TextAnalysis.IsOpentextValid(opentext))
                throw new Exceptions.InvalidOpentext();

            return opentext;
        }

        /// <summary>
        /// Dešifruje zadaný text
        /// </summary>
        /// <param name="ciphertext">Zašifrovaný text</param>
        /// <param name="key">Šifrovací klíč</param>
        /// <returns>Otevřený text</returns>
        public virtual string Decrypt(string ciphertext, string key)
        {
            if (!IsKeyValid(key))
                throw new Exceptions.InvalidCipherKey(ExceptionText);

            return ciphertext;
        }

        /// <summary>
        /// Provede útok na zašifrovaný text s pouhou znalostí jednoho zašifrovaného textu
        /// </summary>
        /// <param name="ciphertext">Zašifrovaný text</param>
        /// <returns>Dešifrovaný text</returns>
        //protected abstract CipherPacket CiphertextAttack(CipherPacket packet);

        /// <summary>
        /// Provede útok na zašifrovaný text se znalostí jak zašifrovaného,
        /// tak otevřeného textu.
        /// </summary>
        /// <param name="ciphertext">Zašifrovaný text</param>
        /// <returns>Dešifrovaný text</returns>
        //protected abstract List<string> KnownPlaintext(string opentext, string ciphertext);

        /// <summary>
        /// Vrátí náhodný klíč k dané šifře
        /// TODO: Kontrola toho, aby klíč nebyl slabý
        /// </summary>
        /// <returns>Náhodný šifrovací klíč</returns>
        public abstract string RandomKey();

        /// <summary>
        /// Seznam method určených k prolamování šifer
        /// </summary>
        protected CrackMethod[] CrackMethods;

        /// <summary>
        /// Nastaví metody pro cracknutí 
        /// </summary>
        protected abstract void SetCrackMethods();

        /// <summary>
        /// Násilné dešifrování zašifrovaného textu bez znalosti klíče.
        /// </summary>
        /// <param name="CipherText">Text, který se pokoušíme dešifrovat</param>
        /// <returns>Otevřený text</returns>
        public List<string> Crack(string ciphertext, int attackType, Storage.Languages language)
        {
            return CrackMethods[attackType](ciphertext, language);
        }

        public override string ToString()
        {
            return "šifra";
        }
    }
}
