using AUTENTICADOR.src.Model.Entities;
using AutoMapper;

namespace AUTENTICADOR.src.ValueObjects.Mapping;

public static class MappingConfig {
	public static MapperConfiguration RegisterMaps() {
		var mappingCong = new MapperConfiguration(config => {
			config.CreateMap<User, UserResponseVO>();
			config.CreateMap<UserResponseVO, User>();

			config.CreateMap<User, UserRequestVO>();
			config.CreateMap<UserRequestVO, User>();
		});
		return mappingCong;
	}
}
