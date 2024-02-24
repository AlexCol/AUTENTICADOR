using System.Security.Cryptography;
using AUTENTICADOR.src.Repository.UserRepository;

namespace AUTENTICADOR.src.Extensions.toUserRepo;

public static class UserRepoExtension {
	public static string RegenActivationToken(this IUserRepository repo) {
		byte[] randomNumber = new byte[32];
		string activationCode;
		do {
			using (var rng = RandomNumberGenerator.Create()) {
				rng.GetBytes(randomNumber);
				activationCode = Convert.ToBase64String(randomNumber);
				activationCode = activationCode.Replace("+", ".");
			}
		} while (repo.FindByActivationToken(activationCode) != null);
		return activationCode;
	}
}
