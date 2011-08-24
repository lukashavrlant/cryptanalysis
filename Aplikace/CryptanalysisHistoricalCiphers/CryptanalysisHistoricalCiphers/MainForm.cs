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

namespace CryptanalysisHistoricalCiphers
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// Obsahuje odkaz na právě vybranou šifru v šifrovací části
        /// </summary>
        private Cipher CurrentEncryptCipher
        {
            get { return Ciphers[Storage.CiphersID[cipherEncrypt.SelectedItem.ToString()]]; }
        }

        private Cipher CurrentAnalysisCipher
        {
            get { return Ciphers[Storage.CiphersID[cipherCryptanalysis.SelectedItem.ToString()]]; }
        }

        /// <summary>
        /// Vlastnost uchovávající klíč v textovém poli
        /// </summary>
        private string Key
        {
            get { return keyTextbox.Text; }
            set { keyTextbox.Text = value; }
        }

        /// <summary>
        /// Vlatnost uchovávající vstupní text
        /// </summary>
        private string InputText
        {
            get { return inputTextbox.Text; }
            set { inputTextbox.Text = value; }
        }

        /// <summary>
        /// Vlastnost uchovávající výstupní text
        /// </summary>
        private string OutputText
        {
            get { return outputTextbox.Text; }
            set { outputTextbox.Text = value; }
        }

        /// <summary>
        /// Aktuálně nastavený jazyk šifrového textu
        /// </summary>
        private Storage.Languages Language
        {
            get { return Storage.LanguagesID[languages.SelectedItem.ToString()]; }
        }

        /// <summary>
        /// Klíš získaný při pokusu o cracknutí šifry
        /// </summary>
        private string CrackKeyResult
        {
            get { return crackKeyResultTextbox.Text; }
            set { crackKeyResultTextbox.Text = value; }
        }

        /// <summary>
        /// Obsahuje instance všech šifer
        /// </summary>
        private Dictionary<Storage.Ciphers, Cipher> Ciphers;

        public MainForm()
        {
            InitializeComponent();
            InitializeUserComponents();
        }

        /// <summary>
        /// Inicializuje uživatelsky definované prvky.
        /// </summary>
        private void InitializeUserComponents()
        {
            // Nastavíme všechny combo boxy
            SetComboBoxes();

            // Vytvoříme instance všech šifer
            InitializeCiphers();

            // DEBUG
            cipherCryptanalysis.SelectedIndex = 3;
            crackAlgorithms.SelectedIndex = 0;
            cipherEncrypt.SelectedIndex = 3;
        }

        /// <summary>
        /// Vytvoří instance všech šifer.
        /// </summary>
        private void InitializeCiphers()
        {
            Ciphers = new Dictionary<Storage.Ciphers, Cipher>();
            Ciphers[Storage.Ciphers.caesar] = new Caesar();
            Ciphers[Storage.Ciphers.monoalphabetic] = new Monoalphabetic();
            Ciphers[Storage.Ciphers.vigenere] = new Vigenere();
            Ciphers[Storage.Ciphers.trans] = new Transposition();
        }

        /// <summary>
        /// Nastaví všechny combo boxy
        /// </summary>
        private void SetComboBoxes()
        {
            SetComboBoxes(cipherCryptanalysis, Storage.CiphersNames);
            SetComboBoxes(cipherEncrypt, Storage.CiphersNames);
            SetComboBoxes(languages, Storage.LanguagesNames);
            SetComboBoxes(crackAlgorithms, Storage.CrackAlgorithms[Storage.Ciphers.caesar]);
        }

        /// <summary>
        /// Nastaví combo boxu klasický styl a předané položky
        /// </summary>
        /// <param name="comboBox">Combo box</param>
        /// <param name="items">Seznam položek, které se mají objevit v Combo boxu</param>
        private void SetComboBoxes(ComboBox comboBox, string[] items)
        {
            comboBox.Items.Clear();
            comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox.Items.AddRange(items);
            comboBox.SelectedIndex = 0;
        }

        /// <summary>
        /// Tlačítko aktivující šifrování. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void encryptButton_Click(object sender, EventArgs e)
        {
            Encrypt();
        }

        /// <summary>
        /// Zašifruje text v inputu a zobrazí jej v outputu.
        /// </summary>
        private MainForm Encrypt()
        {
            string opentext;

            if (keepSpacesButton.Checked)
                opentext = Analyse.NormalizeText(InputText, Analyse.TextTypes.WithSpacesLower);
            else
                opentext = Analyse.NormalizeText(InputText, Analyse.TextTypes.WithoutSpacesLower);
            
            try
            {
                OutputText = CurrentEncryptCipher.Encrypt(opentext, Key);
            }
            catch (CryptanalysisCore.Exceptions.CryptanalysisException ex)
            {
                MessageBox.Show(ex.Message, "Šifrování se nezdařilo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return this;
        }

        /// <summary>
        /// Dešifruje text ze vstupního pole podle klíče
        /// </summary>
        /// <returns></returns>
        private MainForm Decrypt()
        {
            return Decrypt(InputText, Key, CurrentEncryptCipher);
        }

        private MainForm Decrypt(string ciphertext, string key, Cipher decryptCipher)
        {
            ciphertext = Analyse.NormalizeText(ciphertext, Analyse.TextTypes.WithoutSpacesLower);

            try
            {
                OutputText = decryptCipher.Decrypt(ciphertext, key);
            }
            catch (CryptanalysisCore.Exceptions.CryptanalysisException ex)
            {
                MessageBox.Show(ex.Message, "Dešifrování se nezdařilo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            } 

            return this;
        }

        /// <summary>
        /// tlačítko spustí akcí generování náhodného klíče
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void generateRandomKeyButton_Click(object sender, EventArgs e)
        {
            GenerateRandomKey();
        }

        /// <summary>
        /// Vygeneruje náhodný klíč a uloží ho do textového pole pro klíč
        /// </summary>
        private MainForm GenerateRandomKey()
        {
            Key = CurrentEncryptCipher.RandomKey();
            return this;
        }

        /// <summary>
        /// Tlačítko, které dešifruje text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void decryptButton_Click(object sender, EventArgs e)
        {
            Decrypt();
        }

        /// <summary>
        /// Pokusí se prolomit šifrový text
        /// </summary>
        /// <returns></returns>
        private MainForm Crack()
        {
            try
            {
                string ciphertext = Analyse.NormalizeText(InputText, Analyse.TextTypes.WithSpacesLower);
                List<string> keys =  CurrentAnalysisCipher.Crack(ciphertext, crackAlgorithms.SelectedIndex, Language);
                Decrypt(ciphertext, keys[0], CurrentAnalysisCipher);
                CrackKeyResult = keys[0];
            }
            catch (CryptanalysisCore.Exceptions.MatchNotFound)
            {
                MessageBox.Show("Nebyl nalezen žádný klíč, text nebylo možné prolomit.", "Kryptoanalýza neúspěšná", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            return this;
        }

        private void crackCipher_Click(object sender, EventArgs e)
        {
            Crack();
        }

        /// <summary>
        /// Načítání souborů z disku. V případě neúspěchu oznámíme uživateli, 
        /// že nastal problém a umožníme mu ukončit aplikaci.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            // Načteme z disku informace o jazycích
            Parallel.InBackground(() =>
            {
                try
                {
                    Storage.LoadFiles(CommonMethods.NothingAction);
                }
                catch (Exception)
                {
                    ShowWarningIfFilesNotFound();
                }
                
                CallWithInvoke(() => crackCipher.Enabled = true);
            });

            setTextLength();
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

        private void ShowWarningIfFilesNotFound()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => ShowWarningIfFilesNotFound()));
            }
            else
            {
                var res = MessageBox.Show("Nepodařilo se načíst některé soubory. Aplikace pravděpodobně nebude fungovat správně. Zavřít aplikaci?", "Aplikace selhala", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if (res == DialogResult.Yes)
                    Close();
            }
        }

        /// <summary>
        /// Při každé změně šifrovacího algoritmu při kryptoanalýze aktualizujeme
        /// seznam dostupných algoritmů.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cipherCryptanalysis_SelectedIndexChanged(object sender, EventArgs e)
        {
            Storage.Ciphers currentCipher = Storage.CiphersID[cipherCryptanalysis.SelectedItem.ToString()];
            crackAlgorithms.Items.Clear();
            crackAlgorithms.Items.AddRange(Storage.CrackAlgorithms[currentCipher]);
            crackAlgorithms.SelectedIndex = 0;
        }

        private void inputTextbox_TextChanged(object sender, EventArgs e)
        {
            setTextLength();
        }

        private void setTextLength()
        {
            letterCountLabel.Text = "Počet znaků: " + inputTextbox.Text.Length.ToString();
            //setTextLengthColor();
        }

        /*private void setTextLengthColor()
        {
            Color color = Color.Black;
            int length = inputTextbox.Text.Length;

            //Dictionary<Cipher 

            if (length == 0)
                color = Color.Black;
            else if (length < 750)
                color = Color.Red;
            else if (length < 1500)
                color = Color.Orange;
            else if (length >= 1500)
                color = Color.Green;

            letterCountLabel.ForeColor = color;
        }*/

        private void addSpacesButton_Click(object sender, EventArgs e)
        {
            OutputText = Spaces.Add(OutputText, Storage.GetLangChar(Language).SortedDictionary); 
        }

        private void pasteTextButton_Click(object sender, EventArgs e)
        {
            InputText = Clipboard.GetText();
        }

        private void copyTextButton_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(OutputText);
        }

        private void copyUpButton_Click(object sender, EventArgs e)
        {
            InputText = OutputText;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int polygonSize = 5;
            int complementSize = 2 * polygonSize;
            //TextAnalysis.PolygonAttack(Analyse.NormalizeText(InputText, Analyse.TextTypes.WithoutSpacesLower), Storage.GetLangChar(Language).Letters.OrderByDescending(x => x.Value).Take(polygonSize).Select(x => x.Key[0]).ToArray(), complementSize);
        }
    }
}
