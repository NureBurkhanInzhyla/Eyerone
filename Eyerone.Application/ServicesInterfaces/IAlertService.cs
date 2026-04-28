using Eyerone.Application.DTOs;
using Eyerone.Domain.Models;

namespace Eyerone.Application.ServicesInterfaces
{
    public interface IAlertService
    {
        Task<IEnumerable<Alert>> GetAllAlertsAsync();
        Task<Alert> GetAlertByIdAsync(int id);
        Task<Alert> CreateAlertAsync(Alert alert);
        Task<Alert> ReadAlertAsync(int id);
        Task<IEnumerable<AlertDTO>> GetAlertsByUserIdAsync(int userId);
        Task DeleteAlertAsync(int id);
    }
}
