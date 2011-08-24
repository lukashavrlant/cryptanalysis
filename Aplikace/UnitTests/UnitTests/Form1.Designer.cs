namespace UnitTests
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.button2 = new System.Windows.Forms.Button();
            this.parCount = new System.Windows.Forms.TextBox();
            this.letterCount = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.caesarBruteButton = new System.Windows.Forms.Button();
            this.caesarTriangleButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.takeTextbox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.languageSelect = new System.Windows.Forms.ComboBox();
            this.button4 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(13, 403);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(800, 23);
            this.progressBar1.TabIndex = 0;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(214, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(192, 76);
            this.button2.TabIndex = 2;
            this.button2.Text = "Monoalfabetická";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // parCount
            // 
            this.parCount.Location = new System.Drawing.Point(651, 180);
            this.parCount.Name = "parCount";
            this.parCount.Size = new System.Drawing.Size(100, 20);
            this.parCount.TabIndex = 3;
            this.parCount.Text = "1000";
            // 
            // letterCount
            // 
            this.letterCount.Location = new System.Drawing.Point(651, 219);
            this.letterCount.Name = "letterCount";
            this.letterCount.Size = new System.Drawing.Size(100, 20);
            this.letterCount.TabIndex = 4;
            this.letterCount.Text = "3000";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(510, 183);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Počet testovaných řetězců";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(491, 222);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(154, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Počet písmen v jednom řetězci";
            // 
            // caesarBruteButton
            // 
            this.caesarBruteButton.Location = new System.Drawing.Point(13, 12);
            this.caesarBruteButton.Name = "caesarBruteButton";
            this.caesarBruteButton.Size = new System.Drawing.Size(195, 76);
            this.caesarBruteButton.TabIndex = 7;
            this.caesarBruteButton.Text = "Caesarova šifra (Brute Force)";
            this.caesarBruteButton.UseVisualStyleBackColor = true;
            this.caesarBruteButton.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // caesarTriangleButton
            // 
            this.caesarTriangleButton.Location = new System.Drawing.Point(12, 94);
            this.caesarTriangleButton.Name = "caesarTriangleButton";
            this.caesarTriangleButton.Size = new System.Drawing.Size(195, 76);
            this.caesarTriangleButton.TabIndex = 8;
            this.caesarTriangleButton.Text = "Caesarova šifra (Triangle)";
            this.caesarTriangleButton.UseVisualStyleBackColor = true;
            this.caesarTriangleButton.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 305);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(195, 76);
            this.button1.TabIndex = 9;
            this.button1.Text = "Vigenere, délky klíčů";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_2);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(13, 219);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(195, 76);
            this.button3.TabIndex = 10;
            this.button3.Text = "Vigenere, brute";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click_2);
            // 
            // takeTextbox
            // 
            this.takeTextbox.Location = new System.Drawing.Point(651, 258);
            this.takeTextbox.Name = "takeTextbox";
            this.takeTextbox.Size = new System.Drawing.Size(100, 20);
            this.takeTextbox.TabIndex = 12;
            this.takeTextbox.Text = "9";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(594, 264);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Take";
            // 
            // languageSelect
            // 
            this.languageSelect.FormattingEnabled = true;
            this.languageSelect.Items.AddRange(new object[] {
            "czech",
            "english"});
            this.languageSelect.Location = new System.Drawing.Point(612, 305);
            this.languageSelect.Name = "languageSelect";
            this.languageSelect.Size = new System.Drawing.Size(192, 21);
            this.languageSelect.TabIndex = 14;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(214, 219);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(195, 76);
            this.button4.TabIndex = 15;
            this.button4.Text = "Transposition";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click_2);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(825, 438);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.languageSelect);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.takeTextbox);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.caesarTriangleButton);
            this.Controls.Add(this.caesarBruteButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.letterCount);
            this.Controls.Add(this.parCount);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.progressBar1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Unit testy";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox parCount;
        private System.Windows.Forms.TextBox letterCount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button caesarBruteButton;
        private System.Windows.Forms.Button caesarTriangleButton;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox takeTextbox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox languageSelect;
        private System.Windows.Forms.Button button4;
    }
}

