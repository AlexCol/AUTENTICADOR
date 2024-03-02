using AUTENTICADOR.src.Model.Entities;
using AUTENTICADOR.src.Repository.GenericRepository;
using AUTENTICADOR.src.ValueObjects;

namespace AUTENTICADOR.src.Repository.UserRepository;

public interface IUserRepository : IGenericRepository<User> {
	User ValidadeCredentials(UserLoginVO user);
	User FindByEmail(string email);
	User FindByActivationToken(string token);
}