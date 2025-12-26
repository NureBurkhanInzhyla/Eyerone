using Eyerone.Domain.Models;

namespace Eyerone.Application.ServicesInterfaces
{
    public interface ITelemetryService
    {
        Task<IEnumerable<Telemetry>> GetAllTelemetryAsync(int limit = 100);
        Task<Telemetry> GetTelemetryByIdAsync(int id);
        Task<IEnumerable<Telemetry>> GetTelemetryByFlightAsync(int flightId);
        Task<Telemetry> AddTelemetryAsync(Telemetry telemetry);
        Task<double?> GetAverageSpeedAsync(int flightId);
        Task<double?> GetAverageBatteryLevel(int flightId);

    }
}
