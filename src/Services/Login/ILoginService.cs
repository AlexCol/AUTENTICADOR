using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AUTENTICADOR.src.ValueObjects;

namespace AUTENTICADOR.src.Services.Login;

public interface ILoginService {
	AuthVO ValidadeCredentials(UserLoginVO userCredentials);
	AuthVO ValidadeCredentials(AuthVO token);
	bool RevokeToken(Guid id);
}
