using Homework.Models.DTOs;

namespace Homework.Services
{
    public interface IUserService
    {
        LoginResponseDTO GetJWT(LoginDTO loginDTO);
        LoginResponseDTO Login(LoginDTO loginDTO);
        RegisterResponseDTO Register(RegisterDTO registerDTO);
    }
}