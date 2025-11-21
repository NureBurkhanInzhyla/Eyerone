using Eyerone.Domain.Models;

namespace Eyerone.Application.RepositoriesInterfaces
{
    public interface ITelemetryRepository
    {
        Task<IEnumerable<Telemetry>> GetAllAsync(int limit = 100);
        Task<Telemetry?> GetByIdAsync(int id);
        Task<IEnumerable<Telemetry>> GetByFlightIdAsync(int flightId);
        Task<Telemetry> AddAsync(Telemetry telemetry);
    }
}
