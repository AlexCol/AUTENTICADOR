using System.ComponentModel.DataAnnotations.Schema;
using AUTENTICADOR.src.Model.Entities.Base;
using Flunt.Validations;

namespace AUTENTICADOR.src.Model.Entities;

[Table("users")]
public class User : BaseEntity
{
	public User() { Validate(); }

	public User(string email, string fullName, string password, string refreshToken, DateTime refreshTokenExpiryTime)
	{
		this.Email = email;
		this.FullName = fullName;
		this.Password = password;
		this.RefreshToken = refreshToken;
		this.RefreshTokenExpiryTime = refreshTokenExpiryTime;
		Validate();
	}

	[Column("ds_email")]
	public string Email { get; set; }

	[Column("ds_full_name")]
	public string FullName { get; set; }

	[Column("ds_password")]
	public string Password { get; set; }

	[Column("ds_refresh_token")]
	public string RefreshToken { get; set; }

	[Column("dt_refresh_token_expiry_time")]
	public DateTime RefreshTokenExpiryTime { get; set; }

	private void Validate()
	{
		var contract = new Contract<User>()
				.IsNotNullOrEmpty(Email, "Email", "Email precisa estar preenchido.")
				.IsNotNullOrEmpty(FullName, "NomeCompleto", "Nome completo precisa estar preenchido.")
				.IsEmail(Email, "Email", "Email inválido.");
		AddNotifications(contract);
	}
}

