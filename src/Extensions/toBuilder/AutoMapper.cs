using AUTENTICADOR.src.ValueObjects.Mapping;
using AutoMapper;

namespace AUTENTICADOR.src.Extensions.toBuilder;

public static class AutoMapper {
	public static void addAutoMapper(this WebApplicationBuilder builder) {
		//!configuração do automapping (pra evitar precisar criar um conversor)
		IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
		builder.Services.AddSingleton(mapper);
		builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
	}

}
