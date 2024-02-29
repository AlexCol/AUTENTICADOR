using AUTENTICADOR.src.extensions.toBuilder;
using AUTENTICADOR.src.Repository.GenericRepository;
using AUTENTICADOR.src.Repository.UserRepository;
using AUTENTICADOR.src.Services.CryptoService;
using AUTENTICADOR.src.Services.EmailService;
using AUTENTICADOR.src.Services.Login;
using AUTENTICADOR.src.Services.TokenService;
using AUTENTICADOR.src.Services.UserService;

namespace AUTENTICADOR.src.Extensions.toBuilder;

public static class DependenciesBuilder {
	public static void addDependencies(this WebApplicationBuilder builder) {
		//!adicionando configurações padrão
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddControllers();

		//!adicionando configurações
		builder.addSwagger();
		builder.addPostgre();
		builder.AddCors(); //?lembrar depois de colocar useCors no app
		builder.addJWTService();
		builder.addAutoMapper();
		builder.addLogService();
		builder.addEmailService();

		//!adicionando classes para injeções de dependencia
		builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
		builder.Services.AddScoped<IUserRepository, UserRepository>();

		builder.Services.AddScoped<ITokenService, TokenService>();
		builder.Services.AddScoped<ILoginService, LoginService>();
		builder.Services.AddScoped<IUserService, UserService>();
		builder.Services.AddScoped<IEmailService, EmailService>();
		builder.Services.AddScoped<ICryptoService, CryptoService>();

	}
}