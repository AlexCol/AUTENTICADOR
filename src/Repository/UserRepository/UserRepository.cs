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

namespace AUTENTICADOR.src.Repository.UserRepository;

public class UserRepository : GenericRepository<User>, IUserRepository
{
	private IMapper _mapper;

	public UserRepository(PostgreContext context, IMapper mapper) : base(context)
	{
		_mapper = mapper;
	}


	public User ValidadeCredentials(UserLoginVO user)
	{
		var pass = ComputeHash(user.Password, SHA256.Create());
		return _context.Users.FirstOrDefault(u => (u.Email == user.Email) && (u.Password == pass));
	}

	public User ValidadeCredentials(long id)
	{
		return _context.Users.SingleOrDefault(u => u.Id == id);
	}

	public bool RevokeToken(long Id)
	{
		var user = _context.Users.SingleOrDefault(u => u.Id == Id);
		if (user == null) return false;

		user.RefreshToken = null;
		_context.SaveChanges();
		return true;
	}

	private string ComputeHash(string password, HashAlgorithm hashAlgorithm)
	{
		byte[] inputBytes = Encoding.UTF8.GetBytes(password);
		byte[] hashedBytes = hashAlgorithm.ComputeHash(inputBytes);

		var builder = new StringBuilder();

		foreach (var item in hashedBytes)
		{
			builder.Append(item.ToString("x2"));
		}
		return builder.ToString();
	}
}
