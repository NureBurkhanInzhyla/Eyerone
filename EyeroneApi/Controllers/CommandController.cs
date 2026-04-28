using Eyerone.Application.DTOs;
using Eyerone.Application.ServicesImplementation;
using Eyerone.Application.ServicesInterfaces;
using Eyerone.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EyeroneApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommandController : ControllerBase
    {
        private readonly ICommandService _commandService;
        public CommandController(ICommandService commandService)
        {
            _commandService = commandService;
        }

        [HttpGet("getAll")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<CommandDTO>>> GetCommands() 
        {
            var commands = await _commandService.GetAllCommandsAsync();
            return Ok(commands);
        }

        [HttpGet("getAll/drone/{droneId}")]
        public async Task<ActionResult<IEnumerable<CommandDTO>>> GetCommandsByDroneId(int droneId) 
        {
            try
            {
                var commands = await _commandService.GetCommandsByDroneIdAsync(droneId);
                return Ok(commands);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<CommandDTO>> GetCommandById(int id)
        {
            try
            {
                var command = await _commandService.GetCommandByIdAsync(id);
                return Ok(command);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpPost("add")]
        [Authorize(Roles = "operator")]
        public async Task<ActionResult<CommandDTO>> AddCommand(CommandDTO command)
        {
            try
            {
                var createdCommand = await _commandService.CreateCommandAsync(command);

                return CreatedAtAction(nameof(GetCommandById), new { id = createdCommand.CommandId }, createdCommand);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("latest")]
        [Authorize(Roles = "Drone")]
        public async Task<ActionResult<CommandDTO>> GetLatestCommand()
        {
            var droneIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(droneIdClaim)) return Unauthorized();

            int droneId = int.Parse(droneIdClaim);
            var commandDto = await _commandService.GetLatestCommandForDrone(droneId);

            if (commandDto == null)
                return NoContent();

            return Ok(commandDto);
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteCommand(int id)
        {
            try
            {
                var command = await _commandService.GetCommandByIdAsync(id);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }

            await _commandService.DeleteCommandAsync(id);
            return NoContent();
        }
    }
}
