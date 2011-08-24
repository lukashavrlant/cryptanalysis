using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using System.Web;
using System.Threading;
using CryptanalysisCore;
using ExtensionMethods;

namespace Statistics
{
    class TextDownloader
    {
        private static string TextsFolder = Storage.ConfPath + Storage.TextsFolder;

        public delegate void ProgressAction(string title);
        public delegate void AfterFinishAction(string title);

        private ProgressAction progressAction;
        private AfterFinishAction afterFinish;

        private int counter;

        public TextDownloader(ProgressAction progressAction, AfterFinishAction afterFinishAction)
        {
            this.progressAction = progressAction;
            this.afterFinish = afterFinishAction;
            counter = 0;
        }

        public TextDownloader(ProgressAction progressAction)
        {
            this.progressAction = progressAction;
        }

        private void ParallelDosAttack(string filename, string regex, Func<string[], string[]> parseFunction)
        {
            int threadsCount = 2;
            string[] urls = ParseRegUrls(TextsFolder + filename + "_url.txt", regex);
            int cutLength = urls.Length - urls.Length % threadsCount;
            urls = urls.Take(cutLength).ToArray();
            string[][] cutUrls = urls.Split(threadsCount);
            object lockObject = new object();
            int partSize = cutLength / threadsCount;

            string[][] downloadedPages = new string[threadsCount][];
            int[] synchro = new int[threadsCount].Fill(0);

            threadsCount.Times(i =>
                {
                    Thread thread = new Thread(() =>
                        {
                            downloadedPages[i] = DownloadWebPages(cutUrls[i]);

                            lock (lockObject)
                            {
                                synchro[i] = 1;
                                if (synchro.Sum() == threadsCount)
                                {
                                    string[] pages = new string[0];

                                    foreach (string[] newPages in downloadedPages)
                                        pages = pages.Union(newPages).ToArray();

                                    string[] texts = parseFunction(pages);

                                    SaveTexts(texts, filename);
                                    afterFinish("hotovo");
                                }
                            }
                        });

                    thread.Priority = ThreadPriority.BelowNormal;
                    thread.Start();
                });
        }

        private void DosAttack(string filename, string regex, Func<string[], string[]> parseFunction)
        {
            string[] urls = ParseRegUrls(TextsFolder + filename + "_url.txt", regex);
            string[] pages = DownloadWebPages(urls).Where(x => x.Trim() != string.Empty).ToArray();
            string[] texts = parseFunction(pages);

            SaveTexts(texts, filename);
        }

        private void DosAttack(string filename, string regex, Func<string[], string[]> parseFunction, Encoding encoding)
        {
            string[] urls = ParseRegUrls(TextsFolder + filename + "_url.txt", regex);
            string[] pages = DownloadWebPages(urls, encoding).Where(x => x.Trim() != string.Empty).ToArray();
            string[] texts = parseFunction(pages);

            SaveTexts(texts, filename);
        }

        public void Nouvelobs()
        {
            DosAttack("nouvelobs", @"^http://tempsreel.nouvelobs.com/[a-z]+/[a-z]+/(.*)\.html$", ParseNouvelobs);
        }

        public void Spiegel()
        {
            DosAttack("spiegel", @"^http://www.spiegel.de/[a-z]+/[a-z]+/[0-9,]+.html$", ParseSpiegel, Encoding.GetEncoding(28591));
        }

        public void TimeMagazine()
        {
            ParallelDosAttack("timemagazine", @"^http://www.time.com/time/[a-z]+/article/[0-9,]+\.html$", ParseTimeMagazine);
        }

        public void BlogyAktualne()
        {
            DosAttack("blogyaktualne", @"^http://blog.aktualne.centrum.cz/blogy/[a-zA-Z0-9\-]+\.php.itemid=[0-9]+$", ParseBlogyAktualne);
        }

        public void Novinky()
        {
            DosAttack("novinky", @"^http://www.novinky.cz/[a-z]+/[0-9]+.+$", ParseNovinky);
        }

        public void BlogyNovinky()
        {
            DosAttack("blogynovinky", @"^http://[a-z0-9]+\.blogy.novinky.cz/[0-9]+/.+$", ParseBlogyNovinky);
        }

        public void Vitalia()
        {
            DosAttack("vitalia", @"^http://www.vitalia.cz/clanky/[a-z0-9\-]+/", ParseVitalia);
        }

        public void Lupa()
        {
            DosAttack("lupa", @"^http://www.lupa.cz/clanky/[a-z0-9\-]+/", ParseLupa, Encoding.GetEncoding(28592));
        }

        public void Respekt()
        {
            DosAttack("respekt", @"^http://[a-z0-9]+\.blog.respekt.cz/c/[0-9]+/(.*)\.html$", ParseRespekt, Encoding.GetEncoding(1250));
        }

        private void SaveTexts(string[] texts, string filename)
        {
            StringBuilder sb = new StringBuilder();
            texts.ForEach(text => sb.AppendFormat("{0}\n", text));
            File.WriteAllText(TextsFolder + filename + "_texty.txt", sb.ToString());
        }

        private string[] ParseSpiegel(string[] pages)
        {
            string temp = "";
            string[] parsePages = new string[pages.Length];

            for (int i = 0; i < pages.Length; i++)
            {
                try
                {
                    temp = pages[i].Replace('\n', ' ').Replace('\r', ' ');
                    int start = temp.IndexOf("spIntroTeaser");
                    int stop = temp.IndexOf("spSocialBookmark");
                    temp = Regex.Match(temp, "spIntroTeaser(.*)spSocialBookmark").Value;
                    temp = temp.Substring("spIntroTeaser".Length + 2);
                    temp = temp.Substring(0, temp.Length - "<div id=\"spSocialBookmark".Length);
                    temp = StripHTMLTags(temp);
                    temp = temp.Trim();
                    temp = temp.Substring(0, temp.LastIndexOf('.'));
                    parsePages[i] = temp;

                    progressAction(string.Format("Zp: {0}/{1}", i.ToString(), pages.Length));
                }
                catch (Exception)
                {
                    continue;
                }
            }

            return parsePages;
        }

        private string[] ParseNouvelobs(string[] pages)
        {
            string temp = "";
            string[] parsePages = new string[pages.Length];

            for (int i = 0; i < pages.Length; i++)
            {
                try
                {
                    temp = pages[i].Replace('\n', ' ').Replace('\r', ' ');
                    temp = Regex.Match(temp, "<div class=\"obs09-article-body\" id=\"obs09-article-body\">(.*)<!-- Stats  @Begin -->").Value;
                    temp = Entities2Chars(temp);
                    temp = StripHTMLTags(temp);
                    temp = temp.Trim();
                    temp = temp.Substring(0, temp.LastIndexOf('.'));
                    parsePages[i] = temp;

                    progressAction(string.Format("Zp: {0}/{1}", i.ToString(), pages.Length));
                }
                catch (Exception)
                {
                    continue;
                }
            }

            return parsePages;
        }

        private string[] ParseTimeMagazine(string[] pages)
        {
            string temp = "";
            string[] parsePages = new string[pages.Length];

            for (int i = 0; i < pages.Length; i++)
            {
                try
                {
                    temp = pages[i].Replace('\n', ' ').Replace('\r', ' ');
                    temp = Regex.Match(temp, "<li id=\"diggShare\">(.*)<div class=\"quigo\">").Value;
                    temp = StripHTMLTags(temp);
                    temp = temp.Trim();
                    parsePages[i] = temp;

                    progressAction(string.Format("Zp: {0}/{1}", i.ToString(), pages.Length));
                }
                catch (Exception)
                {
                    continue;
                }
            }

            return parsePages;
        }

        private string[] ParseBlogyAktualne(string[] pages)
        {
            string temp = "";
            string[] parsePages = new string[pages.Length];

            for (int i = 0; i < pages.Length; i++)
            {
                try
                {
                    temp = pages[i].Replace('\n', ' ').Replace('\r', ' ');
                    temp = Regex.Match(temp, "<div class=\"contentbody-item\">(.*)<h2>Komentáře</h2>").Value;
                    temp = StripHTMLTags(temp);
                    temp = temp.Remove("Komentáře");
                    temp = temp.Trim();
                    parsePages[i] = temp;

                    progressAction(string.Format("Zp: {0}/{1}", i.ToString(), pages.Length));
                }
                catch (Exception)
                {
                    continue;
                }
            }

            return parsePages;
        }

        private string[] ParseRespekt(string[] pages)
        {
            string temp = "";
            string[] parsePages = new string[pages.Length];

            for (int i = 0; i < pages.Length; i++)
            {
                try
                {
                    temp = pages[i].Replace('\n', ' ').Replace('\r', ' ');
                    temp = Regex.Match(temp, "<div class=\"article-perex\">(.*)<span class=\"article-author\">").Value;
                    temp = StripHTMLTags(temp);
                    temp = temp.Trim();
                    parsePages[i] = temp;

                    progressAction(string.Format("Zp: {0}/{1}", i.ToString(), pages.Length));
                }
                catch (Exception)
                {
                    continue;
                }
            }

            return parsePages;
        }

        private string[] ParseLupa(string[] pages)
        {
            string temp = "";
            string[] parsePages = new string[pages.Length];

            for (int i = 0; i < pages.Length; i++)
            {
                try
                {
                    temp = pages[i].Replace('\n', ' ').Replace('\r', ' ');
                    temp = Regex.Match(temp, "<h1>(.*)<div id=\"author\">").Value;
                    temp = StripHTMLTags(temp);
                    temp = temp.Remove("Nálepky");
                    temp = temp.Trim();
                    parsePages[i] = temp;

                    progressAction(string.Format("Zp: {0}/{1}", i.ToString(), pages.Length));
                }
                catch (Exception)
                {
                    continue;
                }
            }

            return parsePages;
        }

        private string[] ParseVitalia(string[] pages)
        {
            string temp = "";
            string[] parsePages = new string[pages.Length];

            for (int i = 0; i < pages.Length; i++)
            {
                try
                {
                    temp = pages[i].Replace('\n', ' ').Replace('\r', ' ');
                    temp = Regex.Match(temp, "<div id=\"main\" class=\"columns clear\">(.*)<div class=\"author clear\">").Value;
                    temp = StripHTMLTags(temp);
                    temp = temp.Remove("Nálepky");
                    temp = temp.Trim();
                    parsePages[i] = temp;

                    progressAction(string.Format("Zp: {0}/{1}", i.ToString(), pages.Length));
                }
                catch (Exception)
                {
                    continue;
                }
            }

            return parsePages;
        }

        private string[] ParseBlogyNovinky(string[] pages)
        {
            string temp = "";
            string[] parsePages = new string[pages.Length];

            for (int i = 0; i < pages.Length; i++)
            {
                try
                {
                    temp = pages[i].Replace('\n', ' ').Replace('\r', ' ');
                    temp = Regex.Match(temp, "<div id=\"content\">(.*)discussVisibilitySwitchLink").Value;
                    temp = StripHTMLTags(temp);
                    temp = temp.Replace("<a href=\"#\" id=\"discussVisibilitySwitchLink", "");
                    temp = temp.Trim();
                    parsePages[i] = temp;

                    progressAction(string.Format("Zp: {0}/{1}", i.ToString(), pages.Length));
                }
                catch (Exception)
                {
                    continue;
                }
            }

            return parsePages;
        }

        private string[] ParseNovinky(string[] pages)
        {
            string temp;
            string[] parsePages = new string[pages.Length];

            for (int i = 0; i < pages.Length; i++)
            {
                try
                {
                    temp = Regex.Match(pages[i], "id=\"articleBody\"(.*)Sklik\\-kontext\\-stop", RegexOptions.Multiline).Value;
                    temp = StripHTMLTags(temp);
                    temp = temp.Replace("id=\"articleBody\">", "").Replace("<!-- Sklik-kontext-stop", "").Replace("celá zpráva", "");
                    temp = temp.Trim();
                    parsePages[i] = temp;

                    progressAction(string.Format("Zp: {0}/{1}", i.ToString(), pages.Length));
                }
                catch (Exception)
                {
                    continue;
                }
            }

            return parsePages;
        }

        /// <summary>
        /// Naparsuje stránky stažené z iDnes
        /// </summary>
        /// <param name="pages"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        private string[] ParseIdnes(string[] pages)
        {
            int start, end;
            string temp;
            string[] parsePages = new string[pages.Length];

            for(int i = 0; i < pages.Length; i++)
            {
                try
                {
                    start = pages[i].IndexOf("FULLTEXTSTART");
                    end = pages[i].IndexOf("FULLTEXTSTOP");

                    temp = StripHTMLTags(pages[i].Substring(start, end - start));
                    temp = temp.Substring(16, temp.Length - 35);
                    temp = Analyse.NormalizeText(temp.RemoveDiacritics(), Analyse.TextTypes.WithSpacesLower).Trim();
                    temp = temp.Replace("documentwritelnbadge", string.Empty)
                        .Replace("badge", string.Empty)
                        .Replace("linkuj", string.Empty)
                        .Replace("documentwriteln", string.Empty)
                        .Replace(" var ", string.Empty);
                    parsePages[i] = temp;
                    progressAction(string.Format("Zp: {0}/{1}", i.ToString(), pages.Length));
                }
                catch (Exception)
                {
                    continue;
                }
            }

            return parsePages;
        }

        /// <summary>
        /// Odstraní z HTML všechny tagy
        /// </summary>
        /// <param name="html">HTML dokument</param>
        /// <returns>Text bez HTML značek</returns>
        public static string StripHTMLTags(string html)
        {
            string temp = html;
            temp = Regex.Replace(temp, @"<script(.*)</script>", " ");
            temp = Regex.Replace(temp, @"&[#0-9a-zA-Z]+;", " ");
            temp = Regex.Replace(temp, @"<(.|\n)*?>", " ");
            
            return temp;
        }


        /// <summary>
        /// Stáhne webovou stránku a změní kódování na utf
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="inputEncoding"></param>
        /// <returns></returns>
        public static string DownloadWebPage(string Url, Encoding inputEncoding)
        {
            try
            {
                WebClient Client = new WebClient();
                byte[] page = Client.DownloadData(Url);
                Encoding utf = Encoding.UTF8;
                byte[] encodePage = Encoding.Convert(inputEncoding, utf, page);
                MemoryStream stream = new MemoryStream(encodePage);
                StreamReader reader = new StreamReader(stream);
                return reader.ReadToEnd();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Stáhne webovou stránku
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        public static string DownloadWebPage(string Url)
        {
            try
            {
                WebClient Client = new WebClient();
                byte[] page = Client.DownloadData(Url);
                MemoryStream stream = new MemoryStream(page);
                StreamReader reader = new StreamReader(stream);
                return reader.ReadToEnd();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Stáhne požadované stránky a vrátí je jako pole.
        /// </summary>
        /// <param name="urls"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public string[] DownloadWebPages(string[] urls)
        {
            string[] pages = new string[urls.Length];

            for (int i = 0; i < urls.Length; i++)
            {
                pages[i] = DownloadWebPage(urls[i]);
                counter++;
                progressAction(counter.ToString());
            }

            return pages;
        }


        public string[] DownloadWebPages(string[] urls, Encoding encoding)
        {
            string[] pages = new string[urls.Length];

            for (int i = 0; i < urls.Length; i++)
            {
                pages[i] = DownloadWebPage(urls[i], encoding);
                progressAction(string.Format("St: {0}/{1}", i.ToString(), urls.Length));
            }

            return pages;
        }

        /// <summary>
        /// Nalezne jen ty odkazy, které obsahují nějaký klíč/string
        /// </summary>
        /// <param name="path"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string[] ParseMessUrls(string path, string include)
        {
            return File.ReadAllLines(path).Where(x => x.Contains(include)).ToArray();
        }

        public string[] ParseMessUrls(string path, string key, string exclude)
        {
            return File.ReadAllLines(path).Where(x => x.Contains(key) && !x.Contains(exclude)).ToArray();
        }

        public string[] ParseRegUrls(string path, string pattern)
        {
            return File.ReadAllLines(path).Where(x => Regex.IsMatch(x, pattern)).ToArray();
        }

        /// <summary>
        /// Sloučí dohromady předané textové soubory a uloží do
        /// jednoho výstupního souboru output.
        /// </summary>
        /// <param name="output"></param>
        /// <param name="inputs"></param>
        public static void MergeFiles(string output, string[] inputs)
        {
            List<string> files = new List<string>();
            inputs.ForEach(path => files.Add(NormalizeText(File.ReadAllText(TextsFolder + path))));
            File.WriteAllText(TextsFolder + output, files.Implode("\n"));
        }

        public static string NormalizeText(string text)
        {
            return Analyse.NormalizeText(text.RemoveDiacritics(), Analyse.TextTypes.WithSpacesLower);
        }

        public static void RepairIdnes()
        {
            string[] idnes = File.ReadAllLines(TextsFolder + "idnes_texty.txt");
            List<string> repair = new List<string>();
            idnes.ForEach(line =>
                {
                    if (line.Trim() != string.Empty)
                    {
                        repair.Add(line.Substring(0, line.Length - 100));
                    }
                });

            File.WriteAllLines(TextsFolder + "idnes_texty.txt", repair.ToArray());
        }

        public string Entities2Chars(string text)
        {
            string[] entities = { "&eacute;", "&ecirc;", "&ugrave;", "&agrave;", "&egrave;" };
            string[] chars = { "é", "ê", "ù", "à", "è" };

            return text.Replace(entities, chars);
        }
    }
}