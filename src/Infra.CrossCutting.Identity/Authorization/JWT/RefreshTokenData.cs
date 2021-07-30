using System;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.DataProtection;

namespace Infra.CrossCutting.Identity.Authorization.JWT
{
    public class RefreshTokenData
    {

        #region Variables

        private const string _salt = "I1vZ/c9ldK/U67sR/EhUbMavVW0hZId6x6VxlaeTahI=";
        private string _refreshToken;
        private string _userID;
        private string _userName;
        private string _userPassword;
        private string _sistemID;
        //private readonly DateTime _issueUtc;
        //private readonly DateTime _expiresUtc;
        //private readonly string _protectedTicket;

        #endregion

        #region Properties

        public string RefreshToken { get => _refreshToken; }
        public string UserID { get => _userID; }
        public string UserName { get => _userName; }
        public string UserPassword { get => _userPassword; }
        public string SistemID { get => _sistemID; }
        //public DateTime IssueUtc { get => _issueUtc; }
        //public DateTime ExpiresUtc { get => _expiresUtc; }
        //public string ProtectedTicket { get => _protectedTicket; }

        #endregion

        #region Constructors

        private RefreshTokenData() { }

        #endregion

        #region Methods

        public static string CreateRefreshToken(string userName)
        {
            byte[] userNameByteArrray = Encoding.ASCII.GetBytes(userName);
            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();
            string token = Convert.ToBase64String(time.Concat(key).Concat(userNameByteArrray).ToArray());

            return token;
        }

        #endregion

        #region Factories

        public static class Factory
        {

            public static RefreshTokenData CreateToken(string refreshToken,
                                                       string userID,
                                                       string userName,
                                                       string userPassword,
                                                       string sistemID)
            {
                var token = new RefreshTokenData()
                {
                    _refreshToken = refreshToken,
                    _userID = userID,
                    _userName = userName,
                    _userPassword = Protect.Enrypt(userPassword, _salt),
                    _sistemID = sistemID,
                };

                return token;
            }

            public static RefreshTokenData DecryptJsonToken(string jsonRefreshTokenData)
            {
                if (string.IsNullOrWhiteSpace(jsonRefreshTokenData))
                {
                    return null;
                }

                var json = JsonConvert.DeserializeObject<dynamic>(jsonRefreshTokenData);

                var refreshTokenData = new RefreshTokenData()
                {
                    _refreshToken = json.RefreshToken,
                    _userID = json.UserID,
                    _userName = json.UserName,
                    _userPassword = json.UserPassword,
                    _sistemID = json.SistemID,
                };

                if (!string.IsNullOrWhiteSpace(refreshTokenData._userPassword))
                    refreshTokenData._userPassword = Protect.Derypt(refreshTokenData.UserPassword, _salt);

                return refreshTokenData;
            }
        }

        #endregion
    }

    internal static class Protect
    {

        public static string Enrypt(string encryptString, string encryptionKey)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(encryptString);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] {
                    0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
                });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    encryptString = Convert.ToBase64String(ms.ToArray());
                }
            }
            return encryptString;
        }

        public static string Derypt(string decryptText, string encryptionKey)
        {
            decryptText = decryptText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(decryptText);
            using (Aes encryptor = Aes.Create())
            {

                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] {
                    0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
                });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    decryptText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return decryptText;
        }

    }

}
