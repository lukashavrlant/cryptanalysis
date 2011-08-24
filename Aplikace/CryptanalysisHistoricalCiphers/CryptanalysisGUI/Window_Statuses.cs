using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CryptanalysisGUI
{
    public partial class Window : Form
    {
        private void encryptTextBox_MouseHover(object sender, EventArgs e)
        {
            Status = "Sem vložte text, který chcete prolomit";
        }

        private void autoChooseCipher_MouseHover(object sender, EventArgs e)
        {
            Status = "Zatrhněte, pokud chcete nechat aplikaci, aby sama zvolila šifru, kterou byl text zašifrován";
        }

        private void ResetStatus()
        {
            Status = string.Empty;
        }

        private void encryptTextBox_MouseLeave(object sender, EventArgs e)
        {
            ResetStatus();
        }

        private void autoChooseCipher_MouseLeave(object sender, EventArgs e)
        {
            ResetStatus();
        }

        private void languages_MouseHover(object sender, EventArgs e)
        {
            Status = "Jazyk, kterým byl původně šifrový text zašifrován";
        }

        private void languages_MouseLeave(object sender, EventArgs e)
        {
            ResetStatus();
        }

        private void crackCiphers_MouseHover(object sender, EventArgs e)
        {
            Status = "Šifra, kterou byl šifrový text zašifrovaný";
        }

        private void crackCiphers_MouseLeave(object sender, EventArgs e)
        {
            ResetStatus();
        }

        private void crackAlgs_MouseHover(object sender, EventArgs e)
        {
            Status = "Různé algoritmy na prolomení textu. Výchozí je ten nejúspěšnější.";
        }

        private void crackAlgs_MouseLeave(object sender, EventArgs e)
        {
            ResetStatus();
        }

        private void crackButton_MouseHover(object sender, EventArgs e)
        {
            Status = "Pokusí se prolomit šifru a získat původní otevřený text";
        }

        private void crackButton_MouseLeave(object sender, EventArgs e)
        {
            ResetStatus();
        }

        private void crackTextBox_MouseHover(object sender, EventArgs e)
        {
            Status = "Původní otevřený text";
        }

        private void crackTextBox_MouseLeave(object sender, EventArgs e)
        {
            ResetStatus();
        }

        private void textBox1_MouseHover(object sender, EventArgs e)
        {
            Status = "Klíč, pomocí kterého byl text zašifrován";
        }

        private void textBox1_MouseLeave(object sender, EventArgs e)
        {
            ResetStatus();
        }

        private void cipherResultBox_MouseHover(object sender, EventArgs e)
        {
            Status = "Šifra, kterou byl text zašifrován";
        }

        private void cipherResultBox_MouseLeave(object sender, EventArgs e)
        {
            ResetStatus();
        }

        private void randomKey_MouseHover(object sender, EventArgs e)
        {
            Status = "Vygeneruje náhodný a pro danou šifru validní klíč";
        }

        private void randomKey_MouseLeave(object sender, EventArgs e)
        {
            ResetStatus();
        }

        private void keepSpacesBox_MouseHover(object sender, EventArgs e)
        {
            Status = "Zatrhněte, pokud chcete v šifrovém textu ponechat mezery mezi slovy.";
        }

        private void keepSpacesBox_MouseLeave(object sender, EventArgs e)
        {
            ResetStatus();
        }

        private void inputTextBox_MouseHover(object sender, EventArgs e)
        {
            Status = "Sem vložte text, který chcete zašifrovat nebo dešifrovat";
        }

        private void inputTextBox_MouseLeave(object sender, EventArgs e)
        {
            ResetStatus();
        }

        private void encryptButton_MouseHover(object sender, EventArgs e)
        {
            Status = "Zašifruje vložený text podle zvolené šifry a zvoleného klíče";
        }

        private void encryptButton_MouseLeave(object sender, EventArgs e)
        {
            ResetStatus();
        }

        private void decryptButton_MouseHover(object sender, EventArgs e)
        {
            Status = "Dešifruje vložený text podle zvolené šifry a zvoleného klíče";
        }

        private void decryptButton_MouseLeave(object sender, EventArgs e)
        {
            ResetStatus();
        }

        private void keyTextBox_Leave(object sender, EventArgs e)
        {
            statusLock = false;
            ResetStatus();
        }
    }
}
