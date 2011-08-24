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
        private void AddThread(Thread thread)
        {
            lock (ThreadAddLock)
            {
                RunningThreads = RunningThreads.Where(x => x != null && x.IsAlive).ToList();
                RunningThreads.Add(thread);
            }
        }

        private void DeleteThreads()
        {
            lock (ThreadAddLock)
            {
                foreach (Thread thread in RunningThreads)
                {
                    if (thread != null && thread.IsAlive)
                    {
                        thread.Abort();
                    }
                }
            }
        }

        private void AddTimer(System.Timers.Timer timer)
        {
            lock (TimersAddLock)
            {
                RunningTimers = RunningTimers.Where(t => t != null).ToList();
                RunningTimers.Add(timer);
            }
        }

        private void DeleteTimers()
        {
            lock (TimersAddLock)
            {
                foreach (var timer in RunningTimers)
                {
                    if (timer != null)
                    {
                        timer.Stop();
                        timer.Close();
                        timer.Dispose();
                    }
                }
            }
        }

        private void ShowWarningIfDirectiesNotFound()
        {
            CallWithInvoke(() =>
            {
                var res = MessageBox.Show("Nepodařilo se načíst soubory s informace o jazycích, některé algoritmy nebudou funkční.", 
                    "Ale toto je nepříjemné", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning);
                CallWithInvoke(() => Status = "Nepodařilo se načíst některé soubory. Aplikace pravděpodobně nebude fungovat správně.");
            });
        }

        private void HandCrack(string ciphertext)
        {
            Storage.Languages language = Language;
            Cipher cipher = CurrentCrackCipher;
            int attackType = AttackType;
            progressBar.Style = ProgressBarStyle.Marquee;

            AddThread(Parallel.InBackground(() =>
            {
                try
                {
                    var res = cipher.Crack(ciphertext, attackType, language);
                    CallWithInvoke(() =>
                    {
                        SetCrackResult(res.First(), cipher.Decrypt(ciphertext, res.First()), string.Empty);
                    });
                }
                catch (CryptanalysisCore.Exceptions.MatchNotFound)
                {
                    CallWithInvoke(() =>
                    {
                        SetCrackResult();
                    });
                }
                catch (Exception)
                {
                    CallWithInvoke(() =>
                        {
                            SetCrackResult();
                            ShowErrorBox();
                        });
                }
            }));
        }

        private void AutomaticCrack(string ciphertext)
        {
            Storage.Languages language = Language;
            Cipher[] testCiphers = ciphers.Values.ToArray();
            SetProgressBar(testCiphers.Length);
            Cryptanalyse cryptanalyse = new Cryptanalyse();
            cryptanalyse.ProgressFunction = UpdateProgressBar;
            cryptanalyse.FinishFunction = x => CallWithInvoke(() => SetCrackResult(x, ciphertext));
            cryptanalyse.AddThreadFunction = AddThread;
            AddThread(Parallel.InBackground(() => cryptanalyse.Attack(ciphertext, testCiphers, language)));
        }

        private void SetCrackResult(CrackResult result, string ciphertext)
        {
            SetCrackResult(result.key, result.cipher.Decrypt(ciphertext, result.key), Storage.GetCipherName((Storage.Ciphers)Enum.Parse(typeof(Storage.Ciphers), result.cipher.ToString())));
        }

        private void SetCrackResult(string key, string opentext, string cipher)
        {
            lock (AttackRunningLock)
            {
                //AttackRunning = false;
                ResetTimer(shuffleTimer);

                CallWithInvoke(() =>
                {
                    //CrackButtonEnabled = true;
                    crackKeyBox.Text = key;
                    crackTextBox.Text = opentext;
                    cipherResultBox.Text = cipher;
                    //StopButtonEnabled = false;
                    SetAttackStatus(false);
                });
            }

            ResetProgressBar();
        }

        private void SetCrackResult()
        {
            SetCrackResult("???", "Text se nepodařilo dešifrovat", string.Empty);
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

        private void TestKeyValid()
        {
            if (CurrentCipher.IsKeyValid(Key))
            {
                keyTextBox.BackColor = Color.LightGreen;
            }
            else
            {
                keyTextBox.BackColor = Color.OrangeRed;
            }
        }

        private void saveTextToFile(string text)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            save.FilterIndex = 0;
            save.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var res = save.ShowDialog();

            if (res == DialogResult.OK)
            {
                string path = save.FileName;

                try
                {
                    File.WriteAllText(path, text);
                }
                catch (Exception)
                {
                    MessageBox.Show("Text se nepodařilo uložit.", "Jejda!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void ResetProgressBar()
        {
            CallWithInvoke(() =>
            {
                progressBar.Style = ProgressBarStyle.Blocks;
                progressBar.Value = 0;
            });
        }

        private void ResetProgressBar(int miliseconds)
        {
            progressBarTimer = new System.Timers.Timer();
            progressBarTimer.Interval = miliseconds;
            progressBarTimer.Elapsed += new System.Timers.ElapsedEventHandler((a, b) =>
            {
                ResetProgressBar();
                progressBarTimer.Close();
            });
            progressBarTimer.Start();
        }

        private void SetProgressBar(int maximum)
        {
            ResetTimer(progressBarTimer);

            CallWithInvoke(() =>
            {
                progressBar.AccessibleName = "";
                progressBar.Value = 0;
                progressBar.Maximum = maximum;
            });
        }

        private void ResetTimer(System.Timers.Timer timer)
        {
            if (timer != null)
            {
                timer.Stop();
                timer.Close();
                timer.Dispose();
            }
        }

        private void UpdateProgressBar()
        {
            CallWithInvoke(() => progressBar.Value++);
        }

        private void SetCrackSettingsVisibility()
        {
            bool visibility = autoChooseCipher.Checked;
            crackAlgsSettings.Visible = !visibility;
            cipherResultLabel.Visible = visibility;
            cipherResultBox.Visible = visibility;
        }

        /// <summary>
        /// Probíhá při kryptoanalýze
        /// </summary>
        /// <param name="ciphertext"></param>
        private void ShuffleOpentext(string ciphertext)
        {
            int count = 50;
            string cutCiphertext = ciphertext.Take(MaximumShuffleLetters);
            string[] shuffledTexts = TextAnalysis.GetShuffledText(cutCiphertext, count);

            AddThread(Parallel.InBackground(() =>
                {
                    shuffleTimer = new System.Timers.Timer();
                    shuffleTimer.Interval = 50;
                    Random random = new Random();

                    shuffleTimer.Elapsed += new System.Timers.ElapsedEventHandler((a, b) =>
                    {
                        CallWithInvoke(() =>
                        {
                            lock (AttackRunningLock)
                            {
                                if (AttackRunning)
                                {
                                    crackTextBox.Text = shuffledTexts[random.Next(count)];
                                    crackKeyBox.Text = ciphers[Storage.Ciphers.vigenere].RandomKey();
                                    cipherResultBox.Text = Storage.CiphersNames[random.Next(Storage.CiphersNames.Length - 1)].ToString();
                                }
                            }
                        });
                    });

                    shuffleTimer.Start();
                }, ThreadPriority.Normal));
        }

        /// <summary>
        /// Probíhá při šifrování a dešifrování
        /// </summary>
        /// <param name="text"></param>
        private void ShuffleTextbox(string text)
        {
            string shuffletext = text.Take(2500);

            AddThread(Parallel.InBackground(() =>
                {
                    int counter = 5;

                    System.Timers.Timer timer = new System.Timers.Timer();
                    timer.Interval = 20;
                    timer.Elapsed += new System.Timers.ElapsedEventHandler((a, b) =>
                    {
                        if (counter != 0)
                        {
                            CallWithInvoke(() => OutputText = shuffletext.Shuffle());
                            counter--;
                        }
                        else
                        {
                            CallWithInvoke(() =>
                                {
                                    OutputText = text;
                                    EncryptRunning = false;
                                    SetEncryptButtonEnabled();
                                });
                            timer.Stop();
                            timer = null;
                        }
                    });
                    timer.Start();
                    AddTimer(timer);
                }));
        }

        private void SetEncryptButtonEnabled()
        {
            bool enabled = !EncryptRunning && CurrentCipher.IsKeyValid(Key) && !CrackTabActive;

            encryptButton.Enabled = enabled;
            decryptButton.Enabled = enabled;
            encryptMenuItem.Enabled = enabled;
            decryptMenuItem.Enabled = enabled;
        }

        private void StopCryptanalyse()
        {
            SafeCall(() =>
                {
                    DeleteThreads();
                    DeleteTimers();
                    CallWithInvoke(() =>
                    {
                        lock (ThreadAddLock)
                        {
                            Status = "Kryptoanalýza přerušena";
                            progressBar.Style = ProgressBarStyle.Blocks;
                            progressBar.Value = 0;
                        }
                    });

                    SetAttackStatus(false);
                });
        }

        private void SwitchMenuItems()
        {
            crackTabMenuItems.ForEach(x => x.Enabled = CrackTabActive);
            encryptTabMenuItems.ForEach(x => x.Enabled = !CrackTabActive);

            if (CrackTabActive)
            {
                CryptTabButtonVisible();
            }
        }

        private void StartCryptanalyse()
        {
            if (CheckLangLoaded(Language))
            {
                SafeCall(() =>
                    {
                        SetAttackStatus(true);
                        string ciphertext = Ciphertext.Take(MaximumCrackLetters);

                        if (ciphertext.Length > 0)
                        {
                            ShuffleOpentext(ciphertext);

                            if (AutomaticCrackCipher)
                            {
                                AutomaticCrack(ciphertext);
                            }
                            else
                            {
                                HandCrack(ciphertext);
                            }
                        }
                        else
                        {
                            SetAttackStatus(false);
                        }
                    });
            }
            else
            {
                ShowNotLangFound();
            }
        }

        private void SetAttackStatus(bool status)
        {
            CrackButtonEnabled = !status;
            AttackRunning = status;
            StopButtonEnabled = status;
        }

        private void CryptTabButtonVisible()
        {
            CrackButtonEnabled = !AttackRunning;
            StopButtonEnabled = AttackRunning;
        }

        private bool CheckLangLoaded(Storage.Languages language)
        {
            try
            {
                Storage.GetLangChar(language);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void SetRandomKey()
        {
            SafeCall(() => Key = CurrentCipher.RandomKey());
        }

        private void ShowAboutDialog()
        {
            SafeCall(() =>
                {
                    var about = new AboutBox1();
                    about.ShowDialog();
                });
        }

        private void ShowErrorBox()
        {
            MessageBox.Show("Nastala nespecifikovaná chyba aplikace, zkuste prosím operaci znova s jinými parametry",
                "Ale toto je nepříjemné", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
        }

        private void ShowNotLangFound()
        {
            MessageBox.Show("Jazykový balíček nutný ke kryptoanaýze neby nalezen. Zkuste jiný jazyk nebo přidejte jazykové balíčky.",
                "Ale toto je nepříjemné", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
        }

        private bool SafeCall(Action action)
        {
            try
            {
                action();
                return true;
            }
            catch (Exception)
            {
                ShowErrorBox();
                return false;
            }
        }

        private void ProgressBlick()
        {
            SetProgressBar(1);
            UpdateProgressBar();
            ResetProgressBar(1000);
        }

        private void ToggleSpaceButton()
        {
            if (CurrentCipher is Transposition)
            {
                lastKeepSpacesChecked = keepSpacesBox.Checked;
                keepSpacesBox.Enabled = false;
                keepSpacesBox.Checked = false;
            }
            else
            {
                keepSpacesBox.Enabled = true;
                keepSpacesBox.Checked = lastKeepSpacesChecked;
            }
        }
    }
}
