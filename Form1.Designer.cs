
namespace Yatzy_1
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
            this.gbxMöjligheter = new System.Windows.Forms.GroupBox();
            this.chlbxMöjligheter = new System.Windows.Forms.CheckedListBox();
            this.gbxSpel = new System.Windows.Forms.GroupBox();
            this.lblPoäng = new System.Windows.Forms.Label();
            this.lblSpecialfall = new System.Windows.Forms.Label();
            this.lblSlagKvar = new System.Windows.Forms.Label();
            this.btnKlar = new System.Windows.Forms.Button();
            this.chlbxSpecialfall = new System.Windows.Forms.CheckedListBox();
            this.btnRulla = new System.Windows.Forms.Button();
            this.pctTärning5 = new System.Windows.Forms.PictureBox();
            this.pctTärning4 = new System.Windows.Forms.PictureBox();
            this.pctTärning3 = new System.Windows.Forms.PictureBox();
            this.pctTärning2 = new System.Windows.Forms.PictureBox();
            this.pctTärning1 = new System.Windows.Forms.PictureBox();
            this.gbxMöjligheter.SuspendLayout();
            this.gbxSpel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pctTärning5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctTärning4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctTärning3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctTärning2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctTärning1)).BeginInit();
            this.SuspendLayout();
            // 
            // gbxMöjligheter
            // 
            this.gbxMöjligheter.Controls.Add(this.chlbxMöjligheter);
            this.gbxMöjligheter.Location = new System.Drawing.Point(328, 12);
            this.gbxMöjligheter.Name = "gbxMöjligheter";
            this.gbxMöjligheter.Size = new System.Drawing.Size(199, 271);
            this.gbxMöjligheter.TabIndex = 0;
            this.gbxMöjligheter.TabStop = false;
            this.gbxMöjligheter.Text = "Använda Specialfall:";
            // 
            // chlbxMöjligheter
            // 
            this.chlbxMöjligheter.CheckOnClick = true;
            this.chlbxMöjligheter.FormattingEnabled = true;
            this.chlbxMöjligheter.Items.AddRange(new object[] {
            "Ettor",
            "Tvåor",
            "Treor",
            "Fyror",
            "Femmor",
            "Sexor",
            "BONUS",
            "Par",
            "Två Par",
            "Triss",
            "Fyrtal",
            "Kåk",
            "Liten Stege",
            "Stor Stege",
            "Chans",
            "Yatzy"});
            this.chlbxMöjligheter.Location = new System.Drawing.Point(6, 19);
            this.chlbxMöjligheter.Name = "chlbxMöjligheter";
            this.chlbxMöjligheter.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.chlbxMöjligheter.Size = new System.Drawing.Size(187, 244);
            this.chlbxMöjligheter.TabIndex = 7;
            // 
            // gbxSpel
            // 
            this.gbxSpel.Controls.Add(this.lblPoäng);
            this.gbxSpel.Controls.Add(this.lblSpecialfall);
            this.gbxSpel.Controls.Add(this.lblSlagKvar);
            this.gbxSpel.Controls.Add(this.btnKlar);
            this.gbxSpel.Controls.Add(this.chlbxSpecialfall);
            this.gbxSpel.Controls.Add(this.btnRulla);
            this.gbxSpel.Controls.Add(this.pctTärning5);
            this.gbxSpel.Controls.Add(this.pctTärning4);
            this.gbxSpel.Controls.Add(this.pctTärning3);
            this.gbxSpel.Controls.Add(this.pctTärning2);
            this.gbxSpel.Controls.Add(this.pctTärning1);
            this.gbxSpel.Location = new System.Drawing.Point(12, 12);
            this.gbxSpel.Name = "gbxSpel";
            this.gbxSpel.Size = new System.Drawing.Size(310, 271);
            this.gbxSpel.TabIndex = 2;
            this.gbxSpel.TabStop = false;
            this.gbxSpel.Text = "Spel";
            // 
            // lblPoäng
            // 
            this.lblPoäng.AutoSize = true;
            this.lblPoäng.Location = new System.Drawing.Point(197, 80);
            this.lblPoäng.Name = "lblPoäng";
            this.lblPoäng.Size = new System.Drawing.Size(79, 13);
            this.lblPoäng.TabIndex = 3;
            this.lblPoäng.Text = "Spelarpoäng: 0";
            // 
            // lblSpecialfall
            // 
            this.lblSpecialfall.AutoSize = true;
            this.lblSpecialfall.Location = new System.Drawing.Point(15, 104);
            this.lblSpecialfall.Name = "lblSpecialfall";
            this.lblSpecialfall.Size = new System.Drawing.Size(92, 13);
            this.lblSpecialfall.TabIndex = 9;
            this.lblSpecialfall.Text = "Möjliga specialfall:";
            // 
            // lblSlagKvar
            // 
            this.lblSlagKvar.AutoSize = true;
            this.lblSlagKvar.Location = new System.Drawing.Point(29, 80);
            this.lblSlagKvar.Name = "lblSlagKvar";
            this.lblSlagKvar.Size = new System.Drawing.Size(64, 13);
            this.lblSlagKvar.TabIndex = 8;
            this.lblSlagKvar.Text = "Slag kvar: 3";
            // 
            // btnKlar
            // 
            this.btnKlar.Location = new System.Drawing.Point(7, 145);
            this.btnKlar.Name = "btnKlar";
            this.btnKlar.Size = new System.Drawing.Size(109, 23);
            this.btnKlar.TabIndex = 7;
            this.btnKlar.Text = "Klar med slag";
            this.btnKlar.UseVisualStyleBackColor = true;
            this.btnKlar.Click += new System.EventHandler(this.btnKlar_Click);
            // 
            // chlbxSpecialfall
            // 
            this.chlbxSpecialfall.CheckOnClick = true;
            this.chlbxSpecialfall.FormattingEnabled = true;
            this.chlbxSpecialfall.Location = new System.Drawing.Point(122, 104);
            this.chlbxSpecialfall.Name = "chlbxSpecialfall";
            this.chlbxSpecialfall.Size = new System.Drawing.Size(182, 154);
            this.chlbxSpecialfall.TabIndex = 6;
            // 
            // btnRulla
            // 
            this.btnRulla.Location = new System.Drawing.Point(110, 75);
            this.btnRulla.Name = "btnRulla";
            this.btnRulla.Size = new System.Drawing.Size(75, 23);
            this.btnRulla.TabIndex = 5;
            this.btnRulla.Text = "Rulla!";
            this.btnRulla.UseVisualStyleBackColor = true;
            this.btnRulla.Click += new System.EventHandler(this.btnRulla_Click);
            // 
            // pctTärning5
            // 
            this.pctTärning5.Image = global::Yatzy_1.tärningsgrafik.d0;
            this.pctTärning5.Location = new System.Drawing.Point(238, 19);
            this.pctTärning5.Name = "pctTärning5";
            this.pctTärning5.Size = new System.Drawing.Size(52, 50);
            this.pctTärning5.TabIndex = 4;
            this.pctTärning5.TabStop = false;
            this.pctTärning5.Click += new System.EventHandler(this.pctTärning5_Click);
            // 
            // pctTärning4
            // 
            this.pctTärning4.Image = global::Yatzy_1.tärningsgrafik.d0;
            this.pctTärning4.Location = new System.Drawing.Point(180, 19);
            this.pctTärning4.Name = "pctTärning4";
            this.pctTärning4.Size = new System.Drawing.Size(52, 50);
            this.pctTärning4.TabIndex = 3;
            this.pctTärning4.TabStop = false;
            this.pctTärning4.Click += new System.EventHandler(this.pctTärning4_Click);
            // 
            // pctTärning3
            // 
            this.pctTärning3.Image = global::Yatzy_1.tärningsgrafik.d0;
            this.pctTärning3.Location = new System.Drawing.Point(122, 19);
            this.pctTärning3.Name = "pctTärning3";
            this.pctTärning3.Size = new System.Drawing.Size(52, 50);
            this.pctTärning3.TabIndex = 2;
            this.pctTärning3.TabStop = false;
            this.pctTärning3.Click += new System.EventHandler(this.pctTärning3_Click);
            // 
            // pctTärning2
            // 
            this.pctTärning2.Image = global::Yatzy_1.tärningsgrafik.d0;
            this.pctTärning2.Location = new System.Drawing.Point(64, 19);
            this.pctTärning2.Name = "pctTärning2";
            this.pctTärning2.Size = new System.Drawing.Size(52, 50);
            this.pctTärning2.TabIndex = 1;
            this.pctTärning2.TabStop = false;
            this.pctTärning2.Click += new System.EventHandler(this.pctTärning2_Click);
            // 
            // pctTärning1
            // 
            this.pctTärning1.BackColor = System.Drawing.SystemColors.Control;
            this.pctTärning1.Image = global::Yatzy_1.tärningsgrafik.d0;
            this.pctTärning1.Location = new System.Drawing.Point(6, 19);
            this.pctTärning1.Name = "pctTärning1";
            this.pctTärning1.Size = new System.Drawing.Size(52, 50);
            this.pctTärning1.TabIndex = 0;
            this.pctTärning1.TabStop = false;
            this.pctTärning1.Click += new System.EventHandler(this.pctTärning1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(539, 292);
            this.Controls.Add(this.gbxSpel);
            this.Controls.Add(this.gbxMöjligheter);
            this.Name = "Form1";
            this.Text = "Yatzy";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.gbxMöjligheter.ResumeLayout(false);
            this.gbxSpel.ResumeLayout(false);
            this.gbxSpel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pctTärning5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctTärning4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctTärning3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctTärning2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctTärning1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxMöjligheter;
        private System.Windows.Forms.GroupBox gbxSpel;
        private System.Windows.Forms.PictureBox pctTärning5;
        private System.Windows.Forms.PictureBox pctTärning4;
        private System.Windows.Forms.PictureBox pctTärning3;
        private System.Windows.Forms.PictureBox pctTärning2;
        private System.Windows.Forms.PictureBox pctTärning1;
        private System.Windows.Forms.Button btnRulla;
        private System.Windows.Forms.CheckedListBox chlbxSpecialfall;
        private System.Windows.Forms.Button btnKlar;
        private System.Windows.Forms.Label lblSpecialfall;
        private System.Windows.Forms.Label lblSlagKvar;
        private System.Windows.Forms.Label lblPoäng;
        private System.Windows.Forms.CheckedListBox chlbxMöjligheter;
    }
}

