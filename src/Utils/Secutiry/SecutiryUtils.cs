using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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
}
