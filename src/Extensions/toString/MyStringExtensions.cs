using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AUTENTICADOR.src.Extensions.toString;

public static class MyStringExtensions {
	public static bool HasUpperCase(this string str) {
		if (str == null) return false;
		return str.Any(char.IsUpper);
	}

	public static bool HasLowerCase(this string str) {
		if (str == null) return false;
		return str.Any(char.IsLower);
	}

	public static bool HasSpecialCharacter(this string str) {
		if (str == null) return false;
		return str.Any(ch => !char.IsLetterOrDigit(ch));
	}
}
