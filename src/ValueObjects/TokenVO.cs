using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AUTENTICADOR.src.ValueObjects;

public class TokenVO
{
	public TokenVO(string expiration, string accesToken, string refreshToken)
	{
		Expiration = expiration;
		AccesToken = accesToken;
		RefreshToken = refreshToken;
	}

	public string Expiration { get; set; }
	public string AccesToken { get; set; }
	public string RefreshToken { get; set; }
}
