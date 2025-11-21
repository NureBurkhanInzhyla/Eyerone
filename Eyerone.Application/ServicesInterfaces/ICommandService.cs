using Eyerone.Application.DTOs;
using Eyerone.Domain.Models;

namespace Eyerone.Application.ServicesInterfaces
{
    public interface ICommandService
    {
        Task<IEnumerable<CommandDTO>> GetAllCommandsAsync();
        Task<IEnumerable<CommandDTO>> GetCommandsByDroneIdAsync(int droneId);
        Task<CommandDTO> GetCommandByIdAsync(int id);
        Task<CommandDTO> CreateCommandAsync(Command command);
        Task DeleteCommandAsync(int id);
    }
}
