using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AUTENTICADOR.src.Model;
using Microsoft.EntityFrameworkCore;

namespace AUTENTICADOR.src.extensions.toBuilder;

public static class PostgreBuilder {
	public static void addPostgre(this WebApplicationBuilder builder) {
		var conectionString = builder.Configuration["ConnectionStrings:Postgre"];
		builder.Services.AddDbContext<PostgreContext>(options => {
			options.UseNpgsql(conectionString);
		});
	}
}
