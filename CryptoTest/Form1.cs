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
using Newtonsoft.Json.Linq;
using CodeRight.Crypto;

namespace CryptoTest
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

            // Encrypt the string to an array of bytes.
            byte[] clearPayload = Encoding.ASCII.GetBytes(txtClearText.Text);
            CryptoManager.Crypto.ToJson = Convert.ToBase64String(CryptoManager.Crypto.EncryptAES(clearPayload, m_keySize));
            txtClearText.Text = CryptoManager.Crypto.ToJson;
            txtCryptoAESSalt.Text = CryptoManager.Crypto.CryptoSalt;
            txtNonceValue.Text = Encoding.ASCII.GetString(CryptoManager.Crypto.Nonce);

            var bson = Encoding.ASCII.GetBytes(CryptoManager.Crypto.ToJson);
            txtEncryptedText.Text = Convert.ToBase64String(bson);
            var json = Encoding.ASCII.GetString(Convert.FromBase64String(txtEncryptedText.Text));

            var token = new RESTAuthToken { RESTTransactionId = Guid.NewGuid(), RESTAuthTokenInfo = bson, RESTExpiration = DateTime.UtcNow.AddMinutes(60) };
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            // refresh the encryption strength (this doesn't seem to make any difference in the cipher)

            // to test inbound EncryptionLevel settings, convert the keysize to a byte to verify accurate and safe casting to 
            // a CryptoStrength enum selection.
            byte estrength = Convert.ToByte(m_keySize);

            // retrieve the crypto configuration for this particular cipher:
            var response = Convert.FromBase64String(txtEncryptedText.Text);
            
            var storedToken = new RESTAuthToken();
            storedToken.RESTAuthTokenInfo = response;

            // decrypt the message (returned as a byte array)
            txtClearText.Text = storedToken.ClearText;
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
                    m_keySize = CryptoLevel.None;                    
                    break;
                case 1:
                    m_keySize = CryptoLevel.AES128;
                    break;
                default:
                    m_keySize = CryptoLevel.AES256;
                    break;
            }
            CryptoManager.Crypto.InitializeEncryption(aesize: (CryptoLevel)cboStrength.SelectedIndex, hash: cboCryptoHash.Text);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            elevel = BitConverter.GetBytes(0);
            BitArray bits = new BitArray(elevel);

            txtClearText.Text = "Phaeglanssian";
            //byte[] nonce = new byte[] { 1, 2, 3, 4, 5, 5, 4, 3 };
            byte[] nonce = CryptoManager.Crypto.GenerateNonce(8);
            String nonceString = Encoding.ASCII.GetString(nonce);

            var hashid = cboCryptoHash.FindString(CryptoManager.Crypto.CryptoHash);
            cboCryptoHash.SelectedIndex = hashid;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            txtCryptoAESSalt.Text = CryptoConfiguration.GetAppSettings.Settings["Crypto256Salt"].Value;
            txtCryptoAESSalt.Text = CryptoConfiguration.GetAppSettings.Settings["Crypto128Salt"].Value;
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
            if (txtCryptoAESSalt.Text.Length > 0)
            {
                CryptoConfiguration.GetAppSettings.Settings["Crypto256Salt"].Value = txtCryptoAESSalt.Text;
            }
            if (txtCryptoAESSalt.Text.Length > 0)
            {
                CryptoConfiguration.GetAppSettings.Settings["Crypto128Salt"].Value = txtCryptoAESSalt.Text;
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
            txtCryptoAESSalt.Text = CryptoManager.Crypto.CryptoSalt;
            //txtCryptoAESSalt.Text = Encoding.ASCII.GetString(CryptoManager.GenerateNonce(32));
            //txtCrypto128Salt.Text = Encoding.ASCII.GetString(CryptoManager.GenerateNonce(16));
        }

        private void btnGenNonce_Click(object sender, EventArgs e)
        {
            if (numNonceLength.Value > 7)
            {
                txtNonceValue.Text = Encoding.ASCII.GetString(CryptoManager.Crypto.GenerateNonce((int)numNonceLength.Value));
            }
        }
	}
}
