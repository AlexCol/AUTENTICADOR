using System.ComponentModel.DataAnnotations.Schema;
using AUTENTICADOR.src.Model.Entities.Base;

namespace AUTENTICADOR.src.Model.Entities;

[Table("users")]
public class User : BaseEntity {
	public User() {
		this.RefreshTokenExpiryTime = DateTime.Now;
		this.Activated = false;
	}

	public User(string email, string firstName, string lastName, string password, string refreshToken, DateTime refreshTokenExpiryTime) {
		this.Email = email;
		this.FirstName = firstName;
		this.LastName = lastName;
		this.Password = password;
		this.RefreshToken = refreshToken;
		this.RefreshTokenExpiryTime = refreshTokenExpiryTime;
		this.Activated = false;
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
}

