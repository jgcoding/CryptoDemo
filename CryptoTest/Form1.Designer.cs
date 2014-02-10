namespace CryptoTest
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
            this.btnEncrypt = new System.Windows.Forms.Button();
            this.txtClearText = new System.Windows.Forms.TextBox();
            this.txtEncryptedText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnDecrypt = new System.Windows.Forms.Button();
            this.cboStrength = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnGenNonce = new System.Windows.Forms.Button();
            this.btnNewSalt = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.numCryptoIterations = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.cboCryptoHash = new System.Windows.Forms.ComboBox();
            this.numNonceLength = new System.Windows.Forms.NumericUpDown();
            this.txtNonceValue = new System.Windows.Forms.TextBox();
            this.txtCryptoAESSalt = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCryptoIterations)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNonceLength)).BeginInit();
            this.SuspendLayout();
            // 
            // btnEncrypt
            // 
            this.btnEncrypt.Location = new System.Drawing.Point(231, 223);
            this.btnEncrypt.Name = "btnEncrypt";
            this.btnEncrypt.Size = new System.Drawing.Size(102, 33);
            this.btnEncrypt.TabIndex = 0;
            this.btnEncrypt.Text = "Encrypt >>";
            this.btnEncrypt.UseVisualStyleBackColor = true;
            this.btnEncrypt.Click += new System.EventHandler(this.btnEncrypt_Click);
            // 
            // txtClearText
            // 
            this.txtClearText.Location = new System.Drawing.Point(10, 184);
            this.txtClearText.Multiline = true;
            this.txtClearText.Name = "txtClearText";
            this.txtClearText.Size = new System.Drawing.Size(210, 117);
            this.txtClearText.TabIndex = 2;
            // 
            // txtEncryptedText
            // 
            this.txtEncryptedText.Location = new System.Drawing.Point(346, 184);
            this.txtEncryptedText.Multiline = true;
            this.txtEncryptedText.Name = "txtEncryptedText";
            this.txtEncryptedText.Size = new System.Drawing.Size(210, 117);
            this.txtEncryptedText.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 164);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Clear Text";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(346, 164);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Encrypted Text";
            // 
            // btnDecrypt
            // 
            this.btnDecrypt.Location = new System.Drawing.Point(231, 262);
            this.btnDecrypt.Name = "btnDecrypt";
            this.btnDecrypt.Size = new System.Drawing.Size(102, 33);
            this.btnDecrypt.TabIndex = 6;
            this.btnDecrypt.Text = "<< Decrypt";
            this.btnDecrypt.UseVisualStyleBackColor = true;
            this.btnDecrypt.Click += new System.EventHandler(this.btnDecrypt_Click);
            // 
            // cboStrength
            // 
            this.cboStrength.FormattingEnabled = true;
            this.cboStrength.Items.AddRange(new object[] {
            "",
            "128 Bit",
            "256 Bit"});
            this.cboStrength.Location = new System.Drawing.Point(231, 184);
            this.cboStrength.Name = "cboStrength";
            this.cboStrength.Size = new System.Drawing.Size(102, 21);
            this.cboStrength.TabIndex = 7;
            this.cboStrength.Text = "256 Bit";
            this.cboStrength.SelectedIndexChanged += new System.EventHandler(this.cboStrength_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnGenNonce);
            this.groupBox1.Controls.Add(this.btnNewSalt);
            this.groupBox1.Controls.Add(this.btnSave);
            this.groupBox1.Controls.Add(this.btnLoad);
            this.groupBox1.Controls.Add(this.numCryptoIterations);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.cboCryptoHash);
            this.groupBox1.Controls.Add(this.numNonceLength);
            this.groupBox1.Controls.Add(this.txtNonceValue);
            this.groupBox1.Controls.Add(this.txtCryptoAESSalt);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(10, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(574, 146);
            this.groupBox1.TabIndex = 25;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "App.config";
            // 
            // btnGenNonce
            // 
            this.btnGenNonce.Location = new System.Drawing.Point(182, 83);
            this.btnGenNonce.Name = "btnGenNonce";
            this.btnGenNonce.Size = new System.Drawing.Size(28, 23);
            this.btnGenNonce.TabIndex = 41;
            this.btnGenNonce.Text = "...";
            this.btnGenNonce.UseVisualStyleBackColor = true;
            this.btnGenNonce.Click += new System.EventHandler(this.btnGenNonce_Click);
            // 
            // btnNewSalt
            // 
            this.btnNewSalt.Location = new System.Drawing.Point(457, 26);
            this.btnNewSalt.Name = "btnNewSalt";
            this.btnNewSalt.Size = new System.Drawing.Size(104, 23);
            this.btnNewSalt.TabIndex = 40;
            this.btnNewSalt.Text = "New Salt";
            this.btnNewSalt.UseVisualStyleBackColor = true;
            this.btnNewSalt.Click += new System.EventHandler(this.btnNewSalt_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(269, 112);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 38;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(188, 112);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 37;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // numCryptoIterations
            // 
            this.numCryptoIterations.Location = new System.Drawing.Point(314, 55);
            this.numCryptoIterations.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numCryptoIterations.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numCryptoIterations.Name = "numCryptoIterations";
            this.numCryptoIterations.Size = new System.Drawing.Size(76, 20);
            this.numCryptoIterations.TabIndex = 36;
            this.numCryptoIterations.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(220, 57);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(86, 13);
            this.label8.TabIndex = 35;
            this.label8.Text = "Crypto Iterations:";
            // 
            // cboCryptoHash
            // 
            this.cboCryptoHash.FormattingEnabled = true;
            this.cboCryptoHash.Items.AddRange(new object[] {
            "SHA1"});
            this.cboCryptoHash.Location = new System.Drawing.Point(101, 54);
            this.cboCryptoHash.Name = "cboCryptoHash";
            this.cboCryptoHash.Size = new System.Drawing.Size(100, 21);
            this.cboCryptoHash.TabIndex = 34;
            // 
            // numNonceLength
            // 
            this.numNonceLength.Location = new System.Drawing.Point(101, 84);
            this.numNonceLength.Maximum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.numNonceLength.Minimum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.numNonceLength.Name = "numNonceLength";
            this.numNonceLength.Size = new System.Drawing.Size(62, 20);
            this.numNonceLength.TabIndex = 33;
            this.numNonceLength.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // txtNonceValue
            // 
            this.txtNonceValue.Location = new System.Drawing.Point(322, 86);
            this.txtNonceValue.Name = "txtNonceValue";
            this.txtNonceValue.Size = new System.Drawing.Size(240, 20);
            this.txtNonceValue.TabIndex = 32;
            // 
            // txtCryptoAESSalt
            // 
            this.txtCryptoAESSalt.Location = new System.Drawing.Point(101, 29);
            this.txtCryptoAESSalt.Name = "txtCryptoAESSalt";
            this.txtCryptoAESSalt.Size = new System.Drawing.Size(289, 20);
            this.txtCryptoAESSalt.TabIndex = 30;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(220, 89);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 13);
            this.label7.TabIndex = 29;
            this.label7.Text = "Nonce Value:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 86);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(78, 13);
            this.label6.TabIndex = 28;
            this.label6.Text = "Nonce Length:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 57);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 13);
            this.label5.TabIndex = 27;
            this.label5.Text = "Crypto Hash:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 13);
            this.label3.TabIndex = 25;
            this.label3.Text = "CryptoAES Salt:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(594, 311);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cboStrength);
            this.Controls.Add(this.btnDecrypt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtEncryptedText);
            this.Controls.Add(this.txtClearText);
            this.Controls.Add(this.btnEncrypt);
            this.Name = "Form1";
            this.Text = "Encryption Configuration";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCryptoIterations)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNonceLength)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnEncrypt;
		private System.Windows.Forms.TextBox txtClearText;
		private System.Windows.Forms.TextBox txtEncryptedText;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnDecrypt;
		private System.Windows.Forms.ComboBox cboStrength;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnLoad;
		private System.Windows.Forms.NumericUpDown numCryptoIterations;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.ComboBox cboCryptoHash;
		private System.Windows.Forms.NumericUpDown numNonceLength;
        private System.Windows.Forms.TextBox txtNonceValue;
		private System.Windows.Forms.TextBox txtCryptoAESSalt;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button btnGenNonce;
        private System.Windows.Forms.Button btnNewSalt;
	}
}

