using AUTENTICADOR.src.Extensions.toFluntNotifications;
using AUTENTICADOR.src.Model.Entities;
using AUTENTICADOR.src.Model.Error;
using AUTENTICADOR.src.Services.UserService;
using AUTENTICADOR.src.Utils.Email;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AUTENTICADOR.src.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class UserController : ControllerBase {
	private IUserService _service;
	private IEmail _email;
	public UserController(IUserService service, IEmail email) {
		_service = service;
		_email = email;
	}

	[HttpPost]
	[Route("register")]
	public IActionResult Refresh([FromBody] User user) {
		if (user == null || user.Email == null || user.Password == null) {
			return BadRequest(new ErrorModel("Dados inválidos."));
		}

		var newUser = new User(
			user.Email,
			user.FirstName,
			user.LastName,
			user.Password,
			null,
			DateTime.Now
		);
		if (!newUser.IsValid) return BadRequest(new ErrorModel(newUser.Notifications.convertToEnumerable()));

		try {
			_service.Create(newUser);
			_email.sendRegisterEmail(newUser);
		} catch (Exception e) {
			return BadRequest(new ErrorModel(e.Message));
		}

		return Ok(newUser);
	}



}
