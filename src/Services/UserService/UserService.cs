using System.Security.Cryptography;
using System.Text;
using AUTENTICADOR.src.Model.Entities;
using AUTENTICADOR.src.Repository.UserRepository;
using AUTENTICADOR.src.Services.Generic;
using AUTENTICADOR.src.ValueObjects;

namespace AUTENTICADOR.src.Services.UserService;

public class UserService : GenericService<User>, IUserService {
	private IUserRepository _userRepository;
	public UserService(IUserRepository repository) : base(repository) {
		_userRepository = repository;
	}

	public bool RevokeToken(Guid Id) {
		return _userRepository.RevokeToken(Id);
	}

	public User ValidadeCredentials(UserLoginVO user) {
		user.Password = ComputeHash(user.Password, SHA256.Create());
		return _userRepository.ValidadeCredentials(user);
	}

	public User ValidadeCredentials(Guid id) {
		return _userRepository.ValidadeCredentials(id);
	}

	override public User Create(User user) {
		var emailUser = _userRepository.FindByEmail(user.Email);
		if (emailUser != null) throw new Exception("Email já cadastrado!");

		user.Password = ComputeHash(user.Password, SHA256.Create());

		var randomNumber = new byte[32];
		using (var rng = RandomNumberGenerator.Create()) {
			rng.GetBytes(randomNumber);
			var activateCode = Convert.ToBase64String(randomNumber);
			user.RefreshToken = activateCode;
		}
		return _userRepository.Create(user);
	}

	private string ComputeHash(string password, HashAlgorithm hashAlgorithm) {
		byte[] inputBytes = Encoding.UTF8.GetBytes(password);
		byte[] hashedBytes = hashAlgorithm.ComputeHash(inputBytes);

		var builder = new StringBuilder();

		foreach (var item in hashedBytes) {
			builder.Append(item.ToString("x2"));
		}
		return builder.ToString();
	}
}
