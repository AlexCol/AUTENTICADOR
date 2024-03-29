using AUTENTICADOR.src.Extensions.toFluntNotifications;
using AUTENTICADOR.src.Model.Entities;
using AUTENTICADOR.src.Model.Error;
using AUTENTICADOR.src.Services.CryptoService;
using AUTENTICADOR.src.Services.EmailService;
using AUTENTICADOR.src.Services.UserService;
using AUTENTICADOR.src.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace AUTENTICADOR.src.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class UserController : ControllerBase {
	private IUserService _service;
	private IEmailService _email;
	private ICryptoService _crypto;
	public UserController(IUserService service, IEmailService email, ICryptoService crypto) {
		_service = service;
		_email = email;
		_crypto = crypto;
	}

	//! ////////////////////////////////////////////////////////////////////////////////////
	//? bloco com controladores anonimos
	//! ////////////////////////////////////////////////////////////////////////////////////	
	[HttpPost]
	[AllowAnonymous]
	[Route("register")]
	public IActionResult Register([FromBody] CryptedDataVO registerRequest, [FromQuery] string origin) {
		UserRequestVO user = _crypto.Decrypt<UserRequestVO>(registerRequest.Data);

		if (user == null) {
			return BadRequest(new ErrorModel("Dados inválidos."));
		}
		user.ValidateAll();
		if (!user.IsValid) return BadRequest(new ErrorModel(user.Notifications.convertToEnumerable()));

		UserResponseVO userCreated;
		try {
			userCreated = _service.Create(user);
			_email.sendRegisterEmail(userCreated.Email, origin);
		} catch (Exception e) {
			return BadRequest(new ErrorModel(e.Message));
		}

		return Ok(new { Success = "Usuário criado. Favor acessar seu email para acessar o link de ativação." });
	}


	//! ////////////////////////////////////////////////////////////////////////////////////
	//? bloco com controladores que precisam autorização
	//! ////////////////////////////////////////////////////////////////////////////////////	
	[HttpGet]
	[Route("profile")]
	public IActionResult findProfile() {
		var id = getIdFromToken();
		var user = _service.FindById(id);
		var result = _crypto.Encrypt<UserResponseVO>(user);
		return Ok(result);
	}

	[HttpPut]
	[Route("")]
	public IActionResult Update([FromBody] CryptedDataVO updateRequest) {
		var request = _crypto.Decrypt<UserRequestVO>(updateRequest.Data);
		request.ValidateFilledFields();
		if (!request.IsValid) return BadRequest(new ErrorModel(request.Notifications.convertToEnumerable()));

		try {
			var id = getIdFromToken();
			request.id = id;
			var user = _service.Update(request);
			var result = _crypto.Encrypt<UserResponseVO>(user);
			return Ok(result);
		} catch (Exception e) {
			return BadRequest(new ErrorModel(e.Message));
		}
	}

	[HttpDelete]
	[Route("")]
	public IActionResult Delete() {

		try {
			var id = getIdFromToken();
			_service.Delete(id);
		} catch (Exception e) {
			return BadRequest(new ErrorModel(e.Message));
		}
		return NoContent();
	}

	//! ////////////////////////////////////////////////////////////////////////////////////
	//? bloco com funções de apoio
	//! ////////////////////////////////////////////////////////////////////////////////////	
	private Guid getIdFromToken() {
		var claim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
		if (claim == null || claim.Value == null) throw new Exception("Token invalido, ou usuário não existe.");
		Guid.TryParse(claim.Value, out Guid id);
		return id;
	}
}