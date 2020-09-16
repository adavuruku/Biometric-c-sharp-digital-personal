namespace biometrics
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
            this.label1 = new System.Windows.Forms.Label();
            this.ledgerid = new System.Windows.Forms.TextBox();
            this.btnBack = new System.Windows.Forms.Button();
            this.lblPlaceFinger = new System.Windows.Forms.Label();
            this.pbFingerprint = new System.Windows.Forms.PictureBox();
            this.cboReaders = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.login = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbFingerprint)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Ledger ID";
            // 
            // ledgerid
            // 
            this.ledgerid.Location = new System.Drawing.Point(123, 63);
            this.ledgerid.Name = "ledgerid";
            this.ledgerid.Size = new System.Drawing.Size(151, 26);
            this.ledgerid.TabIndex = 1;
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(41, 296);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(230, 35);
            this.btnBack.TabIndex = 7;
            this.btnBack.Text = "Start Capture";
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // lblPlaceFinger
            // 
            this.lblPlaceFinger.BackColor = System.Drawing.Color.Red;
            this.lblPlaceFinger.Location = new System.Drawing.Point(40, 171);
            this.lblPlaceFinger.Name = "lblPlaceFinger";
            this.lblPlaceFinger.Size = new System.Drawing.Size(234, 21);
            this.lblPlaceFinger.TabIndex = 8;
            this.lblPlaceFinger.Text = "Place a finger on the reader";
            // 
            // pbFingerprint
            // 
            this.pbFingerprint.Location = new System.Drawing.Point(41, 95);
            this.pbFingerprint.Name = "pbFingerprint";
            this.pbFingerprint.Size = new System.Drawing.Size(233, 184);
            this.pbFingerprint.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbFingerprint.TabIndex = 9;
            this.pbFingerprint.TabStop = false;
            // 
            // cboReaders
            // 
            this.cboReaders.FormattingEnabled = true;
            this.cboReaders.Location = new System.Drawing.Point(123, 24);
            this.cboReaders.Name = "cboReaders";
            this.cboReaders.Size = new System.Drawing.Size(151, 28);
            this.cboReaders.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 20);
            this.label2.TabIndex = 11;
            this.label2.Text = "Reader";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(41, 337);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(230, 35);
            this.button1.TabIndex = 12;
            this.button1.Text = "Save Record";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // login
            // 
            this.login.Location = new System.Drawing.Point(44, 389);
            this.login.Name = "login";
            this.login.Size = new System.Drawing.Size(230, 35);
            this.login.TabIndex = 13;
            this.login.Text = "Login";
            this.login.Click += new System.EventHandler(this.login_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(351, 477);
            this.Controls.Add(this.login);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboReaders);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.lblPlaceFinger);
            this.Controls.Add(this.pbFingerprint);
            this.Controls.Add(this.ledgerid);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbFingerprint)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ledgerid;
        internal System.Windows.Forms.Button btnBack;
        internal System.Windows.Forms.Label lblPlaceFinger;
        private System.Windows.Forms.ComboBox cboReaders;
        private System.Windows.Forms.Label label2;
        internal System.Windows.Forms.Button button1;
        internal System.Windows.Forms.Button login;
        private System.Windows.Forms.PictureBox pbFingerprint;
    }
}

