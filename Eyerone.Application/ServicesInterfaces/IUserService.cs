using Eyerone.Application.DTOs;
using Eyerone.Domain.Models;

namespace Eyerone.Domain.ServicesInterfaces
{
    public interface IUserService
    {
        Task<UserDto> RegisterAsync(UserRegisterDTO registerDto);

        Task<string> LoginAsync(UserLoginDTO loginDto);

        Task<UserDto> GetByIdAsync(int id);
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto> ChangeRoleAsync(int id, string newRole);
        Task<IEnumerable<object>> GetUserAlertsAsync(int userId);
        Task DeleteUserAsync(int id);
    }
}
