using System.Security.Claims;
using System.Security.Cryptography;
using AUTENTICADOR.src.Extensions.toFluntNotifications;
using AUTENTICADOR.src.Extensions.toUserRepo;
using AUTENTICADOR.src.Model.Entities;
using AUTENTICADOR.src.Model.Error;
using AUTENTICADOR.src.Model.Token;
using AUTENTICADOR.src.Repository.UserRepository;
using AUTENTICADOR.src.Services.TokenService;
using AUTENTICADOR.src.Utils.Secutiry;
using AUTENTICADOR.src.ValueObjects;
using AutoMapper;
using Serilog;

namespace AUTENTICADOR.src.Services.Login;

public class LoginService : ILoginService {

	private const string DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";
	private readonly TokenModel _tokenModel;
	private readonly ITokenService _tokenService;
	private readonly IUserRepository _userRepository;
	private readonly IMapper _mapper;

	public LoginService(TokenModel tokenModel, IUserRepository userRepository, ITokenService tokenService, IMapper mapper) {
		_tokenModel = tokenModel;
		_userRepository = userRepository;
		_tokenService = tokenService;
		_mapper = mapper;
	}

	public AuthVO ValidadeCredentials(UserLoginVO userCredentials) {
		if (userCredentials == null) return new AuthVO(null, null, "Não enviados dados para autenticação.");

		userCredentials.Password = SecutiryUtils.ComputeHash(userCredentials.Password, SHA256.Create());
		var user = _userRepository.ValidadeCredentials(userCredentials);
		if (user == null) return new AuthVO(null, null, "Usuário ou senha invalidos.");

		if (!user.Activated) return new AuthVO(null, null, "Usuário inativo. Verificar seu email e clicar no link para ativação da conta.");

		var claims = new List<Claim>() {
						new Claim("UserId", user.id.ToString())
				};
		var accessToken = _tokenService.GenerateAccesToken(claims);
		var refreshToken = _tokenService.GenerateRefreshToken();
		user.RefreshToken = refreshToken;
		user.RefreshTokenExpiryTime = DateTime.Now.ToLocalTime().AddDays(_tokenModel.DaysToExpire);

		_userRepository.Update(user);

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
		var user = _userRepository.FindById(id);

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
		_userRepository.Update(user);

		return new AuthVO(
			accessToken,
			refreshToken,
			null
		);
	}

	public bool RevokeToken(Guid id) {
		return _userRepository.RevokeToken(id);
	}

	//!activate user
	public UserResponseVO ActivateUser(string token) {
		var user = _userRepository.FindByActivationToken(token);
		Log.Error(token);

		if (user == null) throw new Exception("Link Inválido");

		user.Activated = true;
		user.ActivationToken = null;
		return _mapper.Map<UserResponseVO>(_userRepository.Update(user));
	}

	//!refresh activation token
	public void RefreshActivationToken(string email, bool recoveringPass = false) {
		var user = _userRepository.FindByEmail(email);

		if (user == null) throw new Exception("Usuário não encontrado.");
		if (user.Activated && !recoveringPass) throw new Exception("Usuário já ativado.");

		user.ActivationToken = _userRepository.RegenActivationToken();
		_userRepository.Update(user);
	}
	//!reset password
	public void ResetPassword(string token, string password) {
		var user = _userRepository.FindByActivationToken(token);
		if (user == null) throw new Exception("Usuário não encontrado.");

		var userToValidate = new User(user.Email, user.FirstName, user.LastName, password, user.RefreshToken, user.RefreshTokenExpiryTime);
		if (!userToValidate.IsValid) {
			var error = new ErrorModel(user.Notifications.convertToEnumerable());
			throw new Exception(error.ToString());
		}
		user.ActivationToken = null;
		user.Password = SecutiryUtils.ComputeHash(password, SHA256.Create()); ;

		_userRepository.Update(user);
	}
}