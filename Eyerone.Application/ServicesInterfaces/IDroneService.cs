using Eyerone.Application.DTOs;
using Eyerone.Domain.Models;

namespace Eyerone.Application.ServicesInterfaces
{
    public interface IDroneService
    {
        Task<IEnumerable<DroneDto>> GetAllDronesAsync();
        Task<DroneDto> GetDroneByIdAsync(int id);
        Task<DroneDto> CreateDroneAsync(AddDroneDTO drone);
        Task<IEnumerable<DroneDto>> GetUserDrones(int userId);
        Task<string> AuthenticateDroneAsync(string serialNumber);
        IEnumerable<string> GetAvailableSerials();

        Task<int> GetDroneIdBySerialAsync(string serialNumber);
        Task DeleteDroneAsync(int id);
    }
}
