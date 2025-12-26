using Eyerone.Domain.Models;
using Eyerone.Infrastructure.Data;
using Eyerone.Application.RepositoriesInterfaces;
using Microsoft.EntityFrameworkCore;


namespace Eyerone.Infrastructure.RepositoriesImplementations
{
    public class CommandRepository : ICommandRepository
    {
        private readonly ApplicationDbContext _context;

        public CommandRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Command>> GetAllCommandsAsync()
        {
            return await _context.Commands.ToListAsync();
        }

        public async Task<Command> GetByIdAsync(int id)
        {
            return await _context.Commands.FindAsync(id);
        }
        public async Task<Command> GetLatestCommandForDrone(int droneId)
        {
            var command = await _context.Commands
              .Where(c => c.DroneId == droneId && c.Status == "pending")
              .OrderByDescending(c => c.CreatedAt)
              .FirstOrDefaultAsync();
            
            return command;
        }
        public async Task<IEnumerable<Command>> GetByDroneIdAsync(int droneId)
        {
            return await _context.Commands
               .Where(c => c.DroneId == droneId)
               .OrderByDescending(c => c.CreatedAt)
               .ToListAsync();
        }

        public async Task<Command> AddAsync(Command command)
        {
            _context.Commands.Add(command);

            await _context.SaveChangesAsync();
            return command;
        }
        public async Task<Command> UpdateAsync(Command command)
        {
            _context.Commands.Update(command);
            await _context.SaveChangesAsync();
            return command;
        }
        public async Task DeleteAsync(int id)
        {
            var command = await GetByIdAsync(id);
            if (command != null)
            {
                _context.Commands.Remove(command);
                await _context.SaveChangesAsync();
            }
        }
    }

}



