using Eyerone.Application.RepositoriesInterfaces;
using Eyerone.Domain.Models;
using Eyerone.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Eyerone.Infrastructure.RepositoriesImplementations
{
    public class TelemetryRepository: ITelemetryRepository
    {
        private readonly ApplicationDbContext _context;

        public TelemetryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Telemetry>> GetAllAsync(int limit = 100)
        {
            return await _context.TelemetryData
                .OrderByDescending(t => t.Timestamp)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<Telemetry?> GetByIdAsync(int id)
        {
            return await _context.TelemetryData.FindAsync(id);
        }

        public async Task<IEnumerable<Telemetry>> GetByFlightIdAsync(int flightId)
        {
            return await _context.TelemetryData
                .Where(t => t.FlightId == flightId)
                .OrderBy(t => t.Timestamp)
                .ToListAsync();
        }

        public async Task<Telemetry> AddAsync(Telemetry telemetry)
        {
            _context.TelemetryData.Add(telemetry);
            await _context.SaveChangesAsync();
            return telemetry;
        }
        

    }
}
