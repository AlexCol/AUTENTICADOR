using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AUTENTICADOR.src.ValueObjects;

public class UserRequestVO {
	public Guid id { get; set; }
	public string Email { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string Password { get; set; }
	public string ConfirmPassword { get; set; }
}
