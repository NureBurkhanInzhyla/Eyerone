using Eyerone.Application.DTOs;
using Eyerone.Application.RepositoriesInterfaces;
using Eyerone.Application.ServicesInterfaces;
using Eyerone.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Eyerone.Application.ServicesImplementation
{
    public class DroneNotFoundException : Exception
    {
        public DroneNotFoundException(string message) : base(message) { }
    }
    public class InvalidSerialNumberException : Exception
    {
        public InvalidSerialNumberException(string message) : base(message) { }
    }

    public class DroneService: IDroneService
    {
        private readonly IDroneRepository _droneRepository;
        private readonly IUserRepository _userRepository;
        private readonly IFlightSessionRepository _flightRepository;
        private readonly IConfiguration _configuration;

        private static readonly Dictionary<string, DateTime> _discoveredSerials = new Dictionary<string, DateTime>();

        public DroneService(IDroneRepository droneRepository, IUserRepository userRepository, IFlightSessionRepository flightRepository, IConfiguration configuration)
        {
            _droneRepository = droneRepository;
            _userRepository = userRepository;
            _flightRepository = flightRepository;
            _configuration = configuration;
        }
        private DroneDto MapToDto(Drone drone)
        {
            return new DroneDto
            {
                DroneId = drone.DroneId,
                Name = drone.DroneName,
                Model = drone.Model,
                SerialNumber = drone.SerialNumber,
                OwnerId = drone.OwnerId,


                Owner = drone.Owner == null ? null : new UserDto
                {
                    UserId = drone.Owner.UserId,
                    Username = drone.Owner.Username,
                    Email = drone.Owner.Email
                }
            };
        }
        public async Task<IEnumerable<DroneDto>> GetUserDrones(int userId)
        {
            var drones = await _droneRepository.GetUserDrones(userId);
            return drones.Select(MapToDto);
        }

        public async Task<IEnumerable<DroneDto>> GetAllDronesAsync()
        {
            var drones = await _droneRepository.GetAllAsync();
            return drones.Select(MapToDto);
        }
        public async Task<DroneDto> GetDroneByIdAsync(int id)
        {
            var drone = await _droneRepository.GetByIdAsync(id);

            if (drone == null)
                throw new DroneNotFoundException("Drone not found");

            return MapToDto(drone);
        }
        public async Task<DroneDto> CreateDroneAsync(AddDroneDTO drone)
        {
            var user = await _userRepository.GetByIdAsync(drone.OwnerId);

            if (user == null)
                throw new UserServiceException("Owner not found");

            var newDrone = new Drone
            {
                DroneName = drone.Name,
                Model = drone.Model,
                SerialNumber = drone.SerialNumber,
                OwnerId = drone.OwnerId

            };
            //ValidateSerialNumber(newDrone.SerialNumber);

            try
            {
                var createdDrone = await _droneRepository.AddAsync(newDrone);
                _discoveredSerials.Remove(createdDrone.SerialNumber);
                return MapToDto(createdDrone);
            }
            catch (DbUpdateException ex) when
                (ex.InnerException?.Message.Contains("serial_number") == true ||
                 ex.Message.Contains("serial_number"))
            {
                throw new Exception("A drone with this serial number already exists.");
            }
        }
        //private void ValidateSerialNumber(string serialNumber)
        //{
        //    if (string.IsNullOrWhiteSpace(serialNumber))
        //        throw new InvalidSerialNumberException("Serial number cannot be empty.");

        //    if (serialNumber.Length < 8 || serialNumber.Length > 20)
        //        throw new InvalidSerialNumberException("Serial number must be between 8 and 20 characters long.");

        //    if (!Regex.IsMatch(serialNumber, "^[A-Z0-9]+$"))
        //        throw new InvalidSerialNumberException("Serial number may contain only uppercase Latin letters and digits.");
        //}
        public async Task<string?> AuthenticateDroneAsync(string serialNumber)
        {
            var droneId = await _droneRepository.GetDroneIdBySerial(serialNumber);
            if (droneId == -1)
            {
                _discoveredSerials[serialNumber] = DateTime.UtcNow;
                return null;
            }

            return GenerateJwtTokenForDrone(droneId);
        }
        public async Task<int> GetDroneIdBySerialAsync(string serialNumber)
        {
            var droneId = await _droneRepository.GetDroneIdBySerial(serialNumber);
            if (droneId == -1)
                throw new DroneNotFoundException("Drone not found");
            return droneId;
        }
        public async Task DeleteDroneAsync(int id)
        {
            var drone = await _droneRepository.GetByIdAsync(id);
            if (drone == null)
                throw new DroneNotFoundException("Drone not found");

            var activeSessions = await _flightRepository.GetActiveSessionsByDroneIdAsync(id);


            if (activeSessions.Any())
                throw new Exception("Cannot delete drone: it has active flight sessions.");

            await _droneRepository.DeleteAsync(id);
        }
        public IEnumerable<string> GetAvailableSerials()
        {
            var timeout = DateTime.UtcNow.AddMinutes(-5);

            return _discoveredSerials
                .Where(x => x.Value > timeout)
                .Select(x => x.Key)
                .ToList();
        }

        private string GenerateJwtTokenForDrone(int droneId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, droneId.ToString()),
                new Claim(ClaimTypes.Role, "Drone")
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
