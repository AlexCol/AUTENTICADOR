using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AUTENTICADOR.src.Model.Error;

namespace AUTENTICADOR.src.ValueObjects;

public class AuthVO {
	public AuthVO(string errorMessage) {
		ErrorMessage = errorMessage;
	}

	public AuthVO(string accessToken, string refreshToken) {
		AccessToken = accessToken;
		RefreshToken = refreshToken;
	}

	public string AccessToken { get; set; }
	public string RefreshToken { get; set; }
	public string ErrorMessage { get; set; }
}
