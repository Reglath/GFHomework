using Homework.Models.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Numerics;
using System.Security.Claims;
using System.Text;

namespace HomeworkTest.Helpers
{
    public class JwtHelper
    {
        private readonly IConfiguration _configuration;

        public JwtHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateJwtToken()
        {
            return CreateToken(new LoginDTO { Username = "username", Password = "password"});
        }

        private string CreateToken(LoginDTO loginDTO)
        {
            var claims = new List<Claim>()
        {
            new Claim("Username", loginDTO.Username.ToString()),
        };
            var keyString = _configuration.GetSection("JWTToken").Value;
            keyString ??= Environment.GetEnvironmentVariable("JWTToken");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
