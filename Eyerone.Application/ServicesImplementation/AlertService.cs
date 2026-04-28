using Eyerone.Application.DTOs;
using Eyerone.Application.RepositoriesInterfaces;
using Eyerone.Application.ServicesInterfaces;
using Eyerone.Domain.Models;

namespace Eyerone.Application.ServicesImplementation
{
   
    public class AlertService: IAlertService
    {
        private readonly IAlertRepository _alertRepository;

        public AlertService(IAlertRepository alertRepository)
        {
            _alertRepository = alertRepository;
        }

        public Task<IEnumerable<Alert>> GetAllAlertsAsync()
        {
            return _alertRepository.GetAllAsync();
        }
          

        public Task<Alert> GetAlertByIdAsync(int id)
        {
            return _alertRepository.GetByIdAsync(id);

        }
        public async Task<IEnumerable<AlertDTO>> GetAlertsByUserIdAsync(int userId)
        {
            var alerts = await _alertRepository.GetByUserIdAsync(userId);
            var alertsDto = alerts.Select(a => new AlertDTO
            {
                AlertId = a.AlertId,
                UserId = a.UserId,
                DroneId = a.DroneId,
                DroneName = a.Drone?.DroneName ?? "Unknown",
                Type = a.Type,
                Text = a.Text,
                IsRead = a.IsRead,
                CreatedAt = a.CreatedAt
            });

            return alertsDto;
        }
        public Task<Alert> CreateAlertAsync(Alert alert)
        {
            return _alertRepository.AddAsync(alert);
        }
           

        public Task<Alert> ReadAlertAsync(int id)
        {
            return _alertRepository.MarkAsReadAsync(id);
        }
           

        public Task DeleteAlertAsync(int id)
        {
            return _alertRepository.DeleteAsync(id);
        }
          
    }
}
