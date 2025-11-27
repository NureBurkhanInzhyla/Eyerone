
using Eyerone.Application.DTOs;
using Eyerone.Domain.Models;

namespace Eyerone.Application.ServicesInterfaces
{
    public interface IFlightSessionService
    {
        Task<IEnumerable<FlightSessionDto>> GetAllSessionsAsync();
        Task<FlightSessionDto> GetSessionByIdAsync(int id);
        Task<FlightSession> AddSessionAsync(FlightSession session);
        Task<FlightSessionDto> EndSessionAsync(int id);
        Task DeleteSessionAsync(int id);
    }

}
