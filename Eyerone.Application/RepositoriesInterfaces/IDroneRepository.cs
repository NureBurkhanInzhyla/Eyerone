using Eyerone.Domain.Models;

namespace Eyerone.Application.RepositoriesInterfaces
{
    public interface IDroneRepository
    {
        Task<IEnumerable<Drone>> GetAllAsync();
        Task<Drone> GetByIdAsync(int id);
        Task<Drone> AddAsync(Drone drone);
        Task<IEnumerable<Drone>> GetUserDrones(int userId);
        Task<int> GetDroneIdBySerial(string serialNumber);
        Task DeleteAsync(int id);
    }
}
