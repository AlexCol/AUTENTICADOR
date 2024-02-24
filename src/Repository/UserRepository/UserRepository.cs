using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AUTENTICADOR.src.Model;
using AUTENTICADOR.src.Model.Entities;
using AUTENTICADOR.src.Repository.GenericRepository;
using AUTENTICADOR.src.ValueObjects;
using AutoMapper;
using Serilog;

namespace AUTENTICADOR.src.Repository.UserRepository;

public class UserRepository : GenericRepository<User>, IUserRepository {
	public UserRepository(PostgreContext context) : base(context) { }

	public User ValidadeCredentials(UserLoginVO user) {
		return _context.Users.FirstOrDefault(u => (u.Email == user.Email) && (u.Password == user.Password)); //!senha deve vir tratada do service
	}

	public User ValidadeCredentials(Guid id) {
		return _context.Users.SingleOrDefault(u => u.id == id);
	}

	public bool RevokeToken(Guid Id) {
		var user = _context.Users.SingleOrDefault(u => u.id == Id);
		if (user == null) return false;

		user.RefreshToken = null;
		_context.SaveChanges();
		return true;
	}

	public User FindByEmail(string email) {
		return _context.Users.SingleOrDefault(u => u.Email == email);
	}
}
