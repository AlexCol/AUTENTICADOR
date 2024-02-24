using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AUTENTICADOR.src.Model.Entities;
using AUTENTICADOR.src.Repository.GenericRepository;
using AUTENTICADOR.src.ValueObjects;

namespace AUTENTICADOR.src.Repository.UserRepository;

public interface IUserRepository : IGenericRepository<User> {
	User ValidadeCredentials(UserLoginVO user);
	User ValidadeCredentials(Guid id);
	bool RevokeToken(Guid Id);
	User FindByEmail(string email);
}