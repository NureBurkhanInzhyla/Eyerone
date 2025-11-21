

using Eyerone.Application.DTOs;
using Eyerone.Domain.Models;

namespace Eyerone.Application.ServicesInterfaces
{
    public interface IFlightSessionService
    {
        Task<IEnumerable<FlightSession>> GetAllSessionsAsync();
        Task<FlightSession> GetSessionByIdAsync(int id);
        Task<FlightSession> AddSessionAsync(FlightSession session);
        Task<FlightSessionDto> EndSessionAsync(int id);
        Task DeleteSessionAsync(int id);
    }

}
