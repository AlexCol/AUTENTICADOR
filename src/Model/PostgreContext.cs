using System.Security.Cryptography;
using System.Text;
using AUTENTICADOR.src.Model.Entities;
using Flunt.Notifications;
using Microsoft.EntityFrameworkCore;

namespace AUTENTICADOR.src.Model;

public class PostgreContext : DbContext {
	public PostgreContext() { }
	public PostgreContext(DbContextOptions<PostgreContext> options) : base(options) {
		AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true); //!para permitir usar data 'local'
	}

	public DbSet<User> Users { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder) {
		base.OnModelCreating(modelBuilder);
		modelBuilder.Ignore<Notification>(); //!para ifgnorar os erros se usar a função de notificação

		//https://learn.microsoft.com/en-us/ef/core/modeling/entity-properties?tabs=fluent-api%2Cwithout-nrt
		modelBuilder.Entity<User>()
				.Property(u => u.Email).IsRequired();
		modelBuilder.Entity<User>()
				.HasIndex(u => u.Email)
				.HasDatabaseName("uk_Email")
				.IsUnique(true);
		modelBuilder.Entity<User>()
				.HasIndex(u => u.RefreshToken)
				.HasDatabaseName("idx_refresh");
		modelBuilder.Entity<User>()
				.HasIndex(u => u.ActivationToken)
				.IsUnique()
				.HasDatabaseName("uk_activateToken");
		modelBuilder.Entity<User>()
				.Property(u => u.Password).IsRequired();


		// var password = ComputeHash("1234", SHA256.Create());
		// var newUser = new User("eu_axil@yahoo.com.br", "Alexandre", "Coletti", password, null, DateTime.Now);
		// newUser.id = Guid.NewGuid();
		// newUser.Activated = true;
		// modelBuilder.Entity<User>().HasData(newUser);

		// password = ComputeHash("1234", SHA256.Create());
		// newUser = new User("eu_axill@yahoo.com.br", "Bernard", "Coletti", password, "afa6ef16se5g165rg165rg165", DateTime.Now);
		// newUser.id = Guid.NewGuid();
		// modelBuilder.Entity<User>().HasData(newUser);
	}

	// private string ComputeHash(string password, HashAlgorithm hashAlgorithm) {
	// 	byte[] inputBytes = Encoding.UTF8.GetBytes(password);
	// 	byte[] hashedBytes = hashAlgorithm.ComputeHash(inputBytes);

	// 	var builder = new StringBuilder();

	// 	foreach (var item in hashedBytes) {
	// 		builder.Append(item.ToString("x2"));
	// 	}
	// 	return builder.ToString();
	// }
}
