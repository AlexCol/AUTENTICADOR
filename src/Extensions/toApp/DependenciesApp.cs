using AUTENTICADOR.src.extensions.toBuilder;
using AUTENTICADOR.src.Repository.GenericRepository;
using AUTENTICADOR.src.Repository.UserRepository;
using AUTENTICADOR.src.Services.EmailService;
using AUTENTICADOR.src.Services.Login;
using AUTENTICADOR.src.Services.TokenService;
using AUTENTICADOR.src.Services.UserService;

namespace AUTENTICADOR.src.Extensions.toBuilder;

public static class DependenciesApp {
	public static void addDependencies(this WebApplication app) {
		//!adicionando configurações padrão
		app.UseCors(); //para ativar o cors
									 //app.UseCors("CORSAllowLocalHost");

		app.UseAuthentication();
		app.UseAuthorization();

		app.UseSwagger();
		app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{"Rest Api para autenticação"}"));

		app.UseHttpsRedirection();
		app.MapControllers();

	}
}