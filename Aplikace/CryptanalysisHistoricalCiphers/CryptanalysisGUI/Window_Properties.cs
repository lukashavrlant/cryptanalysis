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
using System.IO;
using System.Threading;

namespace CryptanalysisGUI
{
    public partial class Window : Form
    {
        private const int MaximumCrackLetters = 11111;

        private const int MaximumShuffleLetters = 2500;

        private ToolStripMenuItem[] crackTabMenuItems;
        private ToolStripMenuItem[] encryptTabMenuItems;

        /// <summary>
        /// Obsahuje časovač pro resetování progress baru
        /// </summary>
        private System.Timers.Timer progressBarTimer;

        /// <summary>
        /// Obsahuje časovač pro míchání textu
        /// </summary>
        private System.Timers.Timer shuffleTimer;

        /// <summary>
        /// Obsahuje šifrový text, který chce uživatel dešifrovat
        /// </summary>
        private string Ciphertext
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
        /// Obsahuje text ve status baru
        /// </summary>
        private string Status
        {
            get { return statusBar.Text; }
            set
            {
                if (!statusLock)
                    statusBar.Text = value;
            }
        }

        /// <summary>
        /// Indikuje, zda se může měnit status
        /// </summary>
        private bool statusLock = false;

        /// <summary>
        /// Obsahuje klíč použitý při šifrování/dešifrování
        /// </summary>
        private string Key
        {
            get { return keyTextBox.Text.ToLower(); }
            set { keyTextBox.Text = value; }
        }

        /// <summary>
        /// Obsahuje klíč získaná po kryptoanalýze
        /// </summary>
        private string CrackKey
        {
            get { return crackKeyBox.Text; }
            set { crackKeyBox.Text = value; }
        }

        /// <summary>
        /// Obsahuje informaci, zda má program automaticky rozpoznat šifru
        /// </summary>
        private bool AutomaticCrackCipher
        {
            get { return autoChooseCipher.Checked; }
        }

        /// <summary>
        /// Seznam všech podporovaných šifer
        /// </summary>
        private Dictionary<Storage.Ciphers, Cipher> ciphers;

        /// <summary>
        /// Obsahuje referenci na aktuálně vybranou šifru v šifrovacím tabu
        /// </summary>
        private Cipher CurrentCipher
        {
            get { return ciphers[CurrentCipherType]; }
        }

        /// <summary>
        /// Obsahuje typ aktuálně vybrané šifry
        /// </summary>
        private Storage.Ciphers CurrentCipherType
        {
            get { return Storage.CiphersID[ciphersEncrypt.SelectedItem.ToString()]; }
        }

        /// <summary>
        /// Obsahuje typ aktuálně vybrané šifry v kryptoanalýze
        /// </summary>
        private Storage.Ciphers CurrentCrackCipherType
        {
            get { return Storage.CiphersID[crackCiphers.SelectedItem.ToString()]; }
        }

        // <summary>
        /// Obsahuje referenci na aktuálně vybranou šifru v šifrovacím tabu
        /// </summary>
        private Cipher CurrentCrackCipher
        {
            get { return ciphers[CurrentCrackCipherType]; }
        }

        /// <summary>
        /// Obsahuje výslednou šifru při automatické kryptoanalýze
        /// </summary>
        private string CrackCipherResult
        {
            get { return cipherResultBox.Text; }
            set { cipherResultBox.Text = value; }
        }

        /// <summary>
        /// Obsahuje identifikátor kryptoanalytického algopritmu
        /// </summary>
        private int AttackType
        {
            get { return attackTypeCombo.SelectedIndex; }
        }

        /// <summary>
        /// Obsahuje normalizovaný vstupní text v šifrovacím tabu
        /// </summary>
        private string InputText
        {
            get
            {
                Analyse.TextTypes type = keepSpacesBox.Checked ? Analyse.TextTypes.WithSpacesLower : Analyse.TextTypes.WithoutSpacesLower;
                return Analyse.NormalizeText(inputTextbox.Text, type);
            }
            set
            {
                inputTextbox.Text = value;
            }
        }

        /// <summary>
        /// Obsahuje výstupní text v šifrovacím tabu
        /// </summary>
        private string OutputText
        {
            get { return outputTextBox.Text; }
            set { outputTextBox.Text = value; }
        }

        /// <summary>
        /// Obsahuje jazyk šifrového textu
        /// </summary>
        private Storage.Languages Language
        {
            get { return Storage.LanguagesID[languages.SelectedItem.ToString()]; }
        }

        /// <summary>
        /// Indikuje, zda je aktivní tab s kryptoanalýzou
        /// </summary>
        private bool IsCryptAnalyseTabActive
        {
            get { return tabControl1.SelectedIndex == 0; }
        }

        /// <summary>
        /// Nastavuje, zda má být tlačítko spuštějící útok aktivní
        /// </summary>
        private bool CrackButtonEnabled
        {
            set
            {
                crackButton.Enabled = value;
                crackTextMenuItem.Enabled = value && CrackTabActive;
            }
        }

        private bool StopButtonEnabled
        {
            set
            {
                stopButton.Enabled = value;
                stopMenuItem.Enabled = value && CrackTabActive;
            }
        }

        private bool CrackTabActive
        {
            get { return tabControl1.SelectedIndex == 0; }
        }

        /// <summary>
        /// Indikuje, zda právě probíhá útok
        /// </summary>
        private bool AttackRunning;

        /// <summary>
        /// Synchronizační zámek pro shuffletext a crackattack
        /// </summary>
        private object AttackRunningLock = new object();

        /// <summary>
        /// Synchronizační zámek pro přidávání a odebírání vláken 
        /// ze seznamu běžících vláken
        /// </summary>
        private object ThreadAddLock = new object();

        /// <summary>
        /// Obsahuje všechny běžící procesy.
        /// Slouží k tomu, abychom je mohli při zavření formuláře uzavřít.
        /// </summary>
        private List<Thread> RunningThreads = new List<Thread>();

        /// <summary>
        /// Obsahuje všechny běžící timery
        /// </summary>
        private List<System.Timers.Timer> RunningTimers = new List<System.Timers.Timer>();

        /// <summary>
        /// Synchronizační zámek pro přidávání a odebírání timerů 
        /// </summary>
        private object TimersAddLock = new object();

        /// <summary>
        /// Nastavuje, zda mají být tlačítka na encrypt tabu zapnutá nebo vypnutá
        /// </summary>
        private bool EncryptRunning;

        /// <summary>
        /// Jaká byla hodnota checkboxu na ponechání mezer před vypnutím?
        /// </summary>
        private bool lastKeepSpacesChecked;
    }
}
