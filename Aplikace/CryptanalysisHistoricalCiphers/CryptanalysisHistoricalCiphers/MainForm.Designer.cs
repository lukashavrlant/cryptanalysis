namespace CryptanalysisHistoricalCiphers
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.inputTextbox = new System.Windows.Forms.TextBox();
            this.outputTextbox = new System.Windows.Forms.TextBox();
            this.inputLabel = new System.Windows.Forms.Label();
            this.outputLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.crackAlgorithms = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cipherCryptanalysis = new System.Windows.Forms.ComboBox();
            this.languages = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.generateRandomKeyButton = new System.Windows.Forms.Button();
            this.keepSpacesButton = new System.Windows.Forms.CheckBox();
            this.keyTextbox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cipherEncrypt = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.crackCipher = new System.Windows.Forms.Button();
            this.encryptButton = new System.Windows.Forms.Button();
            this.decryptButton = new System.Windows.Forms.Button();
            this.letterCountLabel = new System.Windows.Forms.Label();
            this.pasteTextButton = new System.Windows.Forms.Button();
            this.copyTextButton = new System.Windows.Forms.Button();
            this.copyUpButton = new System.Windows.Forms.Button();
            this.crackKeyResultTextbox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // inputTextbox
            // 
            this.inputTextbox.Location = new System.Drawing.Point(231, 28);
            this.inputTextbox.Multiline = true;
            this.inputTextbox.Name = "inputTextbox";
            this.inputTextbox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.inputTextbox.Size = new System.Drawing.Size(666, 223);
            this.inputTextbox.TabIndex = 3;
            this.inputTextbox.TextChanged += new System.EventHandler(this.inputTextbox_TextChanged);
            // 
            // outputTextbox
            // 
            this.outputTextbox.Location = new System.Drawing.Point(231, 329);
            this.outputTextbox.Multiline = true;
            this.outputTextbox.Name = "outputTextbox";
            this.outputTextbox.ReadOnly = true;
            this.outputTextbox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.outputTextbox.Size = new System.Drawing.Size(666, 223);
            this.outputTextbox.TabIndex = 4;
            // 
            // inputLabel
            // 
            this.inputLabel.AutoSize = true;
            this.inputLabel.Location = new System.Drawing.Point(231, 9);
            this.inputLabel.Name = "inputLabel";
            this.inputLabel.Size = new System.Drawing.Size(67, 13);
            this.inputLabel.TabIndex = 5;
            this.inputLabel.Text = "Vstupní text:";
            // 
            // outputLabel
            // 
            this.outputLabel.AutoSize = true;
            this.outputLabel.Location = new System.Drawing.Point(234, 310);
            this.outputLabel.Name = "outputLabel";
            this.outputLabel.Size = new System.Drawing.Size(53, 13);
            this.outputLabel.TabIndex = 6;
            this.outputLabel.Text = "Výsledek:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Location = new System.Drawing.Point(10, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(215, 570);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Nastavení";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.crackAlgorithms);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.cipherCryptanalysis);
            this.groupBox3.Controls.Add(this.languages);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Location = new System.Drawing.Point(7, 19);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 223);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Kryptoanalýza";
            // 
            // crackAlgorithms
            // 
            this.crackAlgorithms.FormattingEnabled = true;
            this.crackAlgorithms.Location = new System.Drawing.Point(9, 106);
            this.crackAlgorithms.Name = "crackAlgorithms";
            this.crackAlgorithms.Size = new System.Drawing.Size(175, 21);
            this.crackAlgorithms.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 90);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(174, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Výběr kryptoanalytického algoritmu:";
            // 
            // cipherCryptanalysis
            // 
            this.cipherCryptanalysis.FormattingEnabled = true;
            this.cipherCryptanalysis.Location = new System.Drawing.Point(9, 47);
            this.cipherCryptanalysis.Name = "cipherCryptanalysis";
            this.cipherCryptanalysis.Size = new System.Drawing.Size(175, 21);
            this.cipherCryptanalysis.TabIndex = 5;
            this.cipherCryptanalysis.SelectedIndexChanged += new System.EventHandler(this.cipherCryptanalysis_SelectedIndexChanged);
            // 
            // languages
            // 
            this.languages.FormattingEnabled = true;
            this.languages.Location = new System.Drawing.Point(9, 165);
            this.languages.Name = "languages";
            this.languages.Size = new System.Drawing.Size(175, 21);
            this.languages.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 31);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Použitá šifra:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 149);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(109, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Jazyk šifrového textu:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.generateRandomKeyButton);
            this.groupBox2.Controls.Add(this.keepSpacesButton);
            this.groupBox2.Controls.Add(this.keyTextbox);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.cipherEncrypt);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(7, 274);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 290);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Šifrování";
            // 
            // generateRandomKeyButton
            // 
            this.generateRandomKeyButton.Location = new System.Drawing.Point(13, 114);
            this.generateRandomKeyButton.Name = "generateRandomKeyButton";
            this.generateRandomKeyButton.Size = new System.Drawing.Size(175, 30);
            this.generateRandomKeyButton.TabIndex = 5;
            this.generateRandomKeyButton.Text = "Vygeneruj náhodný klíč";
            this.generateRandomKeyButton.UseVisualStyleBackColor = true;
            this.generateRandomKeyButton.Click += new System.EventHandler(this.generateRandomKeyButton_Click);
            // 
            // keepSpacesButton
            // 
            this.keepSpacesButton.AutoSize = true;
            this.keepSpacesButton.Location = new System.Drawing.Point(13, 167);
            this.keepSpacesButton.Name = "keepSpacesButton";
            this.keepSpacesButton.Size = new System.Drawing.Size(108, 17);
            this.keepSpacesButton.TabIndex = 4;
            this.keepSpacesButton.Text = "Zachovat mezery";
            this.keepSpacesButton.UseVisualStyleBackColor = true;
            // 
            // keyTextbox
            // 
            this.keyTextbox.Location = new System.Drawing.Point(13, 88);
            this.keyTextbox.Name = "keyTextbox";
            this.keyTextbox.Size = new System.Drawing.Size(175, 20);
            this.keyTextbox.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Šifrovací klíč:";
            // 
            // cipherEncrypt
            // 
            this.cipherEncrypt.FormattingEnabled = true;
            this.cipherEncrypt.Location = new System.Drawing.Point(13, 36);
            this.cipherEncrypt.Name = "cipherEncrypt";
            this.cipherEncrypt.Size = new System.Drawing.Size(175, 21);
            this.cipherEncrypt.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Použitá šifra:";
            // 
            // crackCipher
            // 
            this.crackCipher.Enabled = false;
            this.crackCipher.Location = new System.Drawing.Point(231, 257);
            this.crackCipher.Name = "crackCipher";
            this.crackCipher.Size = new System.Drawing.Size(125, 30);
            this.crackCipher.TabIndex = 8;
            this.crackCipher.Text = "&Prolom šifru";
            this.crackCipher.UseVisualStyleBackColor = true;
            this.crackCipher.Click += new System.EventHandler(this.crackCipher_Click);
            // 
            // encryptButton
            // 
            this.encryptButton.Location = new System.Drawing.Point(362, 257);
            this.encryptButton.Name = "encryptButton";
            this.encryptButton.Size = new System.Drawing.Size(125, 30);
            this.encryptButton.TabIndex = 2;
            this.encryptButton.Text = "&Zašifruj";
            this.encryptButton.UseVisualStyleBackColor = true;
            this.encryptButton.Click += new System.EventHandler(this.encryptButton_Click);
            // 
            // decryptButton
            // 
            this.decryptButton.Location = new System.Drawing.Point(493, 257);
            this.decryptButton.Name = "decryptButton";
            this.decryptButton.Size = new System.Drawing.Size(125, 30);
            this.decryptButton.TabIndex = 3;
            this.decryptButton.Text = "&Dešifruj";
            this.decryptButton.UseVisualStyleBackColor = true;
            this.decryptButton.Click += new System.EventHandler(this.decryptButton_Click);
            // 
            // letterCountLabel
            // 
            this.letterCountLabel.AutoSize = true;
            this.letterCountLabel.Location = new System.Drawing.Point(755, 266);
            this.letterCountLabel.Name = "letterCountLabel";
            this.letterCountLabel.Size = new System.Drawing.Size(35, 13);
            this.letterCountLabel.TabIndex = 8;
            this.letterCountLabel.Text = "label6";
            this.letterCountLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // pasteTextButton
            // 
            this.pasteTextButton.Location = new System.Drawing.Point(624, 257);
            this.pasteTextButton.Name = "pasteTextButton";
            this.pasteTextButton.Size = new System.Drawing.Size(125, 30);
            this.pasteTextButton.TabIndex = 9;
            this.pasteTextButton.Text = "&Vložit ze schránky";
            this.pasteTextButton.UseVisualStyleBackColor = true;
            this.pasteTextButton.Click += new System.EventHandler(this.pasteTextButton_Click);
            // 
            // copyTextButton
            // 
            this.copyTextButton.Location = new System.Drawing.Point(231, 558);
            this.copyTextButton.Name = "copyTextButton";
            this.copyTextButton.Size = new System.Drawing.Size(125, 30);
            this.copyTextButton.TabIndex = 10;
            this.copyTextButton.Text = "&Kopíruj do schránky";
            this.copyTextButton.UseVisualStyleBackColor = true;
            this.copyTextButton.Click += new System.EventHandler(this.copyTextButton_Click);
            // 
            // copyUpButton
            // 
            this.copyUpButton.Location = new System.Drawing.Point(362, 558);
            this.copyUpButton.Name = "copyUpButton";
            this.copyUpButton.Size = new System.Drawing.Size(130, 30);
            this.copyUpButton.TabIndex = 11;
            this.copyUpButton.Text = "↑Kopíruj text &nahoru↑";
            this.copyUpButton.UseVisualStyleBackColor = true;
            this.copyUpButton.Click += new System.EventHandler(this.copyUpButton_Click);
            // 
            // crackKeyResultTextbox
            // 
            this.crackKeyResultTextbox.Location = new System.Drawing.Point(722, 564);
            this.crackKeyResultTextbox.Name = "crackKeyResultTextbox";
            this.crackKeyResultTextbox.ReadOnly = true;
            this.crackKeyResultTextbox.Size = new System.Drawing.Size(175, 20);
            this.crackKeyResultTextbox.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(636, 567);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Prolomený klíč:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(909, 615);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.crackKeyResultTextbox);
            this.Controls.Add(this.crackCipher);
            this.Controls.Add(this.copyUpButton);
            this.Controls.Add(this.decryptButton);
            this.Controls.Add(this.encryptButton);
            this.Controls.Add(this.copyTextButton);
            this.Controls.Add(this.pasteTextButton);
            this.Controls.Add(this.letterCountLabel);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.outputLabel);
            this.Controls.Add(this.inputLabel);
            this.Controls.Add(this.outputTextbox);
            this.Controls.Add(this.inputTextbox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Kryptoanalýza historických šifer";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox inputTextbox;
        private System.Windows.Forms.TextBox outputTextbox;
        private System.Windows.Forms.Label inputLabel;
        private System.Windows.Forms.Label outputLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox keyTextbox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cipherEncrypt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox languages;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button encryptButton;
        private System.Windows.Forms.Button decryptButton;
        private System.Windows.Forms.ComboBox cipherCryptanalysis;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox crackAlgorithms;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox keepSpacesButton;
        private System.Windows.Forms.Button crackCipher;
        private System.Windows.Forms.Button generateRandomKeyButton;
        private System.Windows.Forms.Label letterCountLabel;
        private System.Windows.Forms.Button pasteTextButton;
        private System.Windows.Forms.Button copyTextButton;
        private System.Windows.Forms.Button copyUpButton;
        private System.Windows.Forms.TextBox crackKeyResultTextbox;
        private System.Windows.Forms.Label label6;
    }
}

