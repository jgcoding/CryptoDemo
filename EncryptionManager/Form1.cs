using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Crypto;
using System.Security.Cryptography;
using System.Collections;

namespace EncryptionLab
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}
		
		CryptoLevel m_keySize = CryptoLevel.AES256;
		byte[] elevel = new byte[2];

        byte[] io = new byte[4] { 0, 0, 0, 2 };

		private void btnEncrypt_Click(object sender, EventArgs e)
		{
			
			// refresh the encryption strength (this doesn't seem to make any difference in the cipher)
			setAesStrength();

			// generate the nonce for the outbound
			byte[] nonce = CryptoManager.GenerateNonce(16);

			// Encrypt the string to an array of bytes.
			byte[] clearPayload = System.Text.Encoding.ASCII.GetBytes(txtClearText.Text);
			txtEncryptedText.Text = Convert.ToBase64String(CryptoManager.EncryptAES(io, m_keySize));
		}

		private void btnDecrypt_Click(object sender, EventArgs e)
		{
			// refresh the encryption strength (this doesn't seem to make any difference in the cipher)
			setAesStrength();

			// to test inbound EncryptionLevel settings, convert the keysize to a byte to verify accurate and safe casting to 
			// a CryptoStrength enum selection.
			byte estrength = Convert.ToByte(m_keySize);

			// decrypt the message (returned as a byte array)
			byte[] response = CryptoManager.DecryptAES(Convert.FromBase64String(txtEncryptedText.Text), m_keySize);
			// translate the byte array response into readable text:
			txtClearText.Text = Encoding.ASCII.GetString(response);
		}

		private void cboStrength_SelectedIndexChanged(object sender, EventArgs e)
		{
			// update the encryption strength (this doesn't seem to make any difference in the cipher)
			setAesStrength();
		}

		void setAesStrength()
		{
			switch (cboStrength.SelectedIndex)
			{
				case 0:					
					m_keySize = CryptoLevel.AES128;
					CryptoManager.SetEncryptionLevel(m_keySize);					
					break;
				default:
					m_keySize = CryptoLevel.AES256;
					CryptoManager.SetEncryptionLevel(m_keySize);
					break;
			}
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			elevel = BitConverter.GetBytes(0);
			BitArray bits = new BitArray(elevel);			
			
			txtClearText.Text = "this is a test.  ";
			byte[] nonce = new byte[] { 1, 2, 3, 4, 5, 5, 4, 3 };
			String nonceString = Convert.ToBase64String(nonce);
		}

		private void btnLoad_Click(object sender, EventArgs e)
		{
			txtCrypto256Salt.Text = CryptoConfiguration.GetAppSettings.Settings["Crypto256Salt"].Value;
			txtCrypto128Salt.Text = CryptoConfiguration.GetAppSettings.Settings["Crypto128Salt"].Value;
			if (String.IsNullOrWhiteSpace(CryptoConfiguration.GetAppSettings.Settings["CryptoHash"].Value))
			{
				cboCryptoHash.SelectedItem = "SHA1";
			}
			else
			{
				cboCryptoHash.SelectedItem = CryptoConfiguration.GetAppSettings.Settings["CryptoHash"].Value;
			}

			if (String.IsNullOrWhiteSpace(CryptoConfiguration.GetAppSettings.Settings["CryptoIterations"].Value) || int.Parse(CryptoConfiguration.GetAppSettings.Settings["CryptoIterations"].Value) < numCryptoIterations.Minimum)
			{
				numCryptoIterations.Value = numCryptoIterations.Minimum;
			}
			else
			{
				numCryptoIterations.Value = int.Parse(CryptoConfiguration.GetAppSettings.Settings["CryptoIterations"].Value);
			}

			if (String.IsNullOrWhiteSpace(CryptoConfiguration.GetAppSettings.Settings["NonceLength"].Value) || int.Parse(CryptoConfiguration.GetAppSettings.Settings["NonceLength"].Value) < numNonceLength.Minimum)
			{
				numNonceLength.Value = numNonceLength.Minimum; 
			}
			else 
			{
				numNonceLength.Value = int.Parse(CryptoConfiguration.GetAppSettings.Settings["NonceLength"].Value);				
			}

			if (!String.IsNullOrWhiteSpace(CryptoConfiguration.GetAppSettings.Settings["Nonce"].Value))
			{
				txtNonceValue.Text = CryptoConfiguration.GetAppSettings.Settings["Nonce"].Value;
			}
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			if (txtCrypto256Salt.Text.Length > 0)
			{
				CryptoConfiguration.GetAppSettings.Settings["Crypto256Salt"].Value = txtCrypto256Salt.Text;				
			}
			if (txtCrypto128Salt.Text.Length > 0)
			{
				CryptoConfiguration.GetAppSettings.Settings["Crypto128Salt"].Value = txtCrypto128Salt.Text;
			}
			if (cboCryptoHash.SelectedIndex > 0)
			{
				CryptoConfiguration.GetAppSettings.Settings["CryptoHash"].Value = cboCryptoHash.SelectedText;
			}
			if (numCryptoIterations.Value > 0)
			{
				CryptoConfiguration.GetAppSettings.Settings["CryptoIterations"].Value = numCryptoIterations.Value.ToString();
			}
			if (numNonceLength.Value > 0)
			{
				CryptoConfiguration.GetAppSettings.Settings["NonceLength"].Value = numNonceLength.Value.ToString();
			}
			if (txtNonceValue.TextLength >= 8)
			{
				CryptoConfiguration.GetAppSettings.Settings["Nonce"].Value = txtNonceValue.Text;
			}
			else
			{
				MessageBox.Show("Nonce length must be greater than 8 characters", "ERROR!");
			}
			CryptoConfiguration.LastUpdated = DateTime.UtcNow;
			CryptoConfiguration.SaveConfig();
		}

		private void btnNewSalt_Click(object sender, EventArgs e)
		{
			txtCrypto256Salt.Text = Encoding.ASCII.GetString(CryptoManager.GenerateNonce(32));
			txtCrypto128Salt.Text = Encoding.ASCII.GetString(CryptoManager.GenerateNonce(16));
		}

		private void btnGenNonce_Click(object sender, EventArgs e)
		{
			if (numNonceLength.Value > 7)
			{
				txtNonceValue.Text = Encoding.ASCII.GetString(CryptoManager.GenerateNonce((int)numNonceLength.Value));
			}
		}
	}
}
