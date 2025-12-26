using Eyerone.Application.DTOs;
using Eyerone.Domain.Models;

namespace Eyerone.Application.RepositoriesInterfaces
{
    public interface ICommandRepository
    {
        Task<IEnumerable<Command>> GetAllCommandsAsync();
        Task<Command> GetByIdAsync(int id);
        Task<IEnumerable<Command>> GetByDroneIdAsync(int droneId);
        Task<Command> UpdateAsync(Command command);
        Task<Command> AddAsync(Command command);
        Task<Command> GetLatestCommandForDrone(int droneId);
        Task DeleteAsync(int id);
    }
}
