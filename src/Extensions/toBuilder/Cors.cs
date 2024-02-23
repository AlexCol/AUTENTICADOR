using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AUTENTICADOR.src.Extensions.toBuilder;

public static class Cors
{
	public static void AddCors(this WebApplicationBuilder builder)
	{
		//! adicionando liberação para que se permita o consumo da API por outra origem que não C# e fora do dominio
		builder.Services.AddCors(opt => opt.AddDefaultPolicy(build =>
		{
			build
							.AllowAnyOrigin()
							.AllowAnyHeader()
							.AllowAnyMethod();
		}));
	}
}