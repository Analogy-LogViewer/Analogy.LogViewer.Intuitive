#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
using Analogy.LogViewer.Intuitive.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.Versioning;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Xml;

namespace MediaManager.Logging
{
    [ExcludeFromCodeCoverage]
#if NET
    [SupportedOSPlatform("windows")]
#endif
    public class EncryptionLogic
    {
#pragma warning disable SA1132
        internal const int IvLengthByte = 16, EncKeyLengthByte = 128;
#pragma warning restore SA1132
#pragma warning disable SYSLIB0021
        private AesCryptoServiceProvider AesEncryptorService { get; }
#pragma warning restore SYSLIB0021
        private ICryptoTransform Encryptor { get; set; }
        private ICryptoTransform? Decryptor { get; set; }
        private string KeyContainerNameRsa { get; } //can store only full key, public keys are not stored in containers

        // for key encryption
        private CspParameters rsaCspParams = null!;
        private RSACryptoServiceProvider rsaEncryptor = null!;
#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value
        private bool fOaep; //for padding
#pragma warning restore CS0649 // Field is never assigned to, and will always have its default value

        private SerilogEncryptedLogFileMode loggerMode;
        private bool encryptionKeyIsValid;
        private static string ErrorMessage { get; } = "Invalid or Missing Public key for Encryption";

        public EncryptionLogic(SerilogEncryptedLogFileMode mode, string containerKey = "OrpheusFullKeyContainerRSA")
        {
            this.KeyContainerNameRsa = containerKey;
            this.loggerMode = mode;

#pragma warning disable SYSLIB0021
            AesEncryptorService = new AesCryptoServiceProvider()
#pragma warning restore SYSLIB0021
            {
                Mode = CipherMode.CBC,
                BlockSize = 128, //16 bytes
                KeySize = 256, //32 bytes
                Padding = PaddingMode.PKCS7,  //completes the final block to BlockSize
            };
            Encryptor = AesEncryptorService.CreateEncryptor(); //IMPORTANT!! this should go AFTER generate of key & IV, otherwise works on PREVIOUS KEY & IV!!!

            switch (mode)
            {
                case SerilogEncryptedLogFileMode.WriteLogs:
                    InitMainCryptoProviderRsa(useCsp: false, useOnlyCsPstoredKey: false, preventKeyExport: false);
                    break;
                case SerilogEncryptedLogFileMode.ReadLogs:
                    InitMainCryptoProviderRsa(useCsp: true, useOnlyCsPstoredKey: true, preventKeyExport: false);
                    break;
                case SerilogEncryptedLogFileMode.Maintenance:
                    InitMainCryptoProviderRsa(useCsp: true, useOnlyCsPstoredKey: false, preventKeyExport: false);
                    break;
                default:
                    throw new InvalidEnumArgumentException(nameof(mode), (int)mode, mode.GetType());
            }
        }

        /// <summary>
        /// Asymmetric for key encryption
        /// </summary>
        /// <param name="useCsp">Uses CSP - only for private key</param>
        /// <param name="useOnlyCsPstoredKey"></param>
        /// <param name="preventKeyExport">Disable exporting private key from CSP</param>
        /// <returns>true on success, false on fail</returns>
        private bool InitMainCryptoProviderRsa(bool useCsp, bool useOnlyCsPstoredKey, bool preventKeyExport)
        {
            try
            {
                this.rsaCspParams = new CspParameters();
                if (useCsp || useOnlyCsPstoredKey)
                {
                    rsaCspParams.KeyContainerName = KeyContainerNameRsa;
                }
                this.rsaCspParams.Flags |= CspProviderFlags.UseMachineKeyStore; //set the CSP storage to machine-level (the default is user level!!)
                this.rsaCspParams.KeyNumber = (int)KeyNumber.Exchange;

                SecurityIdentifier identifier = new SecurityIdentifier(WellKnownSidType.WorldSid, domainSid: null);
                string role = identifier.Translate(typeof(NTAccount)).Value;
#if !NET
                //Grant access to all users. Apparently permissions can be changed only by the user, who has full access to the CSP file (?)
                CryptoKeyAccessRule rule = new CryptoKeyAccessRule(role, CryptoKeyRights.FullControl, AccessControlType.Allow);
                rsaCspParams.CryptoKeySecurity = new CryptoKeySecurity();
                rsaCspParams.CryptoKeySecurity.SetAccessRule(rule);
#endif
                this.rsaEncryptor = new RSACryptoServiceProvider(rsaCspParams);
                this.rsaEncryptor.PersistKeyInCsp = useCsp; //if set to false, will delete key from CSP on app close!!
                this.rsaEncryptor.ExportParameters(preventKeyExport);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ImportPublicKeyFromString(string key)
        {
            try
            {
                if (!string.IsNullOrEmpty(key) && ValidateKeyStringXmLrsa(key))
                {
                    this.rsaEncryptor.PersistKeyInCsp = false;
                    this.rsaEncryptor.FromXmlString(key);
                    Encryptor = AesEncryptorService.CreateEncryptor(); //IMPORTANT!! this should go AFTER generate of key & IV, otherwise works on PREVIOUS KEY & IV!!!
                    encryptionKeyIsValid = true;
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool ValidateKeyStringXmLrsa(string key)
        {
            try
            {
                XmlDocument document = new XmlDocument();
                document.LoadXml(key);

                XmlReader reader = XmlReader.Create(new StringReader(document.InnerXml));
                while (reader.Read())
                {
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string EncryptLogEntry(string logEntry)
        {
            if (!encryptionKeyIsValid)
            {
                return ErrorMessage;
            }
            try
            {
                List<byte> encLogEntryBytesList = new List<byte>();
                var logEntryBytes = Encoding.UTF8.GetBytes(logEntry);
                var logEntryEnc = Encryptor.TransformFinalBlock(logEntryBytes, 0, logEntryBytes.Length);
                encLogEntryBytesList.AddRange(logEntryEnc);
                byte[] encLogEntryBytes = encLogEntryBytesList.ToArray();
                return Convert.ToBase64String(encLogEntryBytes);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Generates & exports a random pair of keys (private + public & public escaped) to a specified folder, overwrites if files exist
        /// </summary>
        /// <param name="folderPath">Folder to generate keys into</param>
        /// <param name="keyContainerName"></param>
        /// <returns>true on success, false on fail</returns>
        public bool GenerateAndExporKeyXmlToFileRsa(string folderPath, string keyContainerName = "OrpheusGenerateTemp")
        {
            try
            {
                CspParameters rsaCspParamsForGenerate = rsaCspParamsForGenerate = new CspParameters
                {
                    KeyContainerName = keyContainerName, //to ensure not to override current keys
                };

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                RSACryptoServiceProvider rsaEncryptorForGenerate = new RSACryptoServiceProvider(rsaCspParamsForGenerate);
                rsaEncryptorForGenerate.PersistKeyInCsp = false; //we do not want to store the key on the machine!!

                //private
                string key = rsaEncryptorForGenerate.ToXmlString(includePrivateParameters: true);
                string keyFilePath = Path.Combine(folderPath, "full-key.txt");
                File.WriteAllText(keyFilePath, key);

                //public
                key = rsaEncryptorForGenerate.ToXmlString(includePrivateParameters: false);
                keyFilePath = Path.Combine(folderPath, "public-key.txt");
                File.WriteAllText(keyFilePath, key);

                //public encoded for settings
                string keyEscaped = System.Net.WebUtility.HtmlEncode(key);
                keyFilePath = Path.Combine(folderPath, "public-key-for-settings.txt");
                File.WriteAllText(keyFilePath, keyEscaped);

                rsaEncryptorForGenerate.Dispose();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Imports key to CSP container from XML file
        /// </summary>
        /// <param name="filePath">Key file path</param>
        /// <param name="overrideCurrentKey">Import on runtime if true or after app closes</param>
        /// <returns>"true" on success or exception details on failure</returns>
        public bool ImportPrivateKeyFromFile(string filePath, bool overrideCurrentKey)
        {
            string key = File.ReadAllText(filePath);
            return ImportPrivateKey(key, overrideCurrentKey);
        }

        /// <summary>
        /// Imports key to CSP container from XML file
        /// </summary>
        /// <param name="key">Key data</param>
        /// <param name="overrideCurrentKey">Import on runtime if true or after app closes</param>
        /// <returns>"true" on success or exception details on failure</returns>
        public bool ImportPrivateKey(string key, bool overrideCurrentKey)
        {
            try
            {
                if (!string.IsNullOrEmpty(key))
                {
                    if (ValidateKeyStringXmLrsa(key))
                    {
                        CspParameters tempCsPparams = new CspParameters { KeyContainerName = KeyContainerNameRsa };
                        RSACryptoServiceProvider tempRsa = new RSACryptoServiceProvider(rsaCspParams);
                        tempRsa.FromXmlString(key);
                        tempRsa.PersistKeyInCsp = true;
                        if (overrideCurrentKey)
                        {
                            if (this.rsaEncryptor is null)
                            {
                                throw new Exception("exception in function ImportPrivateKeyRSA: rsaEncryptor is null;");
                            }
                            this.rsaEncryptor.FromXmlString(key);
                        }
                        return true;
                    }
                    return false;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private bool GetIvAndKey(byte[] encrLogLine, out byte[] iv, out byte[] aesKey)
        {
            byte[] vtemp = new byte[IvLengthByte];
            byte[] keyEnc = new byte[EncKeyLengthByte];

            try
            {
                Array.Copy(encrLogLine, 0, vtemp, 0, IvLengthByte);
                Array.Copy(encrLogLine, IvLengthByte, keyEnc, 0, EncKeyLengthByte);

                iv = vtemp;
                aesKey = rsaEncryptor.Decrypt(keyEnc, fOaep);
                return true;
            }
            catch (Exception)
            {
                iv = null;
                aesKey = null;
                return false;
            }
        }

        public string GetEncryptionData()
        {
            List<byte> encLogEntryBytesList = new List<byte>();
            byte[] logEntryBytes = Encoding.UTF8.GetBytes("");
            byte[] logEntryEnc = Encryptor.TransformFinalBlock(logEntryBytes, 0, logEntryBytes.Length);

            //add IV + encKey only to the first row
            byte[] aesKeyEncrypted = this.rsaEncryptor.Encrypt(AesEncryptorService.Key, fOaep); //symmetric for data
            encLogEntryBytesList.AddRange(AesEncryptorService.IV); //unencrypted
            encLogEntryBytesList.AddRange(aesKeyEncrypted);
            encLogEntryBytesList.AddRange(logEntryEnc);
            var encrypted = Convert.ToBase64String(encLogEntryBytesList.ToArray());
            return encrypted;
        }

        public bool TryGetCurrentKey(bool includePrivateKey, out string key)
        {
            try
            {
                key = rsaEncryptor.ToXmlString(includePrivateKey);
                return true;
            }
            catch (Exception)
            {
                key = "";
                return false;
            }
        }

        /// <summary>
        /// Exports current key in memory (public/private) to a specified folder
        /// </summary>
        /// <param name="pathFolder">Folder to export key into</param>
        /// <param name="includePrivateKey">if false, export public only</param>
        /// <returns>true on success, false on fail</returns>
        public bool ExportCurrentKeyXmlToFileRsa(string pathFolder, bool includePrivateKey)
        {
            try
            {
                if (!Directory.Exists(pathFolder))
                {
                    Directory.CreateDirectory(pathFolder);
                }
                string key = rsaEncryptor.ToXmlString(includePrivateKey);
                string keyFilePath = Path.Combine(pathFolder, (includePrivateKey ? "curr-full-key" : "curr-public-key") + ".txt");
                File.WriteAllText(keyFilePath, key);
                if (!includePrivateKey)
                {
                    //public encoded for settings
                    string keyEscaped = System.Net.WebUtility.HtmlEncode(key);
                    keyFilePath = Path.Combine(pathFolder, "public-key-for-settings.txt");
                    File.WriteAllText(keyFilePath, keyEscaped);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Deletes key from CSP. Does not delete current key from memory - effect after app closed
        /// </summary>
        /// <returns>true on success, false on fail</returns>
        public bool DeleteKeysFromContainerRsa()
        {
            try
            {
                CspParameters cspParams = new()
                {
                    KeyContainerName = KeyContainerNameRsa,
                    Flags = CspProviderFlags.UseMachineKeyStore,
                };
                using RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(cspParams);
                rsa.PersistKeyInCsp = false;
                rsa.Clear();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CreateDecryptor(string decryptData)
        {
            try
            {
                byte[] encLineBytes = Convert.FromBase64String(decryptData);
                var result = GetIvAndKey(encLineBytes, out var iv, out var aesKey);
                Decryptor = AesEncryptorService.CreateDecryptor(aesKey, iv);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string Decrypt(string encryptedText)
        {
            try
            {
                var encLineBytes = Convert.FromBase64String(encryptedText);
                var dataLength = encLineBytes.Length;
                var encData = new byte[dataLength];
                Array.Copy(encLineBytes, 0, encData, 0, dataLength);
                byte[] decryptedBytes = Decryptor?.TransformFinalBlock(encData, 0, encData.Length) ?? [];
                var logEntry = Encoding.UTF8.GetString(decryptedBytes);
                return logEntry;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}