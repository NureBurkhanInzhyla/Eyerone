using EyeroneApi.Data;
using EyeroneApi.DTOs;
using EyeroneApi.Models; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EyeroneApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightSessionController: ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public FlightSessionController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<FlightSession>>> GetFlightSessions()
        {
            return await _context.FlightSessions.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FlightSession>> GetFlightSessionById(int id)
        {
            var flightSession = await _context.FlightSessions.FindAsync(id);

            if (flightSession == null)
            {
                return NotFound(); 
            }

            return Ok(flightSession); 
        }

        [HttpPost("add")]
        public async Task<ActionResult<FlightSession>> AddFlightSession(FlightSession session)
        {
            _context.FlightSessions.Add(session);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFlightSessionById), new { id = session.SessionId }, session);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteFlightSession(int id)
        {
            var session = await _context.FlightSessions.FindAsync(id);
            if (session == null)
            {
                return NotFound();
            }

            _context.FlightSessions.Remove(session);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("{id}/end")]
        public async Task<ActionResult<FlightSession>> EndFlightSession(int id)
        {
            var session = await _context.FlightSessions.FindAsync(id);
            if (session == null)
            {
                return NotFound();
            }
            if (session.EndedAt != null)
            {
                return BadRequest("This flight session has already been ended.");
            }

            session.EndedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(session);
        }
    }
}
