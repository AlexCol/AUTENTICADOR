using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AUTENTICADOR.src.Services.CryptoService;
using AUTENTICADOR.src.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AUTENTICADOR.src.Controllers;

[ApiController]
[Route("[controller]")]
public class CryptoController : ControllerBase {

	private readonly ICryptoService _crypto;
	public CryptoController(ICryptoService crypto) {
		_crypto = crypto;
	}
	[HttpGet]
	[Route("decrypt")]
	public IActionResult decrypt([FromBody] CryptedDataVO encrypted) {
		try {
			var value = _crypto.Decrypt(encrypted.Data);
			return Ok(value);
		} catch (Exception e) {
			return BadRequest("Token invalido!" + e.Message);
		}
	}

	[HttpGet]
	[Route("encrypt")]
	public IActionResult encrypt([FromBody] object decrypted) {
		try {
			var inputString = decrypted.ToString().Replace("\r\n", "").Replace("\n", "").Replace("\r", ""); //removendo quebras de linha
			var value = _crypto.Encrypt(inputString);
			return Ok(value);
		} catch (Exception e) {
			return BadRequest("Token invalido!" + e.Message);
		}
	}
}
