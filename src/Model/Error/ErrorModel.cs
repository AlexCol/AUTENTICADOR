using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Internal;

namespace AUTENTICADOR.src.Model.Error;

public class ErrorModel {
	public ErrorModel(string errorMessage) {
		ErrorMessage.Add(errorMessage);
	}

	public ErrorModel(IEnumerable<string> errorMessages) {
		ErrorMessage.AddRange(errorMessages);
	}

	public List<string> ErrorMessage { get; set; } = new List<string>();
}
