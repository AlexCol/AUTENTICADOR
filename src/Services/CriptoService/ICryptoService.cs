using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace AUTENTICADOR.src.Services.CryptoService;

public interface ICryptoService {
	public string ComputeHash(string password, HashAlgorithm hashAlgorithm);
	public T Decrypt<T>(string encryptedValue);
	public string Decrypt(string encryptedValue);
	public string Encrypt<T>(T data);
}
