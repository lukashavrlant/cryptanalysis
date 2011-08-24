using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CryptanalysisCore;
using System.Threading;
//using Microsoft.WindowsAPICodePack.Taskbar;

namespace UnitTests
{
    public partial class Form1 : Form
    {
        private MonoTest mono;
        private CaesarTest caesar;
        private VigenereTest vigenere;
        private TransTest trans;

        private Texts texts;

        public static Storage.Languages currentLanguage;

        public Form1()
        {
            InitializeComponent();
            languageSelect.SelectedIndex = 0;
            Storage.LoadFiles();

            InBackground(() =>
                {
                    currentLanguage = Storage.Languages.czech;
                    string lang = currentLanguage.ToString();
                    texts = new Texts(lang);
                    //textsNotStat = new Texts(lang, "texts_not.txt");
                    mono = new MonoTest(UpdateProgressBar, AfterDone, texts);
                    caesar = new CaesarTest(UpdateProgressBar, AfterDone, texts);
                    vigenere = new VigenereTest(UpdateProgressBar, AfterDone, texts);
                    trans = new TransTest(UpdateProgressBar, AfterDone, texts);
                    SetTitle(Text + ": Soubory načteny");
                });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MatchCount.Test(new Caesar(), 0, 2000, (errors, count) => MessageBox.Show(string.Format("Errors: {0}, count: {1}.", errors.ToString(), count.ToString())), progressBar1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //InBackground(() => MatchCount.WithSpaces(ShowDone, SetTitle));
        }

        private void SetTitle(string title)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => SetTitle(title)));
            }
            else
            {
                Text = title;
            }
        }

        private void ShowDone()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => MessageBox.Show("Hotovo!")));
            }
            else
            {
                MessageBox.Show("Hotovo!");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show(MatchCount.Bigrams().ToString());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show(MatchCount.WithSpacesClassic().ToString());
        }

        public static void InBackground(Action action)
        {
            Thread thread = new Thread(() => action());
            thread.IsBackground = true;
            thread.Priority = ThreadPriority.BelowNormal;
            thread.Start();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(Unique.TestUnique(5000, 500).ToString());
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            int paragraphCount = int.Parse(parCount.Text.Trim());
            int lettCount = int.Parse(letterCount.Text.Trim());
            progressBar1.Maximum = paragraphCount;
            progressBar1.Value = 0;
            mono.Attack(paragraphCount, lettCount, 0, MonoKeysTest);
            //caesar.Attack(paragraphCount, lettCount, attackType, (orig, crack) => orig == crack);
        }

        private void AfterDone(string result)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => AfterDone(result)));
            }
            else
            {
                MessageBox.Show(string.Format("{0}", result), "Výsledek", MessageBoxButtons.OK, MessageBoxIcon.Information);
                progressBar1.Value = 0;
                //TaskbarManager.Instance.SetProgressValue(0, 1000000);
            }
        }

        private void UpdateProgressBar()
        {
            if (progressBar1.InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => UpdateProgressBar()));
            }
            else
            {
                progressBar1.Value++;
                Text = string.Format("{0}/{1}", progressBar1.Value, progressBar1.Maximum);
                //TaskbarManager.Instance.SetProgressValue(progressBar1.Value, progressBar1.Maximum);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            CaesarAttack(Caesar.BruteForceID);
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            CaesarAttack(Caesar.TriangleID);
        }

        private void CaesarAttack(int attackType)
        {
            int paragraphCount = int.Parse(parCount.Text.Trim());
            int lettCount = int.Parse(letterCount.Text.Trim());
            progressBar1.Maximum = paragraphCount;
            progressBar1.Value = 0;
            caesar.Attack(paragraphCount, lettCount, attackType, (orig, crack) => orig == crack);
        }

        private void Form1_Load(object sender, EventArgs e)
        { }

        private void button1_Click_2(object sender, EventArgs e)
        {
            int paragraphCount = int.Parse(parCount.Text.Trim());
            int lettCount = int.Parse(letterCount.Text.Trim());
            progressBar1.Maximum = paragraphCount;
            progressBar1.Value = 0;
            vigenere.Attack(paragraphCount, lettCount, Vigenere.KeyLengthAttack, VigenereKeysTest);
        }

        private bool VigenereKeysTest(string originKey, string crackKey)
        {
            return GetNumberErrors(originKey, crackKey) <= 1;
        }

        private bool MonoKeysTest(string origin, string crackKey)
        {
            return GetNumberErrors(origin, crackKey) <= 6;
        }

        private int GetNumberErrors(string originKey, string crackKey)
        {
            int errors = 0;

            for (int i = 0; i < originKey.Length && i < crackKey.Length; i++)
            {
                if (originKey[i] != crackKey[i])
                    errors++;
            }

            errors += Math.Abs(originKey.Length - crackKey.Length);

            return errors;
        }

        private void button3_Click_2(object sender, EventArgs e)
        {
            int paragraphCount = int.Parse(parCount.Text.Trim());
            int lettCount = int.Parse(letterCount.Text.Trim());
            progressBar1.Maximum = paragraphCount;
            progressBar1.Value = 0;
            vigenere.Attack(paragraphCount, lettCount, Vigenere.BruteForce, VigenereKeysTest);
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            int paragraphCount = int.Parse(parCount.Text.Trim());
            int lettCount = int.Parse(letterCount.Text.Trim());
            int take = int.Parse(takeTextbox.Text.Trim());
            progressBar1.Maximum = paragraphCount;
            progressBar1.Value = 0;
            vigenere.TestKeysLength(paragraphCount, lettCount, take);
        }

        private void button4_Click_2(object sender, EventArgs e)
        {
            int paragraphCount = int.Parse(parCount.Text.Trim());
            int lettCount = int.Parse(letterCount.Text.Trim());
            progressBar1.Maximum = paragraphCount;
            progressBar1.Value = 0;
            vigenere.Attack(paragraphCount, lettCount, 0, (orig, crack) => orig == crack);
        }
    }
}
