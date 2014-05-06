using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Crypto
{
    public class RESTAuthToken
    {
        public Guid RESTTransactionId { get; set; }
        public byte[] RESTAuthTokenInfo { get; set; }
        public DateTime RESTExpiration { get; set; }

        /// <summary>
        /// Returns the decrypted value of the ciphered message
        /// </summary>
        public String ClearText
        {
            get
            {
                // convert the byte array back into a string
                var bson = Encoding.ASCII.GetString(this.RESTAuthTokenInfo);

                // parse the json string containing the ciphered message
                var json = JObject.Parse(bson);

                // extract the AES encryption strength
                var keysize = (CryptoLevel)Enum.Parse(typeof(CryptoLevel), json.SelectToken("aesStrength").ToString());

                // extract the hashing mechanism
                var hash = json.SelectToken("hash").ToString();

                // extract the number of SSL hashing iterations used to encrypt the message
                var iterations = (Int16)json.SelectToken("iterations");

                // extract the length of the nonce value used to encrypt the message
                var nonceLength = (Int16)json.SelectToken("nonceLength");

                // extract the salt
                var salt = json.SelectToken("salt").ToString();

                // extract the nonce value
                var nonce = json.SelectToken("nonce").ToString();

                // extract the ciphered message stored
                var m_cipherString = json.SelectToken("cipher").ToString();

                // extract the REST transaction id
                RESTTransactionId = Guid.Parse(json.SelectToken("_id").ToString());

                //extract the token's expiration
                RESTExpiration = DateTime.Parse(json.SelectToken("expiration").ToString());

                // initialize a crypto configuration for this particular cipher:
                CryptoManager.Crypto.InitializeEncryption(keysize, hash, iterations, nonceLength, salt, nonce);

                // decrypt the message (returned as a byte array)
                byte[] response = CryptoManager.Crypto.DecryptAES(Convert.FromBase64String(m_cipherString), keysize);

                // translate the byte array response into readable text:
                var m_cleartext = Encoding.ASCII.GetString(response);

                // return the un-ciphered message
                return m_cleartext;
            }
        }
    }
}
