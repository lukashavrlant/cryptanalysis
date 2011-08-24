using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Statistics
{
    public partial class Form1 : Form
    {
        private TextDownloader textDownloader;

        public Form1()
        {
            InitializeComponent();
            textDownloader = new TextDownloader(SetTitle, ShowDone);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Frequency.SaveData("english");
            ShowDone();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //textDownloader.Idnes(SetTitle);
            ShowDone();
        }

        private void SetTitle(string text)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => SetTitle(text)));
            }
            else
            {
                Text = text;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string[] textFiles = new string[] {
                "capek_hordubal.txt",
                "capek_krakatit.txt",
                "capek_povidky_z_druhe_kapsy.txt",
                "capek_povidky_z_jedne_kapsy.txt",
                "capek_tovarna_na_absolutno.txt",
                "capek_valka_s_mloky.txt",
                "novinky_texty.txt",
                "blogynovinky_texty.txt",
                "sedmy_smysl.txt",
                "svehlavicka.txt",
                "zeme_podvedena.txt",
                "vitalia_texty.txt",
                "lupa_texty.txt",
                "respekt_texty.txt"
            };

            TextDownloader.MergeFiles("texts.czech.txt", textFiles);
            ShowDone();
        }



        private void ShowDone()
        {
            MessageBox.Show("Done");
        }

        private void ShowDone(string text)
        {
            MessageBox.Show(text, "Hotovo",  MessageBoxButtons.OK);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            TextDownloader.RepairIdnes();
            ShowDone();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textDownloader.Novinky();
            ShowDone();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textDownloader.BlogyNovinky();
            ShowDone();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textDownloader.Vitalia();
            ShowDone();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            textDownloader.Lupa();
            ShowDone();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            textDownloader.Respekt();
            ShowDone();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            UniqueWords.Save("czech");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            textDownloader.BlogyAktualne();
            ShowDone();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            string[] textFiles = new string[] {
                "blogyaktualne_texty.txt"
            };

            TextDownloader.MergeFiles("texts_not.czech.txt", textFiles);
            ShowDone();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            textDownloader.TimeMagazine();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Frequency.SaveData("english");
            ShowDone();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            string[] textFiles = new string[] {
                "timemagazine_texty.txt"
                
            };

            TextDownloader.MergeFiles("texts.english.txt", textFiles);
            ShowDone();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            UniqueWords.Save("english");
        }

        private void button15_Click(object sender, EventArgs e)
        {
            LangDictionary dictionary = new LangDictionary();
            dictionary.Save("english", "English.2-1-0.dic");
            ShowDone();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            textDownloader.Nouvelobs();
        }

        private void button19_Click(object sender, EventArgs e)
        {
            string[] textFiles = new string[] {
                "spiegel_texty.txt"
                
            };

            TextDownloader.MergeFiles("texts.germany.txt", textFiles);
            ShowDone();
        }

        private void button20_Click(object sender, EventArgs e)
        {
            Frequency.SaveData("germany");
            ShowDone();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            UniqueWords.Save("germany");
        }

        private void button17_Click(object sender, EventArgs e)
        {
            LangDictionary dictionary = new LangDictionary();
            dictionary.Save("germany", "Deutsch.4-4-1.dic");
            ShowDone();
        }

        private void button21_Click(object sender, EventArgs e)
        {
            textDownloader.Nouvelobs();
            ShowDone();
        }

        private void button22_Click(object sender, EventArgs e)
        {
            string[] textFiles = new string[] {
                "nouvelobs_texty.txt"
                
            };

            TextDownloader.MergeFiles("texts.french.txt", textFiles);
            ShowDone();
        }

        private void button23_Click(object sender, EventArgs e)
        {
            string lang = "french";
            Frequency.SaveData(lang);
            UniqueWords.Save(lang);
            LangDictionary dictionary = new LangDictionary();
            dictionary.Save(lang, "french.1-0-0.dic");
            ShowDone();
        }

    }
}
