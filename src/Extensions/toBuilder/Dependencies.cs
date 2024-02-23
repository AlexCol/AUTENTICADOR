using AUTENTICADOR.src.extensions.toBuilder;
using AUTENTICADOR.src.Services.TokenService;

namespace AUTENTICADOR.src.Extensions.toBuilder;

public static class Dependencies
{
	public static void addDependencies(this WebApplicationBuilder builder)
	{
		//!adicionando configurações
		builder.addSwagger();
		builder.addPostgre();
		builder.AddCors();
		builder.addJWTService();

		//!adicionando classes para injeções de dependencia
		builder.Services.AddTransient<ITokenService, TokenService>();
	}
}
