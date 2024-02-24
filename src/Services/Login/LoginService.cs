using System.Security.Claims;
using AUTENTICADOR.src.Model.Token;
using AUTENTICADOR.src.Services.TokenService;
using AUTENTICADOR.src.Services.UserService;
using AUTENTICADOR.src.ValueObjects;

namespace AUTENTICADOR.src.Services.Login;

public class LoginService : ILoginService {

	private const string DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";
	private TokenModel _tokenModel;
	private readonly ITokenService _tokenService;
	private IUserService _userService;

	public LoginService(TokenModel tokenModel, IUserService userService, ITokenService tokenService) {
		_tokenModel = tokenModel;
		_userService = userService;
		_tokenService = tokenService;
	}

	public AuthVO ValidadeCredentials(UserLoginVO userCredentials) {
		var user = _userService.ValidadeCredentials(userCredentials);
		if (user == null) return new AuthVO(null, null, "Usuário ou senha invalidos.");

		if (!user.Activated) return new AuthVO(null, null, "Usuário inativo. Verificar seu email e clicar no link para ativação da conta.");

		var claims = new List<Claim>() {
						new Claim("UserId", user.id.ToString())
				};
		var accessToken = _tokenService.GenerateAccesToken(claims);
		var refreshToken = _tokenService.GenerateRefreshToken();
		user.RefreshToken = refreshToken;
		user.RefreshTokenExpiryTime = DateTime.Now.ToLocalTime().AddDays(_tokenModel.DaysToExpire);

		_userService.Update(user);

		return new AuthVO(
			accessToken,
			refreshToken,
			null
		);
	}

	public AuthVO ValidadeCredentials(AuthVO token) {
		var accessToken = token.AccessToken;
		var refreshToken = token.RefreshToken;
		var claims = _tokenService.GetClaimsFromExpiredToken(accessToken);

		var idClaim = claims.FirstOrDefault(c => c.Type == "UserId");
		if (idClaim == null) return new AuthVO(null, null, "Id do usuário não encontrado.");

		Guid.TryParse(idClaim.Value, out Guid id);
		var user = _userService.ValidadeCredentials(id);

		if (
			user == null || //usuário não encontrado
			user.RefreshToken != refreshToken || //refresh token invalido
			user.RefreshTokenExpiryTime <= DateTime.Now) //data do refreshtoken expirada
		{
			return new AuthVO(null, null, "Favor realizar novo login.");
		}

		accessToken = _tokenService.GenerateAccesToken(claims);
		refreshToken = _tokenService.GenerateRefreshToken();

		user.RefreshToken = refreshToken;
		_userService.Update(user);

		return new AuthVO(
			accessToken,
			refreshToken,
			null
		);
	}

	public bool RevokeToken(Guid id) {
		return _userService.RevokeToken(id);
	}
}