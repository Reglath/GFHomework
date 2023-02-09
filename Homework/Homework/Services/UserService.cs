using Homework.Contexts;
using Homework.Models.DTOs;
using Homework.Models.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Numerics;
using System.Security.Claims;

namespace Homework.Services
{
    public class UserService : IUserService
    {
        private ApplicationDbContext context;
        private IConfiguration config;

        public UserService(ApplicationDbContext context, IConfiguration config)
        {
            this.context = context;
            this.config = config;
        }

        public RegisterResponseDTO Register(RegisterDTO registerDTO)
        {
            if (registerDTO == null)
                return new RegisterResponseDTO() { Status = 400, Message = "invalid input" };
            if (registerDTO.Username == null || registerDTO.Username.Length == 0)
                return new RegisterResponseDTO() { Status = 400, Message = "username required" };
            if (registerDTO.Password == null || registerDTO.Password.Length == 0)
                return new RegisterResponseDTO() { Status = 400, Message = "password required" };
            if (context.Users.Any(u => u.Username.Equals(registerDTO.Username)))
                return new RegisterResponseDTO() { Status = 400, Message = "username is already taken" };

            AddNewUser(registerDTO);
            return new RegisterResponseDTO() { Status = 201, Message = "user created" };
        }

        public LoginResponseDTO Login(LoginDTO loginDTO)
        {
            if (loginDTO == null)
                return new LoginResponseDTO() { Status = 400, Message = "invalid input" };
            if (loginDTO.Username == null)
                return new LoginResponseDTO() { Status = 400, Message = "username required" };
            if (loginDTO.Password == null)
                return new LoginResponseDTO() { Status = 400, Message = "password required" };
            if (!context.Users.Any(u => u.Username.Equals(loginDTO.Username)))
                return new LoginResponseDTO() { Status = 400, Message = "username not registered" };
            if (!context.Users.FirstOrDefault(u => u.Username == loginDTO.Username).Password.Equals(loginDTO.Password))
                return new LoginResponseDTO() { Status = 400, Message = "incorrect password" };

            CreateJWTToken(loginDTO);
            return new LoginResponseDTO() { Status = 200, Message = "user logged in, current amount of greenBay dollars is: " + context.Users.FirstOrDefault(u => u.Username == loginDTO.Username).Money };
        }

        public LoginResponseDTO GetJWT(LoginDTO loginDTO)
        {
            if (loginDTO == null)
                return new LoginResponseDTO() { Status = 400, Message = "invalid input" };
            if (loginDTO.Username == null)
                return new LoginResponseDTO() { Status = 400, Message = "username required" };
            if (loginDTO.Password == null)
                return new LoginResponseDTO() { Status = 400, Message = "password required" };
            if (!context.Users.Any(u => u.Username.Equals(loginDTO.Username)))
                return new LoginResponseDTO() { Status = 400, Message = "username not registered" };
            if (!context.Users.FirstOrDefault(u => u.Username == loginDTO.Username).Password.Equals(loginDTO.Password))
                return new LoginResponseDTO() { Status = 400, Message = "incorrect password" };

            var message = CreateJWTToken(loginDTO);
            return new LoginResponseDTO() { Status = 200, Message = message };
        }

        private void AddNewUser(RegisterDTO registerDTO)
        {
            context.Users.Add(new User() { Username = registerDTO.Username, Password = registerDTO.Password, Money = 0 });
            context.SaveChanges();
        }

        private string CreateJWTToken(LoginDTO loginDTO)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim("Username", loginDTO.Username.ToString()),
            };

            var keyString = config.GetSection("JWTToken").Value;
            keyString ??= Environment.GetEnvironmentVariable("JWTToken");
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(keyString));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
