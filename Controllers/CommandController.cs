using EyeroneApi.Data;
using EyeroneApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EyeroneApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommandController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CommandController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<Command>>> GetCommands()
        {
            return await _context.Commands.ToListAsync();
        }

        [HttpGet("getAll/drone/{droneId}")]
        public async Task<ActionResult<Command>> GetCommandByUserId(int droneId)
        {
            var commands = await _context.Commands
                .Where(C => C.DroneId == droneId)
                .OrderByDescending(C => C.CreatedAt)
                .ToListAsync();

            if (commands == null || !commands.Any())
            {
                return NotFound("No commands found for this drone.");
            }

            return Ok(commands);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Command>> GetCommandById(int id)
        {
            var command = await _context.Commands.FindAsync(id);

            if (command == null)
            {
                return NotFound();
            }

            return Ok(command);
        }

        [HttpPost("add")]
        public async Task<ActionResult<Command>> AddCommand(Command command)
        {
            _context.Commands.Add(command);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCommandById), new { id = command.CommandId }, command);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCommand(int id)
        {
            var command = await _context.Commands.FindAsync(id);
            if (command == null)
            {
                return NotFound();
            }

            _context.Commands.Remove(command);
            await _context.SaveChangesAsync();

            return NoContent(); 
        }
    }
}
