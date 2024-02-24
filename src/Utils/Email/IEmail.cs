using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AUTENTICADOR.src.Model.Entities;

namespace AUTENTICADOR.src.Utils.Email;

public interface IEmail {
	void sendRegisterEmail(User user);
}
