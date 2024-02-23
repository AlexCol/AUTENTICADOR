using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AUTENTICADOR.src.Model.Entities;
using AUTENTICADOR.src.ValueObjects;

namespace AUTENTICADOR.src.Repository.UserRepository;

public interface IUserRepository
{
	User ValidadeCredentials(UserLoginVO user);
	User ValidadeCredentials(long id);
	bool RevokeToken(long Id);
}
