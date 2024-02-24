using AUTENTICADOR.src.Model.Entities;
using AUTENTICADOR.src.Services.Generic;
using AUTENTICADOR.src.ValueObjects;

namespace AUTENTICADOR.src.Services.UserService;

public interface IUserService : IGenericService<User> {
	User ValidadeCredentials(UserLoginVO user);
	User ValidadeCredentials(Guid id);
	bool RevokeToken(Guid Id);
}