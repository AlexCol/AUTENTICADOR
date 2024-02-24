using System.ComponentModel.DataAnnotations.Schema;
using AUTENTICADOR.src.Extensions.toString;
using AUTENTICADOR.src.Model.Entities.Base;
using Flunt.Validations;

namespace AUTENTICADOR.src.Model.Entities;

[Table("users")]
public class User : BaseEntity {
	public User() { Validate(); }

	public User(string email, string firstName, string lastName, string password, string refreshToken, DateTime refreshTokenExpiryTime) {
		this.Email = email;
		this.FirstName = firstName;
		this.LastName = lastName;
		this.Password = password;
		this.RefreshToken = refreshToken;
		this.RefreshTokenExpiryTime = refreshTokenExpiryTime;
		this.Activated = false;
		Validate();
	}

	[Column("ds_email")]
	public string Email { get; set; }

	[Column("ds_first_name")]
	public string FirstName { get; set; }

	[Column("ds_last_name")]
	public string LastName { get; set; }

	[Column("ds_password")]
	public string Password { get; set; }

	[Column("ds_refresh_token")]
	public string RefreshToken { get; set; }

	[Column("dt_refresh_token_expiry_time")]
	public DateTime RefreshTokenExpiryTime { get; set; }

	[Column("sn_activated")]
	public Boolean Activated { get; set; }
	[Column("ds_activation_token")]
	public string ActivationToken { get; set; }

	private void Validate() {
		var contract = new Contract<User>()
				.IsNotNullOrWhiteSpace(Email, "Email", "Email precisa estar preenchido.")
				.IsEmail(Email, "Email", "Email inválido.")
				.IsNotNullOrWhiteSpace(FirstName, "Nome", "Nome precisa estar preenchido.")
				.IsGreaterOrEqualsThan(FirstName, 3, "Nome", "Nome deve ter pelo menos tres caracteres.")
				.IsNotNullOrWhiteSpace(LastName, "Sobrenome", "Sobrenome precisa estar preenchido.")
				.IsNotNullOrWhiteSpace(Password, "Senha", "A senha é obrigatória.")
				.IsGreaterOrEqualsThan(Password, 5, "Senha", "Senha deve conter pelo menos 5 caracteres.")
				.IsTrue(Password.HasUpperCase(), "Senha", "A senha deve conter carecteres maiúsculos.")
				.IsTrue(Password.HasLowerCase(), "Senha", "A senha deve conter carecteres minúsculos.")
				.IsTrue(Password.HasSpecialCharacter(), "Senha", "A senha deve conter carecteres especiais.");
		;
		AddNotifications(contract);
	}


}

