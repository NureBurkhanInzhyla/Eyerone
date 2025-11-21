using Eyerone.Application.DTOs;
using Eyerone.Application.RepositoriesInterfaces;
using Eyerone.Application.ServicesInterfaces;
using Eyerone.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eyerone.Application.ServicesImplementation
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }
    public class CommandService: ICommandService
    {
        private readonly ICommandRepository _commandRepository;
        public CommandService(ICommandRepository commandRepository)
        {
            _commandRepository = commandRepository;
        }

        private CommandDTO MapToDto(Command command)
        {
            return new CommandDTO
            {
                CommandId = command.CommandId,
                DroneId = command.DroneId,
                CommandParameters = command.Parameters,
                Status = command.Status,
                CreatedAt = command.CreatedAt
            };
        }
        public async Task<IEnumerable<CommandDTO>> GetAllCommandsAsync() 
        {
            var commands = await _commandRepository.GetAllCommandsAsync();
            return commands.Select(MapToDto);
        }
        public async Task<IEnumerable<CommandDTO>> GetCommandsByDroneIdAsync(int droneId)
        {
            var commands = await _commandRepository.GetByDroneIdAsync(droneId);

            if (!commands.Any())
            {
                throw new NotFoundException($"No commands found for drone ID {droneId}.");
            }

            return commands.Select(MapToDto);
        }
        public async Task<CommandDTO> GetCommandByIdAsync(int id) 
        {
            var command = await _commandRepository.GetByIdAsync(id);

            if (command == null)
            {
                throw new NotFoundException($"Command with ID {id} not found.");
            }

            return MapToDto(command);
        }

        public async Task<CommandDTO> CreateCommandAsync(Command command) 
        {
            if(command.DroneId == 0) {
                throw new NotFoundException($"Drone with ID {command.DroneId} not found.");
            }

            var createdCommand = await _commandRepository.AddAsync(command);
            return MapToDto(createdCommand);
        }

        public async Task DeleteCommandAsync(int id)
        {
            var command = await _commandRepository.GetByIdAsync(id);
            if (command == null)
            {
                throw new NotFoundException($"Command with ID {id} not found.");
            }

            if(command.Status == "pending")
            {
                throw new Exception($"Command with ID {id} is executing and can not be deleted.");
            }

            await _commandRepository.DeleteAsync(id);
        }
    }
}
