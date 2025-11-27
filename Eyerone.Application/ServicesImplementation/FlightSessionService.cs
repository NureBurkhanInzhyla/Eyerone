using Eyerone.Application.DTOs;
using Eyerone.Application.RepositoriesInterfaces;
using Eyerone.Application.ServicesInterfaces;
using Eyerone.Domain.Models;
using Eyerone.Domain.ServicesInterfaces;
using System.Text.Json;

namespace Eyerone.Application.ServicesImplementation
{
    public class FlightSessioNotFoundException : Exception
    {
        public FlightSessioNotFoundException(string message) : base(message) { }
    }
    public class FlightSessionService : IFlightSessionService
    {
        private readonly IFlightSessionRepository _repository;
        private readonly ITelemetryService _telemetryService;

        private FlightSessionDto MapToDto(FlightSession session)
        {
            return new FlightSessionDto
            {
                SessionId = session.SessionId,
                StartedAt = session.StartedAt,
                EndedAt = session.EndedAt,
                DroneId = session.DroneId,
                Metadata = session.Metadata
            };
        }
        public FlightSessionService(IFlightSessionRepository repository, ITelemetryService telemetryService)
        {
            _repository = repository;
            _telemetryService = telemetryService;
        }

        public async Task<IEnumerable<FlightSessionDto>> GetAllSessionsAsync()
        {
            var sessions = await _repository.GetAllAsync();
            return sessions.Select(MapToDto);
        }

        public async Task<FlightSessionDto> GetSessionByIdAsync(int id)
        {
            var session = await _repository.GetByIdAsync(id);
            if (session == null)
                throw new FlightSessioNotFoundException("Flight session not found");

            return new FlightSessionDto
            {
                SessionId = session.SessionId,
                StartedAt = session.StartedAt,
                EndedAt = session.EndedAt,
                DroneId = session.DroneId,
                Metadata = session.Metadata 
            };
        }

        public async Task<FlightSession> AddSessionAsync(FlightSession session)
        {
            return await _repository.AddAsync(session);
        }

        public async Task DeleteSessionAsync(int id)
        {
            var session = await _repository.GetByIdAsync(id);
            if (session != null)
                await _repository.DeleteAsync(id);
        }

        public async Task<FlightSessionDto> EndSessionAsync(int id)
        {
            var session = await _repository.GetByIdAsync(id);

            if (session.EndedAt != null)
                throw new InvalidOperationException("Session already ended");

            session.EndedAt = DateTime.UtcNow;

            var duration = session.EndedAt.Value - session.StartedAt;

            await _repository.UpdateAsync(session);

            var avgSpeed = await _telemetryService.GetAverageSpeedAsync(id);

            var recommendations = new List<string>();

            if (avgSpeed.HasValue)
            {
                if (avgSpeed.Value > 10)
                    recommendations.Add("Check high-speed stability");
            }

            if (duration > TimeSpan.FromMinutes(30))
                recommendations.Add("Battery may need replacement");

            return new FlightSessionDto
            {
                SessionId = session.SessionId,
                StartedAt = session.StartedAt,
                EndedAt = session.EndedAt,
                Duration = duration,
                AverageSpeed = avgSpeed.HasValue ? avgSpeed.Value : 0,
                Recommendations = recommendations
            };
        }

    }
}
