using AUTENTICADOR.src.Extensions.toFluntNotifications;
using AUTENTICADOR.src.Model.Entities;
using AUTENTICADOR.src.Model.Error;
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
	public UserController(IUserService service, IEmailService email) {
		_service = service;
		_email = email;
	}

	//! ////////////////////////////////////////////////////////////////////////////////////
	//? bloco com controladores anonimos
	//! ////////////////////////////////////////////////////////////////////////////////////	
	[HttpPost]
	[AllowAnonymous]
	[Route("register")]
	public IActionResult Register([FromBody] UserRequestVO user, [FromQuery] string origin) {
		if (user == null || user.Email == null || user.Password == null || user.ConfirmPassword == null) {
			return BadRequest(new ErrorModel("Dados inválidos."));
		}
		if (user.Password != user.ConfirmPassword) {
			return BadRequest(new ErrorModel("Senha e confirmação de senha não conferem."));
		}

		UserResponseVO userCreated;
		try {
			userCreated = _service.Create(user);
			_email.sendRegisterEmail(userCreated.Email, origin);
		} catch (Exception e) {
			return BadRequest(new ErrorModel(e.Message));
		}

		return Ok(userCreated);
	}


	//! ////////////////////////////////////////////////////////////////////////////////////
	//? bloco com controladores que precisam autorização
	//! ////////////////////////////////////////////////////////////////////////////////////	
	[HttpGet]
	[Route("profile")]
	public IActionResult findProfile() {
		var id = getIdFromToken();
		var user = _service.FindById(id);
		return Ok(user);
	}

	[HttpPut]
	[Route("")]
	public IActionResult Update([FromBody] UserRequestVO request) {
		try {
			ValidaSenhas(request);
			var id = getIdFromToken();
			request.id = id;
			return Ok(_service.Update(request));
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
	private void ValidaSenhas(UserRequestVO request) {
		if (
				(request == null) ||
				(request.Password == null && request.ConfirmPassword != null) ||
				(request.Password != null && request.ConfirmPassword == null) ||
				(request.Password != request.ConfirmPassword)
			) {
			throw new Exception("Senha e confirmação de senha não conferem.");
		}
	}
}