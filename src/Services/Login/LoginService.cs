using System.Security.Claims;
using System.Security.Cryptography;
using AUTENTICADOR.src.Extensions.toFluntNotifications;
using AUTENTICADOR.src.Extensions.toUserRepo;
using AUTENTICADOR.src.Model.Entities;
using AUTENTICADOR.src.Model.Error;
using AUTENTICADOR.src.Model.Token;
using AUTENTICADOR.src.Repository.UserRepository;
using AUTENTICADOR.src.Services.CryptoService;
using AUTENTICADOR.src.Services.TokenService;
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
	private readonly ICryptoService _crypto;

	public LoginService(TokenModel tokenModel, IUserRepository userRepository, ITokenService tokenService, IMapper mapper, ICryptoService crypto) {
		_tokenModel = tokenModel;
		_userRepository = userRepository;
		_tokenService = tokenService;
		_mapper = mapper;
		_crypto = crypto;
	}

	public AuthVO ValidadeCredentials(UserLoginVO userCredentials) {
		if (userCredentials == null) return new AuthVO(null, "Não enviados dados para autenticação.");

		userCredentials.Password = _crypto.ComputeHash(userCredentials.Password, SHA256.Create());
		var user = _userRepository.ValidadeCredentials(userCredentials);
		if (user == null) return new AuthVO(null, "Usuário ou senha invalidos.");

		if (!user.Activated) return new AuthVO(null, "Usuário inativo. Verificar seu email e clicar no link para ativação da conta.");

		var claims = new List<Claim>() {
						new Claim("UserId", user.id.ToString())
				};
		var accessToken = _tokenService.GenerateAccesToken(claims);

		user.ActivationToken = null;

		_userRepository.Update(user);

		return new AuthVO(accessToken);
	}

	//!activate user
	public UserResponseVO ActivateUser(string token) {
		var user = _userRepository.FindByActivationToken(token);
		Log.Error(token);

		if (user == null) throw new Exception("Link não é mais válido.");

		user.Activated = true;
		user.ActivationToken = null;
		return _mapper.Map<UserResponseVO>(_userRepository.Update(user));
	}

	//!refresh activation token
	public void RefreshActivationToken(string email, bool recoveringPass = false) {
		var user = _userRepository.FindByEmail(email);

		if (user == null) throw new Exception("Usuário não encontrado.");
		if (user.Activated && !recoveringPass) throw new Exception("Usuário já ativado.");
		if (!user.Activated && recoveringPass) throw new Exception("Usuário Inativo, não pode mudar a senha. Realize primeiro a ativação do cadastro.");

		user.ActivationToken = _userRepository.RegenActivationToken();
		_userRepository.Update(user);
	}
	//!reset password
	public void ResetPassword(string token, string password) {
		var user = _userRepository.FindByActivationToken(token);
		if (user == null) throw new Exception("Link Inválido");

		var newPassword = _crypto.ComputeHash(password, SHA256.Create());
		if (user.Password == newPassword) throw new Exception("A senha nova não pode ser mesma que a atual.");

		user.ActivationToken = null;
		user.Password = newPassword;

		_userRepository.Update(user);
	}
}