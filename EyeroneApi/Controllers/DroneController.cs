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
    public class DroneController : ControllerBase
    {
        private readonly IDroneService _droneService;
        public DroneController(IDroneService droneService)
        {
            _droneService = droneService;
        }

        [HttpGet("/getAll")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<DroneDto>>> GetDrones()
        {
            var drones = await _droneService.GetAllDronesAsync();
            return Ok(drones);
        }

        [HttpPost("add")]
        [Authorize(Roles = "operator")]
        public async Task<ActionResult<DroneDto>> AddDrone(AddDroneDTO drone)
        {
            try
            {
                var created = await _droneService.CreateDroneAsync(drone);
                return CreatedAtAction(nameof(GetDroneById), new { id = created.DroneId }, created);
            }
            catch (UserServiceException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return Conflict(new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<DroneDto>> GetDroneById(int id)
        {
            try
            {
                var drone = await _droneService.GetDroneByIdAsync(id);
                return Ok(drone);
            }
            catch (DroneNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
        [HttpGet("user/{userId}")]
        [Authorize(Roles = "operator")]
        public async Task<ActionResult<IEnumerable<DroneDto>>> GetUserDrones(int userId)
        {
            try
            {
                var drones = await _droneService.GetUserDrones(userId);
                return Ok(drones);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("authenticate")]
        public async Task<ActionResult> AutheticateDrone(string serialNumber)
        {
            try
            {
                var token = await _droneService.AuthenticateDroneAsync(serialNumber);
                return Ok(new { token });
            }
            catch (DroneNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpGet("availableSerials")]
        public ActionResult<IEnumerable<string>> GetAvailableSerials()
        {
            var serials = _droneService.GetAvailableSerials();
            return Ok(serials);
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "admin,operator")]
        public async Task<IActionResult> DeleteDrone(int id)
        {
            try
            {
                await _droneService.DeleteDroneAsync(id);
                return NoContent();
            }
            catch (DroneNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
    }
}