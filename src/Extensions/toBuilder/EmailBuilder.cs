using AUTENTICADOR.src.Model.EmailModel;
using Microsoft.Extensions.Options;

namespace AUTENTICADOR.src.Extensions.toBuilder;

public static class EmailBuilder {
	public static void addEmailService(this WebApplicationBuilder builder) {

		//! Cria uma instância de TokenModel para armazenar as configurações do token
		var emailModel = new EmailModel();

		//! Configura o tokenModel a partir das configurações fornecidas no arquivo de configuração
		new ConfigureFromConfigurationOptions<EmailModel>(
				builder.Configuration.GetSection("Email")
		).Configure(emailModel);

		//! Registra o tokenModel como um serviço singleton
		builder.Services.AddSingleton(emailModel);
	}
}