using Auth.API.Models.DTOs;

namespace Auth.API.Services
{
    public interface IAuthService
    {
        Task<UserDto> RegisterAsync(RegisterDto model);
        Task<string> LoginAsync(LoginDto model);
    }
}
