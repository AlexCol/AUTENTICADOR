using System.Security.Claims;

namespace AUTENTICADOR.src.Services.TokenService;

public interface ITokenService
{
	string GenerateAccesToken(IEnumerable<Claim> claims);
	string GenerateRefreshToken();
	ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}
