using AUTENTICADOR.src.ValueObjects;

namespace AUTENTICADOR.src.Services.UserService;

public interface IUserService {
	public UserResponseVO FindById(Guid id);
	public List<UserResponseVO> FindAll();
	public UserResponseVO Create(UserRequestVO user);
	public UserResponseVO Update(UserRequestVO user);
	public void Delete(Guid id);
}