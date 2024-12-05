using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TchauDengue.Entities;

namespace TchauDengue.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey Key;
        public TokenService(IConfiguration configuration)
        {
            this.Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenKey"]));
              
        }
        public string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.UserName),
                new Claim(ClaimTypes.Role, user.Role)
            };

            SigningCredentials creds = new SigningCredentials(this.Key, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(4),
                SigningCredentials = creds
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
