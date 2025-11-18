using EyeroneApi.Data;
using EyeroneApi.DTOs;
using EyeroneApi.Models; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EyeroneApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DroneController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DroneController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("/getAll")]
        public async Task<ActionResult<IEnumerable<Drone>>> GetDrones()
        {

            var drones = await _context.Drones.ToListAsync();

            return Ok(drones);

        }
        [HttpPost("add")]
        public async Task<ActionResult<IEnumerable<Drone>>> AddDrone(Drone drone)
        {

            _context.Drones.Add(drone);

            await _context.SaveChangesAsync();

            return Created("api/addDrone/" + drone.DroneId, drone);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Drone>> GetDroneById(int id)
        {
            var drone = await _context.Drones
                .Include(d => d.Owner)
                .Where(d => d.DroneId == id) 

                .Select(d => new DroneDto
                {
                    DroneId = d.DroneId,
                    Name = d.DroneName,
                    Model = d.Model,
                    SerialNumber = d.SerialNumber,
                    Owner = d.Owner == null ? null : new UserDto
                    {
                        UserId = d.Owner.UserId,
                        Username = d.Owner.Username,
                        Email = d.Owner.Email
                    }
                })
                .FirstOrDefaultAsync(); 

            if (drone == null)
            {
                return NotFound();
            }

            return Ok(drone);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteDrone(int id)
        {
            var drone = await _context.Drones.FindAsync(id);
            if (drone == null)
            {
                return NotFound();
            }

            _context.Drones.Remove(drone);
            await _context.SaveChangesAsync();

            return NoContent(); 
        }
    }
}