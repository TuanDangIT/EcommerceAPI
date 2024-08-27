using Ecommerce.Shared.Abstractions.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Infrastructure.Auth
{
    public sealed class AuthManager : IAuthManager
    {
        private readonly AuthOptions _authOptions;
        private readonly TimeProvider _timeProvider;
        private readonly SigningCredentials _signingCredentials;

        public AuthManager(AuthOptions authOptions, TimeProvider timeProvider)
        {
            _authOptions = authOptions;
            _timeProvider = timeProvider;
            _signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authOptions.IssuerSigningKey)), SecurityAlgorithms.HmacSha256);
        }
        public JsonWebToken GenerateAccessToken(string userId, string username, string role = "Customer")
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("User ID claim (subject) cannot be empty.", nameof(userId));
            }


            var now = _timeProvider.GetUtcNow().LocalDateTime;
            var expires = now.AddMinutes(_authOptions.ExpiryInMinutes);

            var jwtClaims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, userId.ToString()),
                new(ClaimTypes.Name, username),
                new(ClaimTypes.Role, role)
            };

            var jwt = new JwtSecurityToken(
                issuer: _authOptions.Issuer,
                audience: _authOptions.Audience,
                claims: jwtClaims,
                notBefore: now,
                expires: expires,
                signingCredentials: _signingCredentials
            );

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new JsonWebToken
            {
                AccessToken = token,
                RefreshToken = GenerateRefreshToken(),
                AccessTokenExpiryTime = expires,
                Id = userId,
                Role = role,
            };
        }
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authOptions.IssuerSigningKey)),
                ValidateLifetime = false
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }
    }
}
