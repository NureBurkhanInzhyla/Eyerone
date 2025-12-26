using Eyerone.Application.DTOs;
using Eyerone.Domain.Models;

namespace Eyerone.Application.ServicesInterfaces
{
    public interface ICommandService
    {
        Task<IEnumerable<CommandDTO>> GetAllCommandsAsync();
        Task<IEnumerable<CommandDTO>> GetCommandsByDroneIdAsync(int droneId);
        Task<CommandDTO> GetCommandByIdAsync(int id);
        Task<CommandDTO> CreateCommandAsync(CommandDTO command);
        Task<CommandDTO> GetLatestCommandForDrone(int droneId);
        Task DeleteCommandAsync(int id);
    }
}
