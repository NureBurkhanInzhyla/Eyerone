using Eyerone.Domain.Models;

namespace Eyerone.Application.RepositoriesInterfaces
{
    public interface IDroneRepository
    {
        Task<IEnumerable<Drone>> GetAllAsync();
        Task<Drone> GetByIdAsync(int id);
        Task<Drone> AddAsync(Drone drone);
        Task DeleteAsync(int id);
    }
}
