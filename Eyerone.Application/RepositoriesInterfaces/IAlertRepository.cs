using Eyerone.Domain.Models;

namespace Eyerone.Application.RepositoriesInterfaces
{
    public interface IAlertRepository
    {
        Task<IEnumerable<Alert>> GetAllAsync();
        Task<Alert> GetByIdAsync(int id);
        Task<IEnumerable<Alert>> GetByUserIdAsync(int userId);
        Task<Alert> AddAsync(Alert alert);
        Task<Alert> MarkAsReadAsync(int id);
        Task DeleteAsync(int id);
    }
}
