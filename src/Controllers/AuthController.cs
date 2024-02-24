using AUTENTICADOR.src.Model.Error;
using AUTENTICADOR.src.Services.Login;
using AUTENTICADOR.src.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AUTENTICADOR.src.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
	private ILoginService _login;
	public AuthController(ILoginService login)
	{
		_login = login;
	}

	[HttpPost]
	[Route("signin")]
	public IActionResult Signin([FromBody] UserLoginVO user)
	{
		if (user == null || user.Email == null || user.Password == null)
			return BadRequest(new ErrorModel("Solicitação invalida."));

		var token = _login.ValidadeCredentials(user);
		if (token.ErrorMessage != null)
			return BadRequest(new ErrorModel(token.ErrorMessage));

		return Ok(new { token.AccessToken, token.RefreshToken });
	}

	[HttpPost]
	[Route("refresh")]
	public IActionResult Refresh([FromBody] AuthVO authVO)
	{
		if (authVO == null)
			return BadRequest(new ErrorModel("Solicitação invalida."));

		var token = _login.ValidadeCredentials(authVO);
		if (token.ErrorMessage != null)
			return BadRequest(new ErrorModel(token.ErrorMessage));

		return Ok(new { token.AccessToken, token.RefreshToken });
	}

	[HttpGet]
	[Authorize]
	[Route("revoke")]
	public IActionResult Revoke()
	{
		var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId");
		if (userId == null)
			return Ok("Erro ao revogar o token.");

		Guid.TryParse(userId.Value, out Guid id);
		var result = _login.RevokeToken(id);
		return Ok("Token revogado.");
	}
}
