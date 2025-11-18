using EyeroneApi.Data;
using EyeroneApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EyeroneApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class TelemetryController: ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TelemetryController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<Telemetry>>> GetTelemetryData()
        {
          
            return await _context.TelemetryData
                .OrderByDescending(t => t.Timestamp)
                .Take(100)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Telemetry>> GetTelemetryById(int id)
        {
            var telemetry = await _context.TelemetryData.FindAsync(id);

            if (telemetry == null)
            {
                return NotFound();
            }

            return Ok(telemetry);
        }
        [HttpGet("flight/{flightId}")]
        public async Task<ActionResult<IEnumerable<Telemetry>>> GetTelemetryByFlight(int flightId)
        {
            var telemetryList = await _context.TelemetryData
             .Where(t => t.FlightId == flightId)
             .OrderBy(t => t.Timestamp) 
             .ToListAsync();         

            if (telemetryList == null || !telemetryList.Any())
            {
                return NotFound("No telemetry found for this flight.");
            }

            return Ok(telemetryList);
        }

        [HttpPost("add")]
        public async Task<ActionResult<Telemetry>> AddTelemetry(Telemetry telemetry)
        {
       
            _context.TelemetryData.Add(telemetry);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTelemetryById), new { id = telemetry.TelemetryId }, telemetry);
        } 
    }
}
