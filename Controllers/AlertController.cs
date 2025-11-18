using EyeroneApi.Data;
using EyeroneApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EyeroneApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlertController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public AlertController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<Alert>>> GetAlerts()
        {
            return await _context.Alerts
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Alert>> GetAlertById(int id)
        {
            var alert = await _context.Alerts.FindAsync(id);

            if (alert == null)
            {
                return NotFound();
            }

            return Ok(alert);
        }

        [HttpGet("/getAll/user/{userId}")]
        public async Task<ActionResult<Alert>> GetAlertByUserId(int userId)
        {
            var alerts = await _context.Alerts
             .Where(a => a.UserId == userId)
             .OrderByDescending(a => a.CreatedAt) 
             .ToListAsync();

            if (alerts == null || !alerts.Any())
            {
                return NotFound("No alerts found for this user.");
            }

            return Ok(alerts);
        }

        [HttpPost("add")]
        public async Task<ActionResult<Alert>> AddAlert(Alert alert)
        {
            
            _context.Alerts.Add(alert);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAlertById), new { id = alert.AlertId }, alert);
        }

        [HttpPost("/isRead{id}")]
        public async Task<ActionResult<Alert>> ReadAlert(int id)
        {

            var alert = await _context.Alerts.FindAsync(id);

            alert.IsRead = true;
            await _context.SaveChangesAsync();

            return Ok(alert);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteAlert(int id)
        {
            var alert = await _context.Alerts.FindAsync(id);
            if (alert == null)
            {
                return NotFound();
            }

            _context.Alerts.Remove(alert);
            await _context.SaveChangesAsync();

            return NoContent(); 
        }
    }
}
