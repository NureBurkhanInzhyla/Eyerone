using Eyerone.Domain.Models;

namespace Eyerone.Application.RepositoriesInterfaces { 
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIdAsync(int id);
        Task<User> AddAsync(User user);
        //Task<ActionResult<User>> GetUsersAlerts(int id);
        Task DeleteAsync(int id);
        Task<User?> GetByEmailAsync(string email);
        Task<bool> IsEmailUniqueAsync(string email);
    }

}
