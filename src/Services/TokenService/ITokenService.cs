using System.Security.Claims;

namespace AUTENTICADOR.src.Services.TokenService;

public interface ITokenService {
	string GenerateAccesToken(IEnumerable<Claim> claims);
	IEnumerable<Claim> GetClaimsFromExpiredToken(string token);
}
