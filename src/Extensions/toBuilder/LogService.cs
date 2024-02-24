using Serilog;

namespace AUTENTICADOR.src.Extensions.toBuilder;

public static class LogService {
	public static void addLogService(this WebApplicationBuilder builder) {
		//!ativando serilog
		builder.Host.UseSerilog((context, configuration) => configuration.WriteTo.Console());
	}
}
