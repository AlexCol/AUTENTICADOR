using AUTENTICADOR.src.Model.Error;
using AUTENTICADOR.src.Services.EmailService;
using AUTENTICADOR.src.Services.Login;
using AUTENTICADOR.src.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace AUTENTICADOR.src.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase {
	private readonly ILoginService _service;
	private readonly IEmailService _email;
	public AuthController(ILoginService service, IEmailService email) {
		_service = service;
		_email = email;
	}

	//! ////////////////////////////////////////////////////////////////////////////////////
	//? bloco com controladores anonimos
	//! ////////////////////////////////////////////////////////////////////////////////////	
	[HttpPost]
	[Route("signin")]
	public IActionResult Signin([FromBody] UserLoginVO user) {
		if (user == null || user.Email == null || user.Password == null)
			return BadRequest(new ErrorModel("Solicitação invalida."));

		var token = _service.ValidadeCredentials(user);
		if (token.ErrorMessage != null)
			return BadRequest(new ErrorModel(token.ErrorMessage));

		return Ok(new { token.AccessToken, token.RefreshToken });
	}

	[HttpPost]
	[Route("refresh")]
	public IActionResult Refresh([FromBody] AuthVO authVO) {
		if (authVO == null)
			return BadRequest(new ErrorModel("Solicitação invalida."));

		try {
			var token = _service.ValidadeCredentials(authVO);
			if (token.ErrorMessage != null)
				return BadRequest(new ErrorModel(token.ErrorMessage));

			return Ok(new { token.AccessToken, token.RefreshToken });
		} catch {
			return BadRequest(new ErrorModel("Realizar novo login para continuar."));
		}
	}

	[HttpPut]
	[Route("activate")]
	public IActionResult Activate([FromQuery] string t) {

		try {
			_service.ActivateUser(t);
		} catch (Exception e) {
			return BadRequest(new ErrorModel(e.Message));
		}
		return Ok("Conta Ativada, vá para a pagina de login para realizar seu acesso.");
	}

	[HttpPut]
	[Route("resend_activation_token")]
	public IActionResult ResendActivationToken([FromBody] UserLoginVO auth, [FromQuery] string origin) {
		try {
			_service.RefreshActivationToken(auth.Email);
			_email.sendRegisterEmail(auth.Email, origin);
		} catch (Exception e) {
			return BadRequest(new ErrorModel(e.Message));
		}
		return Ok("Email re-enviado.");
	}

	[HttpPut]
	[Route("recover_password")]
	public IActionResult RecoverPassword([FromBody] UserLoginVO auth) {

		try {
			_service.RefreshActivationToken(auth.Email, true);
			_email.sendRecoverPasswordEmail(auth.Email);
		} catch (Exception e) {
			return BadRequest(new ErrorModel(e.Message));
		}
		return Ok("Email de troca de senha enviada para o email do cadastro.");
	}
	[HttpPut]
	[AllowAnonymous]
	[Route("newpassword")]
	public IActionResult ResetPassword([FromBody] UserRequestVO userRequest, [FromQuery] string t) {
		var token = t;
		var passwords = new UserRequestVO() { //pra garantir que o usuario não mandou outros dados juntos
			Password = userRequest.Password,
			ConfirmPassword = userRequest.ConfirmPassword
		};

		try {
			ValidaSenhas(passwords);
			_service.ResetPassword(token, passwords.Password);
			return Ok("Senha alterada.");
		} catch (Exception e) {
			return BadRequest(new ErrorModel(e.Message));
		}
	}
	//! ////////////////////////////////////////////////////////////////////////////////////
	//? bloco com controladores que precisam autorização
	//! ////////////////////////////////////////////////////////////////////////////////////	
	[HttpPut]
	[Authorize]
	[Route("revoke")]
	public IActionResult Revoke() {
		var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId");
		if (userId == null)
			return Ok("Erro ao revogar o token.");

		Guid.TryParse(userId.Value, out Guid id);
		var result = _service.RevokeToken(id);

		return Ok("Token revogado.");
	}
	//! ////////////////////////////////////////////////////////////////////////////////////
	//? bloco com funções de apoio
	//! ////////////////////////////////////////////////////////////////////////////////////	
	private void ValidaSenhas(UserRequestVO request) {
		if (
				(request == null) ||
				(request.Password == null && request.ConfirmPassword != null) ||
				(request.Password != null && request.ConfirmPassword == null) ||
				(request.Password == null && request.ConfirmPassword == null) ||
				(request.Password != request.ConfirmPassword)
			) {
			throw new Exception("Senha e confirmação de senha não conferem.");
		}
	}
}