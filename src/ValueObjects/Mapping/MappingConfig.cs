using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AUTENTICADOR.src.Model.Entities;
using AutoMapper;

namespace AUTENTICADOR.src.ValueObjects.Mapping;

public static class MappingConfig {
	public static MapperConfiguration RegisterMaps() {
		var mappingCong = new MapperConfiguration(config => {
			config.CreateMap<User, UserLoginVO>();
			config.CreateMap<UserLoginVO, User>();
		});
		return mappingCong;
	}
}
