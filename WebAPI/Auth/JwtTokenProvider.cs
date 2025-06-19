using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebAPI.Auth
{
    public class JwtTokenProvider
    {
        public static string CreateToken(string secureKey, int expiration, string email = null, List<string?> roles = null)
        {
            var tokenKey = Encoding.UTF8.GetBytes(secureKey);

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, email),
                    new Claim(JwtRegisteredClaimNames.Email, email),
                    new Claim(JwtRegisteredClaimNames.Sub, email)
                };

            foreach (var role in roles)
            {
                claims.Add(new Claim("role", role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(expiration),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityTokenHandler().CreateToken(tokenDescriptor));
        }
    }
}
