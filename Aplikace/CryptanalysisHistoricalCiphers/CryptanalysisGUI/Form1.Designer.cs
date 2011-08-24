namespace CryptanalysisGUI
{
    partial class AltGUI
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.cryptTab = new System.Windows.Forms.TabPage();
            this.encryptTab = new System.Windows.Forms.TabPage();
            this.encryptTextBox = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.crackButton = new System.Windows.Forms.Button();
            this.crackTextBox = new System.Windows.Forms.RichTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tabControl1.SuspendLayout();
            this.cryptTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.cryptTab);
            this.tabControl1.Controls.Add(this.encryptTab);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(889, 496);
            this.tabControl1.TabIndex = 0;
            // 
            // cryptTab
            // 
            this.cryptTab.Controls.Add(this.groupBox1);
            this.cryptTab.Controls.Add(this.crackTextBox);
            this.cryptTab.Controls.Add(this.crackButton);
            this.cryptTab.Controls.Add(this.label1);
            this.cryptTab.Controls.Add(this.encryptTextBox);
            this.cryptTab.Location = new System.Drawing.Point(4, 22);
            this.cryptTab.Name = "cryptTab";
            this.cryptTab.Padding = new System.Windows.Forms.Padding(3);
            this.cryptTab.Size = new System.Drawing.Size(881, 470);
            this.cryptTab.TabIndex = 0;
            this.cryptTab.Text = "Kryptoanalýza";
            this.cryptTab.UseVisualStyleBackColor = true;
            // 
            // encryptTab
            // 
            this.encryptTab.Location = new System.Drawing.Point(4, 22);
            this.encryptTab.Name = "encryptTab";
            this.encryptTab.Padding = new System.Windows.Forms.Padding(3);
            this.encryptTab.Size = new System.Drawing.Size(881, 435);
            this.encryptTab.TabIndex = 1;
            this.encryptTab.Text = "Šifrování";
            this.encryptTab.UseVisualStyleBackColor = true;
            // 
            // encryptTextBox
            // 
            this.encryptTextBox.Location = new System.Drawing.Point(6, 30);
            this.encryptTextBox.Name = "encryptTextBox";
            this.encryptTextBox.Size = new System.Drawing.Size(682, 187);
            this.encryptTextBox.TabIndex = 0;
            this.encryptTextBox.Text = "";
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
            // crackButton
            // 
            this.crackButton.Enabled = false;
            this.crackButton.Location = new System.Drawing.Point(6, 223);
            this.crackButton.Name = "crackButton";
            this.crackButton.Size = new System.Drawing.Size(157, 39);
            this.crackButton.TabIndex = 2;
            this.crackButton.Text = "Prolom šifrový text";
            this.crackButton.UseVisualStyleBackColor = true;
            this.crackButton.Click += new System.EventHandler(this.crackButton_Click);
            // 
            // crackTextBox
            // 
            this.crackTextBox.Location = new System.Drawing.Point(6, 268);
            this.crackTextBox.Name = "crackTextBox";
            this.crackTextBox.ReadOnly = true;
            this.crackTextBox.Size = new System.Drawing.Size(682, 187);
            this.crackTextBox.TabIndex = 3;
            this.crackTextBox.Text = "";
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(694, 30);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(181, 187);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Nastavení kryptoanalýzy";
            // 
            // AltGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(913, 520);
            this.Controls.Add(this.tabControl1);
            this.Name = "AltGUI";
            this.Text = "NewGUI";
            this.Load += new System.EventHandler(this.AltGUI_Load);
            this.tabControl1.ResumeLayout(false);
            this.cryptTab.ResumeLayout(false);
            this.cryptTab.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage cryptTab;
        private System.Windows.Forms.RichTextBox encryptTextBox;
        private System.Windows.Forms.TabPage encryptTab;
        private System.Windows.Forms.Button crackButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox crackTextBox;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}

