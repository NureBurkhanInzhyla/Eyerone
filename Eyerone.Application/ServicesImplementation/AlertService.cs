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
