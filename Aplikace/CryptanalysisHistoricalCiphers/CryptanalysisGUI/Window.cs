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
        public Window()
        {
            InitializeComponent();
        }

        private void AltGUI_Load(object sender, EventArgs e)
        {
            crackTabMenuItems = new ToolStripMenuItem[] { crackTextMenuItem, stopMenuItem };
            encryptTabMenuItems = new ToolStripMenuItem[] { encryptMenuItem, decryptMenuItem, randomKeyMenuItem };
            SwitchMenuItems();
            CrackButtonEnabled = false;
            StopButtonEnabled = false;
            InitializeCiphers();
            SetCrackSettingsVisibility();
            SetComboBoxes();
            Status = "Načítám slovníky...";
            statusLock = true;
            string actionName = "loading dict files";
            

            var thread = Parallel.InBackground(() =>
            {
                SetProgressBar(Enum.GetNames(typeof(Storage.Languages)).Length);
                CallWithInvoke(() => progressBar.AccessibleName = actionName);

                try
                {
                    Storage.LoadFiles(() => CallWithInvoke(() =>
                            {
                                if (progressBar.AccessibleName == actionName)
                                    progressBar.Value += 1;
                            }));
                }
                catch (Exception)
                {
                    ShowWarningIfDirectiesNotFound();
                }

                CallWithInvoke(() =>
                {
                    CrackButtonEnabled = true;
                    statusLock = false;
                    progressBar.AccessibleName = "";
                    Status = "Načtení slovníků bylo úspěšné";
                    ResetProgressBar();
                });
                
            });

            AddThread(thread);
        }

        

        /// <summary>
        /// Nastaví všechny combo boxy
        /// </summary>
        private void SetComboBoxes()
        {
            SetComboBoxes(crackCiphers, Storage.CiphersNames);
            SetComboBoxes(languages, Storage.LanguagesNames);
            SetComboBoxes(ciphersEncrypt, Storage.CiphersNames);
            SetComboBoxes(attackTypeCombo, Storage.CrackAlgorithms[Storage.Ciphers.caesar]);
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

        private void InitializeCiphers()
        {
            ciphers = new Dictionary<Storage.Ciphers, Cipher>();
            ciphers[Storage.Ciphers.caesar] = new Caesar();
            ciphers[Storage.Ciphers.monoalphabetic] = new Monoalphabetic();
            ciphers[Storage.Ciphers.trans] = new Transposition();
            ciphers[Storage.Ciphers.vigenere] = new Vigenere();
        }

        private void crackButton_Click(object sender, EventArgs e)
        {
            StartCryptanalyse();
        }
        

        private void autoChooseCipher_CheckedChanged(object sender, EventArgs e)
        {
            SetCrackSettingsVisibility();
        }

        private void keyTextBox_Enter(object sender, EventArgs e)
        {
            SetKeyHelp();
            statusLock = true;
        }

        private void SetKeyHelp()
        {
            Status = Storage.keyHelp[CurrentCipherType];
        }

        private void ciphersEncrypt_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToggleSpaceButton();
            SetKeyHelp();

            if (Key.Length > 0)
            {
                TestKeyValid();
                SetEncryptButtonEnabled();
            }
        }

        private void randomKey_Click(object sender, EventArgs e)
        {
            SetRandomKey();
        }

        private void ProcessText(Func<string, string, string> function)
        {
            ProgressBlick();

            var thread = Parallel.InBackground(() =>
                {
                    CallWithInvoke(() =>
                            {
                                try
                                {

                                    EncryptRunning = true;
                                    SetEncryptButtonEnabled();
                                    string result = function(InputText, Key);
                                    ShuffleTextbox(result);

                                }
                                catch (CryptanalysisCore.Exceptions.InvalidCipherKey ex)
                                {
                                    CallWithInvoke(() => MessageBox.Show(ex.Message, "Šifrový klíč není validní", MessageBoxButtons.OK, MessageBoxIcon.Warning));
                                    EncryptRunning = false;
                                    SetEncryptButtonEnabled();
                                }
                                catch (Exception)
                                {
                                    ShowErrorBox();
                                    EncryptRunning = false;
                                    SetEncryptButtonEnabled();
                                }
                            });
                    
                });
            AddThread(thread);
        }

        private void encryptButton_Click(object sender, EventArgs e)
        {
            ProcessText(CurrentCipher.Encrypt);
        }

        private void keyTextBox_TextChanged(object sender, EventArgs e)
        {
            TestKeyValid();
            SetEncryptButtonEnabled();
        }        

        private void decryptButton_Click(object sender, EventArgs e)
        {
            ProcessText(CurrentCipher.Decrypt);
        }

        private void copyToClipboardButton_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(OutputText);
        }

        private void saveTextToFile_Click(object sender, EventArgs e)
        {
            saveTextToFile(OutputText);
        }

        

        private void outputTextBox_MouseHover(object sender, EventArgs e)
        {
            Status = "Text po zašifrování či dešifrování";
        }

        private void outputTextBox_MouseLeave(object sender, EventArgs e)
        {
            ResetStatus();
        }

        private void AltGUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            ResetTimer(progressBarTimer);
            ResetTimer(shuffleTimer);
            DeleteThreads();
            DeleteTimers();
        }

        

        private void crackCiphers_SelectedIndexChanged(object sender, EventArgs e)
        {
            attackTypeCombo.Items.Clear();
            attackTypeCombo.Items.AddRange(Storage.CrackAlgorithms[CurrentCrackCipherType]);
            attackTypeCombo.SelectedIndex = 0;
        }   

        private void encryptTextBox_TextChanged(object sender, EventArgs e)
        {
            textLengthBox.Text = String.Format("Počet znaků: {0}", encryptTextBox.Text.Length);
        }

        private void kopírovatVšeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string text = IsCryptAnalyseTabActive ? CrackText : OutputText;

            if(text != string.Empty)
                Clipboard.SetText(text);
        }

        private void kopírovatVšeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string text = IsCryptAnalyseTabActive ? encryptTextBox.Text : inputTextbox.Text;

            if(text != string.Empty)
                Clipboard.SetText(text);
        }

        private void smazatVšeAVložitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsCryptAnalyseTabActive)
                Ciphertext = Clipboard.GetText();
            else
                InputText = Clipboard.GetText();
        }

        private void smazatVšeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string defaultValue = string.Empty;

            if (IsCryptAnalyseTabActive)
                Ciphertext = defaultValue;
            else
                InputText = defaultValue;
        }

        private void vložitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string value = Clipboard.GetText();
            if (IsCryptAnalyseTabActive)
                Ciphertext += value;
            else
                InputText += value;
        }

        private void inputTextBox_TextChanged(object sender, EventArgs e)
        {
            textLengthEncryptTextBox.Text = string.Format("Počet znaků: {0}", inputTextbox.Text.Length);
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            StopCryptanalyse();
        }

        private void ukončitAplikaciToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void tabControl_selectedIndexChanged(object sender, EventArgs e)
        {
            SwitchMenuItems();
            SetEncryptButtonEnabled();
        }

        private void stopMenuItem_Click(object sender, EventArgs e)
        {
            StopCryptanalyse();
        }

        private void crackTextMenuItem_Click(object sender, EventArgs e)
        {
            StartCryptanalyse();
        }

        private void encryptMenuItem_Click(object sender, EventArgs e)
        {
            ProcessText(CurrentCipher.Encrypt);
        }

        private void decryptMenuItem_Click(object sender, EventArgs e)
        {
            ProcessText(CurrentCipher.Decrypt);
        }

        private void randomKeyMenuItem_Click(object sender, EventArgs e)
        {
            SetRandomKey();
        }

        private void oAplikaciToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowAboutDialog();
        }

        private void jakPoužívatAplikaciToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string helpPath = Storage.StatsFolderPath + "../Help/napoveda.chm";

                if (File.Exists(helpPath))
                {
                    Help.ShowHelp(inputTextbox, helpPath, HelpNavigator.TableOfContents);
                }
                else
                {
                    MessageBox.Show("Soubor s nápovědou nebyl nalezen nebo je poškozen.", "Ale toto je nepříjemné", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Soubor s nápovědou nebyl nalezen nebo je poškozen.", "Ale toto je nepříjemné", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void selectAll_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((System.Windows.Forms.Control.ModifierKeys == Keys.Control) && (e.KeyChar == (char)1))
            {
                ((TextBox)sender).SelectAll();
            }
        }

        private void TextBox_MouseDown(object sender, MouseEventArgs e)
        {
            /*TextBox txt = (TextBox)sender;
            txt.DoDragDrop(txt.SelectedText, DragDropEffects.Copy);*/
        }

        private void TextBox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void TextBox_DragDrop(object sender, DragEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            txt.Text = (string)e.Data.GetData(DataFormats.Text);
        }
    }
}
