using System.Security.Cryptography;
using AUTENTICADOR.src.Model;
using AUTENTICADOR.src.Model.Entities;
using AUTENTICADOR.src.Repository.GenericRepository;
using AUTENTICADOR.src.ValueObjects;

namespace AUTENTICADOR.src.Repository.UserRepository;

public class UserRepository : GenericRepository<User>, IUserRepository {
	public UserRepository(PostgreContext context) : base(context) { }

	public User ValidadeCredentials(UserLoginVO user) {
		return _context.Users.FirstOrDefault(u => (u.Email == user.Email) && (u.Password == user.Password)); //!senha deve vir tratada do service
	}

	public User FindByEmail(string email) {
		return _context.Users.SingleOrDefault(u => u.Email == email);
	}

	public User FindByActivationToken(string token) {
		return _context.Users.SingleOrDefault(u => u.ActivationToken == token);
	}
}
