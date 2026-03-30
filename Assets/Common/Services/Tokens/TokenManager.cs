using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Common.Services.Tokens
{
    public class TokenManager
    {
        private static string accessToken;
        private static string refreshToken;

        private static readonly string filePath = Path.Combine(Application.persistentDataPath, "tokens.dat");

        private static readonly byte[] key = Encoding.UTF8.GetBytes("1234567890123456"); // 16 байт
        private static readonly byte[] iv = Encoding.UTF8.GetBytes("6543210987654321");  // 16 байт

        public static string AccessToken => accessToken;
        public static string RefreshToken => refreshToken;


        public static void SetTokens(string access, string refresh)
        {
            accessToken = access;
            refreshToken = refresh;
            SaveTokensToDisk();
        }

        public static void ClearTokens()
        {
            accessToken = null;
            refreshToken = null;
            if (File.Exists(filePath))
                File.Delete(filePath);
        }

        public static void LoadTokensFromDisk()
        {
            if (!File.Exists(filePath))
                return;

            try
            {
                byte[] encrypted = File.ReadAllBytes(filePath);
                string json = Decrypt(encrypted);
                TokenData data = JsonUtility.FromJson<TokenData>(json);
                accessToken = data.accessToken;
                refreshToken = data.refreshToken;
            }
            catch (Exception e)
            {
                Debug.LogWarning("Не удалось загрузить токены: " + e.Message);
            }
        }

        private static void SaveTokensToDisk()
        {
            try
            {
                TokenData data = new TokenData
                {
                    accessToken = accessToken,
                    refreshToken = refreshToken
                };
                string json = JsonUtility.ToJson(data);
                byte[] encrypted = Encrypt(json);
                File.WriteAllBytes(filePath, encrypted);
            }
            catch (Exception e)
            {
                Debug.LogWarning("Не удалось сохранить токены: " + e.Message);
            }
        }


        private static byte[] Encrypt(string plainText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream ms = new MemoryStream())
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                using (StreamWriter sw = new StreamWriter(cs))
                {
                    sw.Write(plainText);
                    sw.Close();
                    return ms.ToArray();
                }
            }
        }

        private static string Decrypt(byte[] cipherText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using (MemoryStream ms = new MemoryStream(cipherText))
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (StreamReader sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }


        [Serializable]
        private class TokenData
        {
            public string accessToken;
            public string refreshToken;
        }
    }
}
