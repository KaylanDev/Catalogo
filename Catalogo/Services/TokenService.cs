using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Catalogo.Services
{
    public class TokenService : ITokenService
    {
        public JwtSecurityToken GenerateAcessToken(IEnumerable<Claim> claims, IConfiguration _config)
        {
            var secretKey = _config.GetSection("JWT").GetValue<string>("SecretKey")?? throw new ArgumentException("secret key is invalid!");

            var privateKey = Encoding.UTF8.GetBytes(secretKey);

            var sigininCredentials = new SigningCredentials(new SymmetricSecurityKey(privateKey),SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_config.GetSection("JWT").GetValue<double>("TokenValidityInMinutes")),
                Audience = _config.GetSection("JWT").GetValue<string>("ValidAudience"),
                Issuer = _config.GetSection("JWT").GetValue<string>("ValidIssuer"),
                SigningCredentials = sigininCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);


            return token;
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token, IConfiguration _config)
        {
            var secretKey = _config["JWT:Secretkey"] ?? throw new InvalidOperationException("secretkey invalid!");

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ValidateLifetime = false
            };

            var tokenhandler = new JwtSecurityTokenHandler();

            var principal = tokenhandler.ValidateToken(token,tokenValidationParameters,out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("token invalid!");
            }

            return principal;
        }

        public string RegenerateRefreshToken()
        {
            var securityradombytes = new byte[128];

            var radomnumbergenerate = RandomNumberGenerator.Create();

            radomnumbergenerate.GetBytes(securityradombytes);

            var TokenRefresh = Convert.ToBase64String(securityradombytes);

            return TokenRefresh;

        }
    }
}
