using Eyerone.Domain.Models;
using Eyerone.Infrastructure.Data;
using Eyerone.Application.RepositoriesInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Eyerone.Infrastructure.RepositoriesImplementations
{

    public class AlertRepository: IAlertRepository
    {
        private readonly ApplicationDbContext _context;

        public AlertRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Alert>> GetAllAsync() =>
        await _context.Alerts.OrderByDescending(a => a.CreatedAt).ToListAsync();

        public async Task<Alert?> GetByIdAsync(int id)
        {
            return await _context.Alerts.FindAsync(id);
        }

        public async Task<IEnumerable<Alert>> GetByUserIdAsync(int userId) =>
            await _context.Alerts
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

        public async Task<Alert> AddAsync(Alert alert)
        {
            _context.Alerts.Add(alert);
            await _context.SaveChangesAsync();
            return alert;
        }

        public async Task<Alert> MarkAsReadAsync(int id)
        {
            var alert = await _context.Alerts.FindAsync(id);
            if (alert != null)
            {
                alert.IsRead = true;
                await _context.SaveChangesAsync();
            }
            return alert;
        }

        public async Task DeleteAsync(int id)
        {
            var alert = await _context.Alerts.FindAsync(id);
            if (alert != null)
            {
                _context.Alerts.Remove(alert);
                await _context.SaveChangesAsync();
            }
        }
    }
}
