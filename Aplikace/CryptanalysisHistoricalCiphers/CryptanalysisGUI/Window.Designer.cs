namespace CryptanalysisGUI
{
    partial class Window
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Window));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.cryptTab = new System.Windows.Forms.TabPage();
            this.encryptTextBox = new System.Windows.Forms.RichTextBox();
            this.contextMenuTextBox = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.smazatVšeAVložitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kopírovatVšeToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.stopButton = new System.Windows.Forms.Button();
            this.textLengthBox = new System.Windows.Forms.Label();
            this.cipherResultBox = new System.Windows.Forms.TextBox();
            this.cipherResultLabel = new System.Windows.Forms.Label();
            this.crackAlgsSettings = new System.Windows.Forms.Panel();
            this.attackTypeCombo = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.crackCiphers = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.autoChooseCipher = new System.Windows.Forms.CheckBox();
            this.languages = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.crackKeyBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.crackTextBox = new System.Windows.Forms.RichTextBox();
            this.contextMenuTextBoxReadOnly = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.kopírovatVšeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.crackButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.encryptTab = new System.Windows.Forms.TabPage();
            this.inputTextbox = new System.Windows.Forms.RichTextBox();
            this.textLengthEncryptTextBox = new System.Windows.Forms.Label();
            this.keepSpacesBox = new System.Windows.Forms.CheckBox();
            this.randomKey = new System.Windows.Forms.Button();
            this.keyTextBox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.ciphersEncrypt = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.decryptButton = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.outputTextBox = new System.Windows.Forms.RichTextBox();
            this.encryptButton = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.progressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.statusBar = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.souborToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.crackTextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.encryptMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.decryptMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.randomKeyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ukončitAplikaciToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nápovědaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jakPoužívatAplikaciToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oAplikaciToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1.SuspendLayout();
            this.cryptTab.SuspendLayout();
            this.contextMenuTextBox.SuspendLayout();
            this.crackAlgsSettings.SuspendLayout();
            this.contextMenuTextBoxReadOnly.SuspendLayout();
            this.encryptTab.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.cryptTab);
            this.tabControl1.Controls.Add(this.encryptTab);
            this.tabControl1.Location = new System.Drawing.Point(12, 27);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(889, 533);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl_selectedIndexChanged);
            // 
            // cryptTab
            // 
            this.cryptTab.Controls.Add(this.encryptTextBox);
            this.cryptTab.Controls.Add(this.stopButton);
            this.cryptTab.Controls.Add(this.textLengthBox);
            this.cryptTab.Controls.Add(this.cipherResultBox);
            this.cryptTab.Controls.Add(this.cipherResultLabel);
            this.cryptTab.Controls.Add(this.crackAlgsSettings);
            this.cryptTab.Controls.Add(this.autoChooseCipher);
            this.cryptTab.Controls.Add(this.languages);
            this.cryptTab.Controls.Add(this.label4);
            this.cryptTab.Controls.Add(this.crackKeyBox);
            this.cryptTab.Controls.Add(this.label3);
            this.cryptTab.Controls.Add(this.label2);
            this.cryptTab.Controls.Add(this.crackTextBox);
            this.cryptTab.Controls.Add(this.crackButton);
            this.cryptTab.Controls.Add(this.label1);
            this.cryptTab.Location = new System.Drawing.Point(4, 22);
            this.cryptTab.Name = "cryptTab";
            this.cryptTab.Padding = new System.Windows.Forms.Padding(3);
            this.cryptTab.Size = new System.Drawing.Size(881, 507);
            this.cryptTab.TabIndex = 0;
            this.cryptTab.Text = "Kryptoanalýza";
            this.cryptTab.UseVisualStyleBackColor = true;
            // 
            // encryptTextBox
            // 
            this.encryptTextBox.ContextMenuStrip = this.contextMenuTextBox;
            this.encryptTextBox.EnableAutoDragDrop = true;
            this.encryptTextBox.Location = new System.Drawing.Point(6, 32);
            this.encryptTextBox.Name = "encryptTextBox";
            this.encryptTextBox.Size = new System.Drawing.Size(680, 185);
            this.encryptTextBox.TabIndex = 24;
            this.encryptTextBox.Text = "";
            this.encryptTextBox.TextChanged += new System.EventHandler(this.encryptTextBox_TextChanged);
            // 
            // contextMenuTextBox
            // 
            this.contextMenuTextBox.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.smazatVšeAVložitToolStripMenuItem,
            this.kopírovatVšeToolStripMenuItem1});
            this.contextMenuTextBox.Name = "contextMenuTextBox";
            this.contextMenuTextBox.Size = new System.Drawing.Size(146, 48);
            // 
            // smazatVšeAVložitToolStripMenuItem
            // 
            this.smazatVšeAVložitToolStripMenuItem.Name = "smazatVšeAVložitToolStripMenuItem";
            this.smazatVšeAVložitToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.smazatVšeAVložitToolStripMenuItem.Text = "Vložit";
            this.smazatVšeAVložitToolStripMenuItem.Click += new System.EventHandler(this.smazatVšeAVložitToolStripMenuItem_Click);
            // 
            // kopírovatVšeToolStripMenuItem1
            // 
            this.kopírovatVšeToolStripMenuItem1.Name = "kopírovatVšeToolStripMenuItem1";
            this.kopírovatVšeToolStripMenuItem1.Size = new System.Drawing.Size(145, 22);
            this.kopírovatVšeToolStripMenuItem1.Text = "Kopírovat vše";
            this.kopírovatVšeToolStripMenuItem1.Click += new System.EventHandler(this.kopírovatVšeToolStripMenuItem1_Click);
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(112, 223);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(100, 23);
            this.stopButton.TabIndex = 23;
            this.stopButton.Text = "Přerušit luštění";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // textLengthBox
            // 
            this.textLengthBox.AutoSize = true;
            this.textLengthBox.Location = new System.Drawing.Point(560, 223);
            this.textLengthBox.Name = "textLengthBox";
            this.textLengthBox.Size = new System.Drawing.Size(79, 13);
            this.textLengthBox.TabIndex = 22;
            this.textLengthBox.Text = "Počet znaků: 0";
            // 
            // cipherResultBox
            // 
            this.cipherResultBox.Location = new System.Drawing.Point(695, 358);
            this.cipherResultBox.Name = "cipherResultBox";
            this.cipherResultBox.ReadOnly = true;
            this.cipherResultBox.Size = new System.Drawing.Size(180, 20);
            this.cipherResultBox.TabIndex = 13;
            this.cipherResultBox.MouseLeave += new System.EventHandler(this.cipherResultBox_MouseLeave);
            this.cipherResultBox.MouseHover += new System.EventHandler(this.cipherResultBox_MouseHover);
            // 
            // cipherResultLabel
            // 
            this.cipherResultLabel.AutoSize = true;
            this.cipherResultLabel.Location = new System.Drawing.Point(695, 342);
            this.cipherResultLabel.Name = "cipherResultLabel";
            this.cipherResultLabel.Size = new System.Drawing.Size(67, 13);
            this.cipherResultLabel.TabIndex = 12;
            this.cipherResultLabel.Text = "Použitá šifra:";
            // 
            // crackAlgsSettings
            // 
            this.crackAlgsSettings.Controls.Add(this.attackTypeCombo);
            this.crackAlgsSettings.Controls.Add(this.label6);
            this.crackAlgsSettings.Controls.Add(this.crackCiphers);
            this.crackAlgsSettings.Controls.Add(this.label5);
            this.crackAlgsSettings.Location = new System.Drawing.Point(694, 91);
            this.crackAlgsSettings.Name = "crackAlgsSettings";
            this.crackAlgsSettings.Size = new System.Drawing.Size(181, 95);
            this.crackAlgsSettings.TabIndex = 11;
            // 
            // attackTypeCombo
            // 
            this.attackTypeCombo.FormattingEnabled = true;
            this.attackTypeCombo.Location = new System.Drawing.Point(0, 58);
            this.attackTypeCombo.Name = "attackTypeCombo";
            this.attackTypeCombo.Size = new System.Drawing.Size(180, 21);
            this.attackTypeCombo.TabIndex = 14;
            this.attackTypeCombo.MouseHover += new System.EventHandler(this.crackAlgs_MouseHover);
            this.attackTypeCombo.MouseLeave += new System.EventHandler(this.crackAlgs_MouseLeave);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 42);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(109, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Specifický algoritmus:";
            // 
            // crackCiphers
            // 
            this.crackCiphers.FormattingEnabled = true;
            this.crackCiphers.Location = new System.Drawing.Point(1, 16);
            this.crackCiphers.Name = "crackCiphers";
            this.crackCiphers.Size = new System.Drawing.Size(180, 21);
            this.crackCiphers.TabIndex = 12;
            this.crackCiphers.MouseHover += new System.EventHandler(this.crackCiphers_MouseHover);
            this.crackCiphers.SelectedIndexChanged += new System.EventHandler(this.crackCiphers_SelectedIndexChanged);
            this.crackCiphers.MouseLeave += new System.EventHandler(this.crackCiphers_MouseLeave);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Použitá šifra:";
            // 
            // autoChooseCipher
            // 
            this.autoChooseCipher.AutoSize = true;
            this.autoChooseCipher.Checked = true;
            this.autoChooseCipher.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autoChooseCipher.Location = new System.Drawing.Point(695, 58);
            this.autoChooseCipher.Name = "autoChooseCipher";
            this.autoChooseCipher.Size = new System.Drawing.Size(138, 17);
            this.autoChooseCipher.TabIndex = 10;
            this.autoChooseCipher.Text = "Uhádnout použitou šifru";
            this.autoChooseCipher.UseVisualStyleBackColor = true;
            this.autoChooseCipher.MouseLeave += new System.EventHandler(this.autoChooseCipher_MouseLeave);
            this.autoChooseCipher.CheckedChanged += new System.EventHandler(this.autoChooseCipher_CheckedChanged);
            this.autoChooseCipher.MouseHover += new System.EventHandler(this.autoChooseCipher_MouseHover);
            // 
            // languages
            // 
            this.languages.FormattingEnabled = true;
            this.languages.Location = new System.Drawing.Point(695, 30);
            this.languages.Name = "languages";
            this.languages.Size = new System.Drawing.Size(180, 21);
            this.languages.TabIndex = 9;
            this.languages.MouseHover += new System.EventHandler(this.languages_MouseHover);
            this.languages.MouseLeave += new System.EventHandler(this.languages_MouseLeave);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(695, 14);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(109, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Jazyk šifrového textu:";
            // 
            // crackKeyBox
            // 
            this.crackKeyBox.Location = new System.Drawing.Point(695, 315);
            this.crackKeyBox.Name = "crackKeyBox";
            this.crackKeyBox.ReadOnly = true;
            this.crackKeyBox.Size = new System.Drawing.Size(180, 20);
            this.crackKeyBox.TabIndex = 7;
            this.crackKeyBox.MouseLeave += new System.EventHandler(this.textBox1_MouseLeave);
            this.crackKeyBox.MouseHover += new System.EventHandler(this.textBox1_MouseHover);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(692, 298);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Šifrový klíč:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 298);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Dešifrovaný text:";
            // 
            // crackTextBox
            // 
            this.crackTextBox.ContextMenuStrip = this.contextMenuTextBoxReadOnly;
            this.crackTextBox.Location = new System.Drawing.Point(6, 314);
            this.crackTextBox.Name = "crackTextBox";
            this.crackTextBox.ReadOnly = true;
            this.crackTextBox.Size = new System.Drawing.Size(682, 187);
            this.crackTextBox.TabIndex = 3;
            this.crackTextBox.Text = "";
            this.crackTextBox.MouseHover += new System.EventHandler(this.crackTextBox_MouseHover);
            this.crackTextBox.MouseLeave += new System.EventHandler(this.crackTextBox_MouseLeave);
            // 
            // contextMenuTextBoxReadOnly
            // 
            this.contextMenuTextBoxReadOnly.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.kopírovatVšeToolStripMenuItem});
            this.contextMenuTextBoxReadOnly.Name = "contextMenuTextBoxReadOnly";
            this.contextMenuTextBoxReadOnly.Size = new System.Drawing.Size(146, 26);
            // 
            // kopírovatVšeToolStripMenuItem
            // 
            this.kopírovatVšeToolStripMenuItem.Name = "kopírovatVšeToolStripMenuItem";
            this.kopírovatVšeToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.kopírovatVšeToolStripMenuItem.Text = "Kopírovat vše";
            this.kopírovatVšeToolStripMenuItem.Click += new System.EventHandler(this.kopírovatVšeToolStripMenuItem_Click);
            // 
            // crackButton
            // 
            this.crackButton.Location = new System.Drawing.Point(6, 223);
            this.crackButton.Name = "crackButton";
            this.crackButton.Size = new System.Drawing.Size(100, 23);
            this.crackButton.TabIndex = 2;
            this.crackButton.Text = "Prolomit text";
            this.crackButton.UseVisualStyleBackColor = true;
            this.crackButton.MouseLeave += new System.EventHandler(this.crackButton_MouseLeave);
            this.crackButton.Click += new System.EventHandler(this.crackButton_Click);
            this.crackButton.MouseHover += new System.EventHandler(this.crackButton_MouseHover);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Zašifrovaný text:";
            // 
            // encryptTab
            // 
            this.encryptTab.Controls.Add(this.inputTextbox);
            this.encryptTab.Controls.Add(this.textLengthEncryptTextBox);
            this.encryptTab.Controls.Add(this.keepSpacesBox);
            this.encryptTab.Controls.Add(this.randomKey);
            this.encryptTab.Controls.Add(this.keyTextBox);
            this.encryptTab.Controls.Add(this.label10);
            this.encryptTab.Controls.Add(this.ciphersEncrypt);
            this.encryptTab.Controls.Add(this.label9);
            this.encryptTab.Controls.Add(this.decryptButton);
            this.encryptTab.Controls.Add(this.label7);
            this.encryptTab.Controls.Add(this.outputTextBox);
            this.encryptTab.Controls.Add(this.encryptButton);
            this.encryptTab.Controls.Add(this.label8);
            this.encryptTab.Location = new System.Drawing.Point(4, 22);
            this.encryptTab.Name = "encryptTab";
            this.encryptTab.Padding = new System.Windows.Forms.Padding(3);
            this.encryptTab.Size = new System.Drawing.Size(881, 507);
            this.encryptTab.TabIndex = 1;
            this.encryptTab.Text = "Šifrování";
            this.encryptTab.UseVisualStyleBackColor = true;
            // 
            // inputTextbox
            // 
            this.inputTextbox.ContextMenuStrip = this.contextMenuTextBox;
            this.inputTextbox.EnableAutoDragDrop = true;
            this.inputTextbox.Location = new System.Drawing.Point(6, 30);
            this.inputTextbox.Name = "inputTextbox";
            this.inputTextbox.Size = new System.Drawing.Size(680, 185);
            this.inputTextbox.TabIndex = 25;
            this.inputTextbox.Text = "";
            this.inputTextbox.TextChanged += new System.EventHandler(this.inputTextBox_TextChanged);
            // 
            // textLengthEncryptTextBox
            // 
            this.textLengthEncryptTextBox.AutoSize = true;
            this.textLengthEncryptTextBox.Location = new System.Drawing.Point(560, 223);
            this.textLengthEncryptTextBox.Name = "textLengthEncryptTextBox";
            this.textLengthEncryptTextBox.Size = new System.Drawing.Size(79, 13);
            this.textLengthEncryptTextBox.TabIndex = 23;
            this.textLengthEncryptTextBox.Text = "Počet znaků: 0";
            // 
            // keepSpacesBox
            // 
            this.keepSpacesBox.AutoSize = true;
            this.keepSpacesBox.Location = new System.Drawing.Point(698, 132);
            this.keepSpacesBox.Name = "keepSpacesBox";
            this.keepSpacesBox.Size = new System.Drawing.Size(108, 17);
            this.keepSpacesBox.TabIndex = 17;
            this.keepSpacesBox.Text = "Ponechat mezery";
            this.keepSpacesBox.UseVisualStyleBackColor = true;
            this.keepSpacesBox.MouseLeave += new System.EventHandler(this.keepSpacesBox_MouseLeave);
            this.keepSpacesBox.MouseHover += new System.EventHandler(this.keepSpacesBox_MouseHover);
            // 
            // randomKey
            // 
            this.randomKey.Location = new System.Drawing.Point(698, 102);
            this.randomKey.Name = "randomKey";
            this.randomKey.Size = new System.Drawing.Size(177, 23);
            this.randomKey.TabIndex = 16;
            this.randomKey.Text = "Vygenerovat náhodný klíč";
            this.randomKey.UseVisualStyleBackColor = true;
            this.randomKey.MouseLeave += new System.EventHandler(this.randomKey_MouseLeave);
            this.randomKey.Click += new System.EventHandler(this.randomKey_Click);
            this.randomKey.MouseHover += new System.EventHandler(this.randomKey_MouseHover);
            // 
            // keyTextBox
            // 
            this.keyTextBox.Location = new System.Drawing.Point(698, 75);
            this.keyTextBox.Name = "keyTextBox";
            this.keyTextBox.Size = new System.Drawing.Size(177, 20);
            this.keyTextBox.TabIndex = 15;
            this.keyTextBox.TextChanged += new System.EventHandler(this.keyTextBox_TextChanged);
            this.keyTextBox.Leave += new System.EventHandler(this.keyTextBox_Leave);
            this.keyTextBox.Enter += new System.EventHandler(this.keyTextBox_Enter);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(698, 58);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(63, 13);
            this.label10.TabIndex = 14;
            this.label10.Text = "Šifrový klíč:";
            // 
            // ciphersEncrypt
            // 
            this.ciphersEncrypt.FormattingEnabled = true;
            this.ciphersEncrypt.Location = new System.Drawing.Point(698, 30);
            this.ciphersEncrypt.Name = "ciphersEncrypt";
            this.ciphersEncrypt.Size = new System.Drawing.Size(180, 21);
            this.ciphersEncrypt.TabIndex = 13;
            this.ciphersEncrypt.SelectedIndexChanged += new System.EventHandler(this.ciphersEncrypt_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(695, 14);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(67, 13);
            this.label9.TabIndex = 12;
            this.label9.Text = "Použitá šifra:";
            // 
            // decryptButton
            // 
            this.decryptButton.Location = new System.Drawing.Point(112, 223);
            this.decryptButton.Name = "decryptButton";
            this.decryptButton.Size = new System.Drawing.Size(100, 23);
            this.decryptButton.TabIndex = 11;
            this.decryptButton.Text = "Dešifrovat text";
            this.decryptButton.UseVisualStyleBackColor = true;
            this.decryptButton.MouseLeave += new System.EventHandler(this.decryptButton_MouseLeave);
            this.decryptButton.Click += new System.EventHandler(this.decryptButton_Click);
            this.decryptButton.MouseHover += new System.EventHandler(this.decryptButton_MouseHover);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 298);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(212, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "Výsledný zašifrovaný nebo dešifrovaný text:";
            // 
            // outputTextBox
            // 
            this.outputTextBox.ContextMenuStrip = this.contextMenuTextBoxReadOnly;
            this.outputTextBox.Location = new System.Drawing.Point(6, 314);
            this.outputTextBox.Name = "outputTextBox";
            this.outputTextBox.ReadOnly = true;
            this.outputTextBox.Size = new System.Drawing.Size(682, 187);
            this.outputTextBox.TabIndex = 9;
            this.outputTextBox.Text = "";
            this.outputTextBox.MouseHover += new System.EventHandler(this.outputTextBox_MouseHover);
            this.outputTextBox.MouseLeave += new System.EventHandler(this.outputTextBox_MouseLeave);
            // 
            // encryptButton
            // 
            this.encryptButton.Location = new System.Drawing.Point(6, 223);
            this.encryptButton.Name = "encryptButton";
            this.encryptButton.Size = new System.Drawing.Size(100, 23);
            this.encryptButton.TabIndex = 8;
            this.encryptButton.Text = "Zašifrovat text";
            this.encryptButton.UseVisualStyleBackColor = true;
            this.encryptButton.MouseLeave += new System.EventHandler(this.encryptButton_MouseLeave);
            this.encryptButton.Click += new System.EventHandler(this.encryptButton_Click);
            this.encryptButton.MouseHover += new System.EventHandler(this.encryptButton_MouseHover);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 14);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(220, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "Text, který chcete zašifrovat nebo dešifrovat:";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.progressBar,
            this.statusBar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 564);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(913, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "Kryptoanalýza historických šifer";
            // 
            // progressBar
            // 
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(100, 16);
            // 
            // statusBar
            // 
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(173, 17);
            this.statusBar.Text = "Kryptoanalýza historických šifer";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.souborToolStripMenuItem,
            this.nápovědaToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(913, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // souborToolStripMenuItem
            // 
            this.souborToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.crackTextMenuItem,
            this.stopMenuItem,
            this.toolStripSeparator1,
            this.encryptMenuItem,
            this.decryptMenuItem,
            this.randomKeyMenuItem,
            this.toolStripSeparator2,
            this.ukončitAplikaciToolStripMenuItem});
            this.souborToolStripMenuItem.Name = "souborToolStripMenuItem";
            this.souborToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.souborToolStripMenuItem.Text = "&Soubor";
            // 
            // crackTextMenuItem
            // 
            this.crackTextMenuItem.Name = "crackTextMenuItem";
            this.crackTextMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.B)));
            this.crackTextMenuItem.Size = new System.Drawing.Size(252, 22);
            this.crackTextMenuItem.Text = "Prolomit text";
            this.crackTextMenuItem.Click += new System.EventHandler(this.crackTextMenuItem_Click);
            // 
            // stopMenuItem
            // 
            this.stopMenuItem.Name = "stopMenuItem";
            this.stopMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.stopMenuItem.Size = new System.Drawing.Size(252, 22);
            this.stopMenuItem.Text = "Přerušit luštění";
            this.stopMenuItem.Click += new System.EventHandler(this.stopMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(249, 6);
            // 
            // encryptMenuItem
            // 
            this.encryptMenuItem.Name = "encryptMenuItem";
            this.encryptMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.encryptMenuItem.Size = new System.Drawing.Size(252, 22);
            this.encryptMenuItem.Text = "Zašifrovat text";
            this.encryptMenuItem.Click += new System.EventHandler(this.encryptMenuItem_Click);
            // 
            // decryptMenuItem
            // 
            this.decryptMenuItem.Name = "decryptMenuItem";
            this.decryptMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.decryptMenuItem.Size = new System.Drawing.Size(252, 22);
            this.decryptMenuItem.Text = "Dešifrovat text";
            this.decryptMenuItem.Click += new System.EventHandler(this.decryptMenuItem_Click);
            // 
            // randomKeyMenuItem
            // 
            this.randomKeyMenuItem.Name = "randomKeyMenuItem";
            this.randomKeyMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.randomKeyMenuItem.Size = new System.Drawing.Size(252, 22);
            this.randomKeyMenuItem.Text = "Vygenerovat náhodný klíč";
            this.randomKeyMenuItem.Click += new System.EventHandler(this.randomKeyMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(249, 6);
            // 
            // ukončitAplikaciToolStripMenuItem
            // 
            this.ukončitAplikaciToolStripMenuItem.Name = "ukončitAplikaciToolStripMenuItem";
            this.ukončitAplikaciToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.ukončitAplikaciToolStripMenuItem.Size = new System.Drawing.Size(252, 22);
            this.ukončitAplikaciToolStripMenuItem.Text = "Ukončit aplikaci";
            this.ukončitAplikaciToolStripMenuItem.Click += new System.EventHandler(this.ukončitAplikaciToolStripMenuItem_Click);
            // 
            // nápovědaToolStripMenuItem
            // 
            this.nápovědaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.jakPoužívatAplikaciToolStripMenuItem,
            this.oAplikaciToolStripMenuItem});
            this.nápovědaToolStripMenuItem.Name = "nápovědaToolStripMenuItem";
            this.nápovědaToolStripMenuItem.Size = new System.Drawing.Size(73, 20);
            this.nápovědaToolStripMenuItem.Text = "&Nápověda";
            // 
            // jakPoužívatAplikaciToolStripMenuItem
            // 
            this.jakPoužívatAplikaciToolStripMenuItem.Name = "jakPoužívatAplikaciToolStripMenuItem";
            this.jakPoužívatAplikaciToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.jakPoužívatAplikaciToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.jakPoužívatAplikaciToolStripMenuItem.Text = "&Jak používat aplikaci";
            this.jakPoužívatAplikaciToolStripMenuItem.Click += new System.EventHandler(this.jakPoužívatAplikaciToolStripMenuItem_Click);
            // 
            // oAplikaciToolStripMenuItem
            // 
            this.oAplikaciToolStripMenuItem.Name = "oAplikaciToolStripMenuItem";
            this.oAplikaciToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.oAplikaciToolStripMenuItem.Text = "&O aplikaci";
            this.oAplikaciToolStripMenuItem.Click += new System.EventHandler(this.oAplikaciToolStripMenuItem_Click);
            // 
            // Window
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(913, 586);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Window";
            this.Text = "Bletchley Park";
            this.Load += new System.EventHandler(this.AltGUI_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AltGUI_FormClosing);
            this.tabControl1.ResumeLayout(false);
            this.cryptTab.ResumeLayout(false);
            this.cryptTab.PerformLayout();
            this.contextMenuTextBox.ResumeLayout(false);
            this.crackAlgsSettings.ResumeLayout(false);
            this.crackAlgsSettings.PerformLayout();
            this.contextMenuTextBoxReadOnly.ResumeLayout(false);
            this.encryptTab.ResumeLayout(false);
            this.encryptTab.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage cryptTab;
        private System.Windows.Forms.TabPage encryptTab;
        private System.Windows.Forms.Button crackButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox crackTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox crackKeyBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar progressBar;
        private System.Windows.Forms.ToolStripStatusLabel statusBar;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel crackAlgsSettings;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox autoChooseCipher;
        private System.Windows.Forms.ComboBox languages;
        private System.Windows.Forms.ComboBox crackCiphers;
        private System.Windows.Forms.ComboBox attackTypeCombo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.RichTextBox outputTextBox;
        private System.Windows.Forms.Button encryptButton;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button decryptButton;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox keyTextBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox ciphersEncrypt;
        private System.Windows.Forms.CheckBox keepSpacesBox;
        private System.Windows.Forms.Button randomKey;
        private System.Windows.Forms.TextBox cipherResultBox;
        private System.Windows.Forms.Label cipherResultLabel;
        private System.Windows.Forms.Label textLengthBox;
        private System.Windows.Forms.ContextMenuStrip contextMenuTextBoxReadOnly;
        private System.Windows.Forms.ToolStripMenuItem kopírovatVšeToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuTextBox;
        private System.Windows.Forms.ToolStripMenuItem kopírovatVšeToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem smazatVšeAVložitToolStripMenuItem;
        private System.Windows.Forms.Label textLengthEncryptTextBox;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem souborToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nápovědaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem oAplikaciToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem jakPoužívatAplikaciToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem crackTextMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem encryptMenuItem;
        private System.Windows.Forms.ToolStripMenuItem decryptMenuItem;
        private System.Windows.Forms.ToolStripMenuItem randomKeyMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem ukončitAplikaciToolStripMenuItem;
        private System.Windows.Forms.RichTextBox encryptTextBox;
        private System.Windows.Forms.RichTextBox inputTextbox;
    }
}

