using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AUTENTICADOR.src.Utils.Secutiry;

public static class SecutiryUtils {

	//!compute hash
	public static string ComputeHash(string password, HashAlgorithm hashAlgorithm) {
		byte[] inputBytes = Encoding.UTF8.GetBytes(password);
		byte[] hashedBytes = hashAlgorithm.ComputeHash(inputBytes);

		var builder = new StringBuilder();

		foreach (var item in hashedBytes) {
			builder.Append(item.ToString("x2"));
		}
		return builder.ToString();
	}

	//!decript data generated with aesAlg and a given key
	public static T Decrypt<T>(string encryptedValue, string secretKey) {
		using (Aes aesAlg = Aes.Create()) {
			aesAlg.Key = Encoding.UTF8.GetBytes(secretKey);
			aesAlg.Mode = CipherMode.CBC;
			aesAlg.Padding = PaddingMode.PKCS7;

			byte[] iv = new byte[aesAlg.BlockSize / 8];
			Array.Copy(Encoding.UTF8.GetBytes(secretKey), iv, iv.Length);
			aesAlg.IV = iv;

			ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

			using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(encryptedValue)))
			using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
			using (StreamReader srDecrypt = new StreamReader(csDecrypt)) {
				var decryptedValue = srDecrypt.ReadToEnd();
				return JsonConvert.DeserializeObject<T>(decryptedValue);
			}
		}
	}

	//!encrypt data generated with aesAlg and a given key
	public static string Encrypt<T>(T data, string secretKey) {
		using (Aes aesAlg = Aes.Create()) {
			aesAlg.Key = Encoding.UTF8.GetBytes(secretKey);
			aesAlg.Mode = CipherMode.CBC;
			aesAlg.Padding = PaddingMode.PKCS7;

			byte[] iv = new byte[aesAlg.BlockSize / 8];
			Array.Copy(Encoding.UTF8.GetBytes(secretKey), iv, iv.Length);
			aesAlg.IV = iv;

			ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

			using (MemoryStream msEncrypt = new MemoryStream())
			using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write)) {
				using (StreamWriter swEncrypt = new StreamWriter(csEncrypt)) {
					string jsonData = JsonConvert.SerializeObject(data);
					swEncrypt.Write(jsonData);
				}

				byte[] encryptedBytes = msEncrypt.ToArray();
				return Convert.ToBase64String(encryptedBytes);
			}
		}
	}
}
