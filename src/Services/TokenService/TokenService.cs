using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AUTENTICADOR.src.Model.Token;
using Microsoft.IdentityModel.Tokens;

namespace AUTENTICADOR.src.Services.TokenService
{
	public class TokenService : ITokenService
	{
		private TokenModel _configuration;

		//! Construtor da classe TokenService
		public TokenService(TokenModel configuration)
		{
			_configuration = configuration;
		}

		//! Método para gerar um token de acesso
		public string GenerateAccesToken(IEnumerable<Claim> claims)
		{
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

		//! Método para gerar um token de atualização
		public string GenerateRefreshToken()
		{
			var randomNumber = new byte[32];
			using (var rng = RandomNumberGenerator.Create())
			{
				rng.GetBytes(randomNumber);
				return Convert.ToBase64String(randomNumber);
			}
		}

		//! Método para obter o principal de um token JWT expirado
		public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
		{
			var tokenValidationParameters = new TokenValidationParameters
			{
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
			ClaimsPrincipal principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
			var jwtSecurityToken = securityToken as JwtSecurityToken;

			//! Verifica se o algoritmo de assinatura é válido
			if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCulture))
				throw new SecurityTokenException("Invalid Token.");

			return principal;
		}
	}
}
