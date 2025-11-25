using BCrypt.Net;
using Eyerone.Application.DTOs;
using Eyerone.Application.RepositoriesInterfaces;
using Eyerone.Domain.Models;
using Eyerone.Domain.ServicesInterfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Eyerone.Application.ServicesImplementation
{
    public class UserServiceException : Exception
    {
        public UserServiceException() { }

        public UserServiceException(string message)
            : base(message) { }

        public UserServiceException(string message, Exception innerException)
            : base(message, innerException) { }
    }
    public class UserService: IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAlertRepository _alertRepository;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository userRepository, IAlertRepository alertRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _alertRepository = alertRepository;
            _configuration = configuration;
        }
        private UserDto MapToDto(User user)
        {
            return new UserDto
            {
                UserId = user.UserId,
                Email = user.Email,
                Username = user.Username,
                CreatedAt = user.CreatedAt
            };
        }
        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]
                              ?? throw new InvalidOperationException("JWT Key not configured.")));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(double.Parse(_configuration["Jwt:ExpiresInHours"] ?? "1")),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<UserDto> RegisterAsync(UserRegisterDTO registerDto)
        {
            var isUnique = await _userRepository.IsEmailUniqueAsync(registerDto.Email);
            if (!isUnique)
            {
                throw new UserServiceException($"A user with email '{registerDto.Email}' already exists.");
            }

            if (string.IsNullOrWhiteSpace(registerDto.Password) || registerDto.Password.Length < 8)
            {
                throw new UserServiceException("Password must be at least 8 characters long.");
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            var newUser = new User
            {
                Email = registerDto.Email,
                PasswordHash = passwordHash,
                Username = registerDto.Username
            };

            var createdUser = await _userRepository.AddAsync(newUser);

            return MapToDto(createdUser);
        }

        public async Task<string> LoginAsync(UserLoginDTO loginDto)
        {
            var user = await _userRepository.GetByEmailAsync(loginDto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                throw new UserServiceException("Invalid login credentials.");
            }

            var token = GenerateJwtToken(user);

            return token;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();

            return users.Select(MapToDto);
        }
        public async Task<UserDto> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                throw new UserServiceException($"User with id {id} was not found.");
            }

            return MapToDto(user);
        }
        public async Task<IEnumerable<object>> GetUserAlertsAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new UserServiceException($"User with id {userId} was not found.");

            var alerts = await _alertRepository.GetByUserIdAsync(userId);

            return alerts.Select(a => (object)a);
        }
        public async Task<UserDto> ChangeRoleAsync(int id, string newRole)
        {
            var user = await _userRepository.GetByIdAsync(id)
                ?? throw new UserServiceException("User not found.");

            if (newRole != "admin" && newRole != "operator")
                throw new UserServiceException("Invalid role.");

            user.Role = newRole;

            var updated = await _userRepository.UpdateAsync(user);
            return MapToDto(updated);
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new UserServiceException($"User with id {id} was not found.");
            }

            await _userRepository.DeleteAsync(id);
        }
    }

}
