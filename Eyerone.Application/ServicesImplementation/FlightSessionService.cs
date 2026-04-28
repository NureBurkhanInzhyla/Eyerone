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

        private async Task<FlightSessionDto> ProcessFlightSummaryAsync(FlightSession session)
        {
            var avgSpeed = await _telemetryService.GetAverageSpeedAsync(session.SessionId);
            var avgBattery = await _telemetryService.GetAverageBatteryLevel(session.SessionId);

            var recommendations = new List<string>();

            if (avgSpeed.HasValue && avgSpeed.Value > 10)
            {
                recommendations.Add("Check high-speed stability");
            }

            if (avgBattery < 20)
            {
                recommendations.Add("Consider charging battery before next flight");
            }

            return new FlightSessionDto
            {
                SessionId = session.SessionId,
                StartedAt = session.StartedAt,
                EndedAt = session.EndedAt,
                Duration = session.EndedAt.HasValue ? session.EndedAt.Value - session.StartedAt : null,
                DroneId = session.DroneId,
                DroneName = session.Drone?.DroneName ?? "Unknown Drone",
                AverageSpeed = avgSpeed ?? 0,
                Recommendations = recommendations,
                Metadata = session.Metadata
            };
        }

        public async Task<IEnumerable<FlightSessionDto>> GetSessionsByUserIdAsync(int userId)
        {
            var sessions = (await _repository.GetSessionsByUserId(userId)).ToList();
            var sessionsDto = new List<FlightSessionDto>();

            foreach (var session in sessions)
            {
                sessionsDto.Add(await ProcessFlightSummaryAsync(session));
            }
            return sessionsDto;
        }

        public async Task<FlightSession> AddSessionAsync(int droneId)
        {
            var session = new FlightSession
            {
                DroneId = droneId,
            };
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

            return await ProcessFlightSummaryAsync(session);
        }

    }
}
