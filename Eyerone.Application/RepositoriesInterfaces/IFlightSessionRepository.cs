using Eyerone.Domain.Models;

namespace Eyerone.Application.RepositoriesInterfaces
{
    public interface IFlightSessionRepository
    {
        Task<IEnumerable<FlightSession>> GetAllAsync();
        Task<FlightSession> GetByIdAsync(int id);
        Task<FlightSession> AddAsync(FlightSession session);
        Task UpdateAsync(FlightSession session);
        Task DeleteAsync(int id);
    }
}
