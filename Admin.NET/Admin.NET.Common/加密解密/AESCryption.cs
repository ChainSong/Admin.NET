 
using System; 
using System.Text; 
using System.Security.Cryptography;
namespace Common.Utility
{ 
	/// <summary> 
	/// RSA���ܽ��ܼ�RSAǩ������֤
	/// </summary> 
	public  class AESCryption
    {
        private static string key = "1234567890123456"; // 16�ֽ���Կ��AES-128
        private static string iv = "1234567890123456";  // 16�ֽ�IV����ʼ��������

        // ���ܺ���
        public static string Encrypt(string plainText)
        {
            using (Aes aesAlg = Aes.Create()) // ����AES����ʵ��
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);  // ������Կ
                aesAlg.IV = Encoding.UTF8.GetBytes(iv);    // ���ó�ʼ������

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV); // ����������

                using (MemoryStream msEncrypt = new MemoryStream())  // ���ڴ洢���ܺ������
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))  // ��������������
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))  // д�������
                        {
                            swEncrypt.Write(plainText);  // д����������
                        }
                    }
                    // ���ؼ��ܺ�����ݣ�ת��ΪBase64�ַ���
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        // ���ܺ���
        public static string Decrypt(string cipherText)
        {
            using (Aes aesAlg = Aes.Create()) // ����AESʵ��
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);  // ������Կ
                aesAlg.IV = Encoding.UTF8.GetBytes(iv);    // ���ó�ʼ������

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);  // ����������

                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))  // ��Base64�ַ���ת��Ϊ�ֽ����鲢��ȡ
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))  // ��������������
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))  // �ӽ������ж�ȡ����
                        {
                            return srDecrypt.ReadToEnd();  // ���ؽ��ܺ������
                        }
                    }
                }
            }
        }

	} 
} 
