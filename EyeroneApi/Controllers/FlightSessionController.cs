using Eyerone.Application.DTOs;
using Eyerone.Application.ServicesImplementation;
using Eyerone.Application.ServicesInterfaces;
using Eyerone.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;

namespace EyeroneApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightSessionController : ControllerBase
    {
        private readonly IFlightSessionService _service;

        public FlightSessionController(IFlightSessionService service)
        {
            _service = service;
        }

        [HttpGet("getAll")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<FlightSession>>> GetFlightSessions()
        {
            var sessions = await _service.GetAllSessionsAsync();
            return Ok(sessions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FlightSession>> GetFlightSessionById(int id)
        {
            try
            {
                var session = await _service.GetSessionByIdAsync(id);
                return Ok(session);
            }
            catch (FlightSessioNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<FlightSessionDto>>> GetUserFlightHistory(int userId)
        {
            var history = await _service.GetSessionsByUserIdAsync(userId);
            return Ok(history);
        }

        [HttpPost("add/{droneId}")]
        [Authorize(Roles = "Drone")]
        public async Task<ActionResult<FlightSession>> AddFlightSession()
        {
            int droneId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            var created = await _service.AddSessionAsync(droneId);
            return CreatedAtAction(nameof(GetFlightSessionById), new { id = created.SessionId }, created);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteFlightSession(int id)
        {
            await _service.DeleteSessionAsync(id);
            return NoContent();
        }

        [HttpPut("{id}/end")]
        public async Task<ActionResult<FlightSessionDto>> EndFlightSession(int id)
        {
            try
            {
                var result = await _service.EndSessionAsync(id);
                return Ok(result);
            }
            catch (FlightSessioNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Unexpected error: {ex.Message}");
            }
        }

    }
}
