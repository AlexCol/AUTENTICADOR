using Serilog;

namespace AUTENTICADOR.src.Extensions.toBuilder;

public static class LogBuilder {
	public static void addLogService(this WebApplicationBuilder builder) {
		//!ativando serilog
		builder.Host.UseSerilog((context, configuration) => configuration.WriteTo.Console());
	}
}
