
using Eyerone.Application.ServicesImplementation;
using Eyerone.Application.ServicesInterfaces;
using Eyerone.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace EyeroneApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class TelemetryController: ControllerBase
    {
        private readonly ITelemetryService _service;

        public TelemetryController(ITelemetryService service)
        {
            _service = service;
        }

        [HttpGet("getAll")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<Telemetry>>> GetTelemetryDatax(int limit = 100)
        {
            var telemetry = await _service.GetAllTelemetryAsync(limit);
            return Ok(telemetry);
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<Telemetry>> GetTelemetryById(int id)
        {
            try
            {
                var telemetry = await _service.GetTelemetryByIdAsync(id);
                return Ok(telemetry);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpGet("drone/{droneId}/latest")]
        [Authorize(Roles = "operator")]
        public async Task<ActionResult<Telemetry>> GetLatestByDrone(int droneId)
        {
            var telemetry = await _service.GetLatestTelemetryByDroneAsync(droneId);

            if (telemetry == null)
                return NotFound("No telemetry found");

            return Ok(telemetry);
        }

        [HttpGet("flight/{flightId}")]
        public async Task<ActionResult<IEnumerable<Telemetry>>> GetTelemetryByFlight(int flightId)
        {
            var telemetry = await _service.GetTelemetryByFlightAsync(flightId);
            if (!telemetry.Any())
                return NotFound("No telemetry found for this flight.");
            return Ok(telemetry);
        }

        [HttpPost("add")]
        [Authorize(Roles = "Drone")]
        public async Task<ActionResult<Telemetry>> AddTelemetry(Telemetry telemetry)
        {
            var created = await _service.AddTelemetryAsync(telemetry);
            return CreatedAtAction(nameof(GetTelemetryById), new { id = created.TelemetryId }, created);
        } 
    }
}
