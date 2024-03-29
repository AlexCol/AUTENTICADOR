using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AUTENTICADOR.src.Model.Token;
using Microsoft.IdentityModel.Tokens;

namespace AUTENTICADOR.src.Services.TokenService {
	public class TokenService : ITokenService {
		private TokenModel _configuration;

		//! Construtor da classe TokenService
		public TokenService(TokenModel configuration) {
			_configuration = configuration;
		}

		//! Método para gerar um token de acesso com base nas reivindicações fornecidas
		public string GenerateAccesToken(IEnumerable<Claim> claims) {
			var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Secret));
			var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

			//! Criação das opções para o token JWT
			var options = new JwtSecurityToken(
					issuer: _configuration.Issuer,
					audience: _configuration.Audience,
					claims: claims,
					signingCredentials: signingCredentials,
					expires: DateTime.Now.AddMinutes(_configuration.Minutes)
			);

			var tokenHandler = new JwtSecurityTokenHandler();
			string tokenString = tokenHandler.WriteToken(options);
			return tokenString;
		}

		//! Método para obter as reivindicações de um token JWT expirado
		public IEnumerable<Claim> GetClaimsFromExpiredToken(string token) {
			var validToken = validateToken(token);
			if (!validToken)
				throw new SecurityTokenException("Invalid Token.");

			var handler = new JwtSecurityTokenHandler();
			var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
			var claims = jsonToken.Claims.Where(c => c.Type != "aud");
			return claims;
		}

		//! Método privado para validar um token JWT
		private Boolean validateToken(string token) {
			var tokenValidationParameters = new TokenValidationParameters {
				ValidIssuer = _configuration.Issuer,
				ValidAudience = _configuration.Audience,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Secret)),
				ClockSkew = TimeSpan.Zero,
				ValidateIssuer = false,
				ValidateAudience = false,
				ValidateLifetime = false,
				ValidateIssuerSigningKey = true,
			};
			var tokenHandler = new JwtSecurityTokenHandler();
			SecurityToken securityToken;

			//! Valida o token JWT
			tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
			var jwtSecurityToken = securityToken as JwtSecurityToken;

			//! Verifica se o algoritmo de assinatura é válido
			if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCulture))
				return false;
			return true;
		}
	}
}
