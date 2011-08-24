using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CryptanalysisCore;
using ExtensionMethods;

namespace CryptanalysisGUI
{
    public partial class AltGUI : Form
    {
        /// <summary>
        /// Obsahuje šifrový text, který chce uživatel dešifrovat
        /// </summary>
        private string EncryptText
        {
            get { return Analyse.NormalizeText(encryptTextBox.Text, Analyse.TextTypes.WithSpacesLower); }
            set { encryptTextBox.Text = value; }
        }

        /// <summary>
        /// Obsahuje prolomený text
        /// </summary>
        private string CrackText
        {
            get { return crackTextBox.Text; }
            set { crackTextBox.Text = value; }
        }

        /// <summary>
        /// Seznam všech podporovaných šifer
        /// </summary>
        private Cipher[] ciphers;

        public AltGUI()
        {
            InitializeComponent();
        }

        private void AltGUI_Load(object sender, EventArgs e)
        {
            Parallel.InBackground(() =>
            {
                Storage.LoadFiles(() => { });
                CallWithInvoke(() => crackButton.Enabled = true);
            });

            InitializeCiphers();
        }

        private void InitializeCiphers()
        {
            ciphers = new Cipher[] { new Caesar(), new Monoalphabetic(), new Vigenere() };
        }

        private void crackButton_Click(object sender, EventArgs e)
        {
            Universal.Attack(EncryptText, ciphers, Storage.Languages.czech);
        }

        private void CallWithInvoke(Action action)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => action()));
            }
            else
            {
                action();
            }
        }
    }
}
