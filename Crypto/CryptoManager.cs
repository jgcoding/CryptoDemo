using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Collections;
using System.Reflection;
using Newtonsoft.Json;

namespace Crypto
{
    /// <summary>
    /// There are 3 different types of encryption that are supported when communicating to a Barix device.
    /// 1. No Encryption.  Communications are sent as clear text.
    /// 2. 128-bit AES Encryption
    /// 3. 256-bit AES Encryption
    /// </summary>
    public enum CryptoLevel
    {
        [Description("No Encryption")]
        None = 0,
        [Description("128-bit AES")]
        AES128 = 1,
        [Description("256-bit AES")]
        AES256 = 2
    }

    /// <summary>
    /// Encrypts un-ciphered data and decrypts ciphered data
    /// </summary>
    /// <remarks>
    /// based on the example demonstrating how to encrypt and decrypt sample data using the Aes class on MSDN
    /// http://msdn.microsoft.com/en-us/library/system.security.cryptography.aes.aes.aspx
    /// </remarks>
    public sealed class CryptoManager
    {
        #region constructor
        private static readonly CryptoManager crypto = new CryptoManager();

        /// <summary>
        /// Static singleton constructor
        /// </summary>
        public static CryptoManager Crypto
        {
            get
            {
                return crypto;
            }
        }

        /// <summary>
        /// default constructor
        /// </summary>
        private CryptoManager()
        {
            //SetEncryptionLevel();
        }
        #endregion

        #region public properties
        /// <summary>
        /// Gets or sets the encryption level as a CryptoLevel enum
        /// </summary>
        public static CryptoLevel KeySize
        {
            get
            {
                if (m_keySize == CryptoLevel.None)
                {
                    m_keySize = CryptoLevel.AES256;
                }

                return m_keySize;
            }
            set
            {
                m_keySize = value;
                // convert the CryptoLevel to an integer to assist calculationg the crypto properties
                m_encryptionBits = m_keySize == CryptoLevel.AES256 ? 256 : m_keySize == CryptoLevel.AES128 ? 128 : 0;
                m_cryptoSalt = m_keySize == CryptoLevel.AES256 ? Encoding.UTF8.GetString(GenerateNonce(32)) : m_keySize == CryptoLevel.AES128 ? Encoding.UTF8.GetString(GenerateNonce(16)) : String.Empty;
            }
        }

        /// <summary>
        /// Gets the characters from which the nonce value is derived. The value may be privately set.
        /// </summary>
        public static Char[] ValidNonceChars
        {
            get
            {
                if (m_validNonceChars == null)
                {
                    m_validNonceChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToArray();
                }

                return m_validNonceChars;
            }
            private set
            {
                m_validNonceChars = value;
            }
        }

        /// <summary>
        /// Gets the cryptographic hash method. The value may be privately set.
        /// </summary>
        public static String CryptoHash
        {
            get
            {
                if (String.IsNullOrWhiteSpace(m_cryptoHash))
                {
                    m_cryptoHash = "SHA1";
                }

                return m_cryptoHash;
            }
            private set
            {
                m_cryptoHash = value;
            }
        }

        /// <summary>
        /// Gets the number of iterations employed in the encryption and decryption process. The value may be privately set.
        /// </summary>
        public static Int16 CryptoIterations
        {
            get
            {
                if (m_cryptoIterations < 1)
                {
                    m_cryptoIterations = 5;
                }

                return m_cryptoIterations;
            }
            private set
            {
                m_cryptoIterations = value;
            }
        }

        /// <summary>
        /// Gets the length of the nonce string to be generated. The value may be privately set.
        /// </summary>
        public static Int16 NonceLength
        {
            get
            {
                if (m_nonceLength < 1)
                {
                    m_nonceLength = 8;
                }

                return m_nonceLength;
            }
            private set
            {
                m_nonceLength = value;
                m_nonce = GenerateNonce(m_nonceLength);
            }
        }

        /// <summary>
        /// Gets the Nonce value in bytes. This value is calculated when the nonce length is privately set.
        /// </summary>
        public static byte[] Nonce
        {
            get
            {
                if (m_nonce == null)
                {
                    m_nonce = GenerateNonce(NonceLength);
                }
                return m_nonce;
            }
            private set
            {
                m_nonce = value;
            }
        }

        /// <summary>
        /// Gets the cryptographic salt string utilized in the encryption/decryption. This value is set when the AES key strength is set.
        /// </summary>
        public static String CryptoSalt
        {
            get
            {
                if (m_cryptoSalt == null)
                {
                    m_cryptoSalt = m_keySize == CryptoLevel.AES256 ? Encoding.UTF8.GetString(GenerateNonce(32)) : m_keySize == CryptoLevel.AES128 ? Encoding.UTF8.GetString(GenerateNonce(16)) : String.Empty;
                }
                return m_cryptoSalt;
            }
            private set
            {
                m_cryptoSalt = value;
            }
        }

        static String m_json;
        public static String ToJson
        {
            get
            {
                return m_json;
            }
            set
            {
                m_json = JsonConvert.SerializeObject(
                    new { 
                        TokenId = Guid.NewGuid(),
                        TokenExpiration = DateTime.UtcNow.AddMinutes(60),
                        AesStrength = KeySize.ToString(), 
                        hash = CryptoHash, 
                        iterations = CryptoIterations, 
                        salt = CryptoSalt, 
                        nonceLength = NonceLength,
                        nonce = Encoding.ASCII.GetString(Nonce), 
                        cipher = value
                    });
            }
        }

        #endregion

        #region public and private methods

        /// <summary>
        /// Loads the crypto properties for test purposes.
        /// </summary>
        static void ConfigureCryptoTest()
        {
            m_cryptoSalt = KeySize == CryptoLevel.AES256 ? "deadbeefdeadbeefdeadbeefdeadbeef" : KeySize == CryptoLevel.AES128 ? "deadbeefdeadbeef" : String.Empty;
            m_cryptoHash = "SHA1";
            m_cryptoIterations = 5;
            m_validNonceChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToArray();
            m_nonceLength = 8;
            m_nonce = Convert.FromBase64String("AQIDBAUFBAM=");
        }

        /// <summary>
        /// Sets or resets the encryption level for the encryption instance
        /// </summary>
        /// <param name="keysize">The encryption level as an CryptoLevel enum</param>
        public static void InitializeEncryption(CryptoLevel aesize = CryptoLevel.AES256, String hash = "SHA1", Int16 iterations = 5, Int16 nonceLength = 8, String salt = "", String nonce = "")
        {
            KeySize = aesize;
            m_cryptoHash = hash;
            m_cryptoIterations = iterations;
            NonceLength = nonceLength;

            // if a salt and a nonce have been provided use them:
            if (!String.IsNullOrWhiteSpace(salt) && !String.IsNullOrWhiteSpace(nonce))
            {
                Nonce = Encoding.ASCII.GetBytes(nonce);
                CryptoSalt = salt;
            }
            //otherwise; use the calculated nonce and salt values

            // initialize the IV array
            iv_array = new byte[16];

            // initialize the Key array
            key_array = new byte[m_encryptionBits / 8];

            // generate the Crypto Key and IV
            // create the key and Initial Vector from the 
            OpenSslCompatDeriveBytes crap = new OpenSslCompatDeriveBytes(CryptoSalt, Nonce, CryptoHash, CryptoIterations);

            stuff_array = crap.GetBytes((m_encryptionBits / 8) + 16);
            Buffer.BlockCopy(stuff_array, 0, key_array, 0, m_encryptionBits / 8);
            Buffer.BlockCopy(stuff_array, m_encryptionBits / 8, iv_array, 0, 16);
        }

        /// <summary>
        /// Generates a random value derived from a set of pre-defined alphanumeric characters.
        /// </summary>
        /// <param name="length">The length of the random value string</param>
        /// <returns>Returns a random value derived from a set of pre-defined alphanumeric characters</returns>
        public static byte[] GenerateNonce(Int32 length = 8)
        {
            // set the random seed
            var seed = ValidNonceChars.Count();
            var result = String.Empty;
            // construct the nonce from characters randomly selected from the permissible characters
            for (int i = 0; i < length; i++)
            {
                result += ValidNonceChars[m_random.Next(0, seed)].ToString(CultureInfo.InvariantCulture);
            }

            m_nonce = Encoding.ASCII.GetBytes(result);

            // convert the results to an ASCII encoded byte[] and return it.
            return m_nonce;
        }

        /// <summary>
        /// Encrypts the un-cyphered data as an array of bytes
        /// </summary>
        /// <param name="clearPayload">The un-ciphered data to be encrypted</param>
        /// <param name="keySize">The encryption level to be used to construct the encryption hash.</param>
        /// <returns>An ciphered array of bytes</returns>
        public static byte[] EncryptAES(byte[] clearPayload, CryptoLevel keysize = CryptoLevel.AES256)
        {
            // if the key strength requested is different from the instance
            if (KeySize != KeySize)
            {
                KeySize = keysize;
            }

            // Check arguments. 
            if (clearPayload == null || clearPayload.Length <= 0)
            {
                throw new ArgumentNullException("clearPayload");
            }
            if (key_array == null || key_array.Length <= 0)
            {
                throw new ArgumentNullException("Key");
            }
            if (iv_array == null || iv_array.Length <= 0)
            {
                throw new ArgumentNullException("Key");
            }

            // declare the output object.
            byte[] encrypted;
            // Create an Aes object 
            // with the specified key and IV. 
            using (Aes aes = Aes.Create())
            {
                aes.Key = key_array;
                aes.IV = iv_array;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(clearPayload, 0, clearPayload.Length);
                        csEncrypt.FlushFinalBlock();
                        encrypted = msEncrypt.ToArray();
                    }
                    msEncrypt.Close();
                }
            }
            // Return the encrypted bytes from the memory stream. 
            return encrypted;
        }

        /// <summary>
        /// Decrypts a cyphered key and returns it as clear text
        /// </summary>
        /// <param name="plainText">The ciphered string values in the form of a byte array</param>
        /// <param name="keySize">the encryption strength - haven't seen this make a difference yet.</param>
        /// <returns>Clear text as a string</returns>
        public static byte[] DecryptAES(byte[] cipheredPayload, CryptoLevel keysize = CryptoLevel.AES256)
        {
            // if the key strenght requested is different from the instance
            if (KeySize != KeySize)
            {
                KeySize = keysize;
            }

            // Check arguments. 
            if (cipheredPayload == null || cipheredPayload.Length <= 0)
            {
                throw new ArgumentNullException("cipheredPayload");
            }
            if (key_array == null || key_array.Length <= 0)
            {
                throw new ArgumentNullException("Key");
            }
            if (iv_array == null || iv_array.Length <= 0)
            {
                throw new ArgumentNullException("Key");
            }

            // Declare the string used to hold 
            // the decrypted text. 
            string plaintext = null;

            // Create an Aes object 
            // with the specified key and IV. 
            using (Aes aes = Aes.Create())
            {
                aes.Key = key_array;
                aes.IV = iv_array;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                // Create the streams used for decryption. 
                using (MemoryStream msDecrypt = new MemoryStream(cipheredPayload))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return Encoding.ASCII.GetBytes(plaintext);
        }

        /// <summary>
        /// Un-documented: Retrieves crypto properties from an encrypted app.config. This isn't in the specifications at this time.
        /// </summary>
        static void ConfigureCryptoFromAppConfig()
        {
            m_cryptoSalt = KeySize == CryptoLevel.AES256 ? ConfigurationManager.AppSettings["Crypto256Salt"] : KeySize == CryptoLevel.AES128 ? ConfigurationManager.AppSettings["Crypto128Salt"] : String.Empty;
            m_cryptoHash = ConfigurationManager.AppSettings["CryptoHash"];
            m_cryptoIterations = short.Parse(ConfigurationManager.AppSettings["CryptoIterations"]);
            m_validNonceChars = ConfigurationManager.AppSettings["ValidNonceChars"].ToArray();
            m_nonceLength = short.Parse(ConfigurationManager.AppSettings["NonceLength"]);
            m_nonce = Convert.FromBase64String(ConfigurationManager.AppSettings["Nonce"]);
        }

        #endregion

        #region instance fields
        static CryptoLevel m_keySize;
        static String m_cryptoSalt;
        static String m_cryptoHash;
        static Int16 m_cryptoIterations;
        static Char[] m_validNonceChars;
        static Int16 m_nonceLength;
        static byte[] m_nonce;
        static byte[] stuff_array;
        static byte[] iv_array;
        static byte[] key_array;
        static int m_encryptionBits;
        static Random m_random = new Random();
        #endregion

    }
}