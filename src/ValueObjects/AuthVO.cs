using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AUTENTICADOR.src.Model.Error;

namespace AUTENTICADOR.src.ValueObjects;

public class AuthVO {
	public AuthVO(string accessToken, string errorMessage = null) {
		AccessToken = accessToken;
		ErrorMessage = errorMessage;
	}

	public string AccessToken { get; set; }
	public string ErrorMessage { get; set; }
}
