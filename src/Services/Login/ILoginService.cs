using AUTENTICADOR.src.ValueObjects;

namespace AUTENTICADOR.src.Services.Login;

public interface ILoginService {
	AuthVO ValidadeCredentials(UserLoginVO userCredentials);
	AuthVO ValidadeCredentials(AuthVO token);
	bool RevokeToken(Guid id);
	public UserResponseVO ActivateUser(string token);
	public void RefreshActivationToken(string email, bool recoveringPass = false);
	public void ResetPassword(string token, string password);
}
