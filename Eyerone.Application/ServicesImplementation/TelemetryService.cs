using Eyerone.Application.RepositoriesInterfaces;
using Eyerone.Application.ServicesInterfaces;
using Eyerone.Domain.Models;
using System.Collections.Generic;

namespace Eyerone.Application.ServicesImplementation
{
    public class TelemetryNotFoundException : Exception
    {
        public TelemetryNotFoundException(string message) : base(message) { }
    }
    public class TelemetryService: ITelemetryService
    {
        private readonly ITelemetryRepository _repository;

        public TelemetryService(ITelemetryRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<Telemetry>> GetAllTelemetryAsync(int limit = 100)
        {
            return _repository.GetAllAsync(limit);
        }

        public async Task<Telemetry> GetTelemetryByIdAsync(int id)
        {
            var telemetry = await _repository.GetByIdAsync(id);
            if (telemetry == null)
                throw new NotFoundException("Telemetry not found");
            return telemetry;
        }

        public Task<IEnumerable<Telemetry>> GetTelemetryByFlightAsync(int flightId)
        {
            return _repository.GetByFlightIdAsync(flightId);
        }

        public Task<Telemetry> AddTelemetryAsync(Telemetry telemetry)
        {
            telemetry.Timestamp = DateTime.UtcNow;
            return _repository.AddAsync(telemetry);
        }
        public async Task<double?> GetAverageSpeedAsync(int flightId)
        {
            var telemetry = await _repository.GetByFlightIdAsync(flightId);
            if (!telemetry.Any()) return null;
            return telemetry.Average(t => t.Speed);
        }
    }
}
