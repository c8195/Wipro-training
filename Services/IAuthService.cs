using DoConnect.API.Models.DTOs;

namespace DoConnect.API.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<UserDto> GetUserByIdAsync(int userId);
        Task<bool> IsUserAdminAsync(int userId);
    }
}