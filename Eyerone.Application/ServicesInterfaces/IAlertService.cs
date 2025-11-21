using Eyerone.Domain.Models;

namespace Eyerone.Application.ServicesInterfaces
{
    public interface IAlertService
    {
        Task<IEnumerable<Alert>> GetAllAlertsAsync();
        Task<Alert> GetAlertByIdAsync(int id);
        Task<Alert> CreateAlertAsync(Alert alert);
        Task<Alert> ReadAlertAsync(int id);
        Task DeleteAlertAsync(int id);
    }
}
