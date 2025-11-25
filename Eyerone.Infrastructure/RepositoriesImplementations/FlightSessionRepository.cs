using Eyerone.Domain.Models;
using Eyerone.Infrastructure.Data;
using Eyerone.Application.RepositoriesInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Eyerone.Infrastructure.RepositoriesImplementations
{
   
    public class FlightSessionRepository : IFlightSessionRepository
    {
        private readonly ApplicationDbContext _context;
        public FlightSessionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FlightSession>> GetAllAsync()
        {
            return await _context.FlightSessions.ToListAsync();
        }
            

        public async Task<FlightSession> GetByIdAsync(int id)
        {
            return await _context.FlightSessions.FindAsync(id);
        }

        public async Task<FlightSession> AddAsync(FlightSession session)
        {
            _context.FlightSessions.Add(session);
            await _context.SaveChangesAsync();
            return session;
        }
        public async Task UpdateAsync(FlightSession session)
        {
            _context.FlightSessions.Update(session);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<FlightSession>> GetActiveSessionsByDroneIdAsync(int droneId)
        {
            return await _context.FlightSessions
                .Where(s => s.DroneId == droneId && s.EndedAt == null)
                .ToListAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var session = await GetByIdAsync(id);
            if (session != null)
            {
                _context.FlightSessions.Remove(session);
                await _context.SaveChangesAsync();
            }
        }
    }

}
