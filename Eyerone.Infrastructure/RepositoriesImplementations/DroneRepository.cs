using Eyerone.Domain.Models;
using Eyerone.Infrastructure.Data;
using Eyerone.Application.RepositoriesInterfaces;
using Microsoft.EntityFrameworkCore;


namespace Eyerone.Infrastructure.RepositoriesImplementations
{
    
    public class DroneRepository : IDroneRepository
    {
        private readonly ApplicationDbContext _context;

        public DroneRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Drone>> GetAllAsync()
        {
            return await _context.Drones.ToListAsync();
        }

        public async Task<Drone> GetByIdAsync(int id)
        {
            return await _context.Drones.FindAsync(id);
        }

        public async Task<Drone> AddAsync(Drone drone)
        {
            _context.Drones.Add(drone);
            await _context.SaveChangesAsync();
            return drone;
        }

        public async Task DeleteAsync(int id)
        {
            var drone = await _context.Drones.FindAsync(id);
            if (drone != null)
            {
                _context.Drones.Remove(drone);
                await _context.SaveChangesAsync();
            }
        }
    }

}
