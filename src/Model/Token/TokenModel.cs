using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AUTENTICADOR.src.Model.Token;

public class TokenModel
{
	public string Audience { get; set; }
	public string Issuer { get; set; }
	public string Secret { get; set; }
	public int Minutes { get; set; }
	public int DaysToExpire { get; set; }
}
