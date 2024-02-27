using AUTENTICADOR.src.Extensions.toString;
using AUTENTICADOR.src.Model.Entities;
using Flunt.Notifications;
using Flunt.Validations;

namespace AUTENTICADOR.src.ValueObjects;

public class UserRequestVO : Notifiable<Notification> {
	public UserRequestVO() { ValidateAll(); }

	public Guid id { get; set; }
	public string Email { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string Password { get; set; }
	public string ConfirmPassword { get; set; }

	public void ValidateAll() {
		Clear();
		var contract = new Contract<UserRequestVO>()
				.IsNotNullOrWhiteSpace(Email, "Email", "Email precisa estar preenchido.")
				.IsEmail(Email, "Email", "Email inválido.")
				.IsNotNullOrWhiteSpace(FirstName, "Nome", "Nome precisa estar preenchido.")
				.IsGreaterOrEqualsThan(FirstName, 3, "Nome", "Nome deve ter pelo menos tres caracteres.")
				.IsNotNullOrWhiteSpace(LastName, "Sobrenome", "Sobrenome precisa estar preenchido.")
				.IsNotNullOrWhiteSpace(Password, "Senha", "A senha é obrigatória.")
				.IsGreaterOrEqualsThan(Password, 5, "Senha", "Senha deve conter pelo menos 5 caracteres.")
				.IsTrue(Password.HasUpperCase(), "Senha", "A senha deve conter carecteres maiúsculos.")
				.IsTrue(Password.HasLowerCase(), "Senha", "A senha deve conter carecteres minúsculos.")
				.IsTrue(Password.HasNumber(), "Senha", "A senha deve conter números.")
				.IsTrue(Password.HasSpecialCharacter(), "Senha", "A senha deve conter carecteres especiais.")
				.IsTrue(Password == ConfirmPassword, "Senha", "A senha e sua confirmação devem ser iguais.");
		;
		AddNotifications(contract);
	}

	public void ValidateFilledFields() {
		Clear();
		var contract = new Contract<UserRequestVO>();

		if (Email != null) {
			contract.IsNotNullOrWhiteSpace(Email, "Email", "Email precisa estar preenchido.")
							.IsEmail(Email, "Email", "Email inválido.");
		}
		if (FirstName != null) {
			contract.IsNotNullOrWhiteSpace(FirstName, "Nome", "Nome precisa estar preenchido.")
							.IsGreaterOrEqualsThan(FirstName, 3, "Nome", "Nome deve ter pelo menos tres caracteres.");
		}
		if (LastName != null) {
			contract.IsNotNullOrWhiteSpace(LastName, "Sobrenome", "Sobrenome precisa estar preenchido.");
		}
		if (Password != null || ConfirmPassword != null) {
			contract.IsNotNullOrWhiteSpace(Password, "Senha", "A senha é obrigatória.")
							.IsGreaterOrEqualsThan(Password, 5, "Senha", "Senha deve conter pelo menos 5 caracteres.")
							.IsTrue(Password.HasUpperCase(), "Senha", "A senha deve conter carecteres maiúsculos.")
							.IsTrue(Password.HasLowerCase(), "Senha", "A senha deve conter carecteres minúsculos.")
							.IsTrue(Password.HasSpecialCharacter(), "Senha", "A senha deve conter carecteres especiais.")
							.IsTrue(Password == ConfirmPassword, "Senha", "A senha e sua confirmação devem ser iguais.");
		}
		AddNotifications(contract);
	}
}



