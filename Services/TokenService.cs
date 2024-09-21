using Microsoft.IdentityModel.Tokens;
using Ordernary.Models;
using Ordernary.Services.ServiceInterfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ordernary.Services
{
    public class TokenService
    {
        public string SecretKey { get; set; }
        public int TokenDuration { get; set; }
        private readonly IConfiguration _config;

        public TokenService(IConfiguration configuration)
        {
            configuration = _config;
            SecretKey = "S3DJ8JDNafnhaj812222FAFAFADADADADADASASASASASASA";
            TokenDuration = 30;

        }
        public string GenerateToken(string appuserid, string name, string surname, string email, string role)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));

            var signature = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var payload = new[]
            {
                new Claim("appuserid",appuserid),
                new Claim("name",name),
                new Claim("surname",surname),
                new Claim("email",email),
                new Claim("role",role),
                


            };

            var jwtToken = new JwtSecurityToken(
                issuer: "localhost",
                audience: "localhost",
                claims: payload,
                expires: DateTime.Now.AddMinutes(TokenDuration),
                signingCredentials: signature
                );

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}
