using Eyerone.Application.DTOs;
using Eyerone.Application.ServicesImplementation;
using Eyerone.Application.ServicesInterfaces;
using Eyerone.Domain.Models;
using EyeroneApi.FCM;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EyeroneApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlertController : ControllerBase
    {
        private readonly IAlertService _alertService;
        private readonly IDroneService _droneService;

        public AlertController(IAlertService alertService, IDroneService droneService)
        {
            _alertService = alertService;
            _droneService = droneService;
        }

        [HttpGet("getAll")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<Alert>>> GetAlerts() =>
        Ok(await _alertService.GetAllAlertsAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<Alert>> GetAlertById(int id)
        {
            var alert = await _alertService.GetAlertByIdAsync(id);
            if (alert == null) return NotFound();
            return Ok(alert);
        }

        [HttpPost("add")]
        [Authorize(Roles = "Drone")]
        public async Task<ActionResult<Alert>> AddAlert([FromBody] Alert alert)
        {
            var droneId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);

            var drone = await _droneService.GetDroneByIdAsync(droneId);
            var ownerId = drone.OwnerId;

            alert.DroneId = droneId;
            alert.UserId = ownerId;
            var newAlert = await _alertService.CreateAlertAsync(alert);

            if (TokenStorage.UserTokens.TryGetValue(alert.UserId, out var token))
            {
                var message = new Message()
                {
                    Notification = new Notification { Title = alert.Type, Body = alert.Text },
                    Token = token,
                    Android = new AndroidConfig
                    {
                        Priority = Priority.High,
                        Notification = new AndroidNotification
                        {
                            ChannelId = "DRONE_ALERTS_CHANNEL",
                            Priority = NotificationPriority.HIGH,
                            DefaultSound = true,
                            DefaultVibrateTimings = true
                        }
                    }
                };
                await FirebaseMessaging.DefaultInstance.SendAsync(message);
            }
           

            return Ok(newAlert);
        }

        [HttpGet("user/{userId}")]
        [Authorize(Roles = "operator")]
        public async Task<ActionResult<IEnumerable<AlertDTO>>> GetAlertsByUserId(int userId)
        {
            var alerts = await _alertService.GetAlertsByUserIdAsync(userId);
            return Ok(alerts);
        }

        [HttpPut("MarkAsRead/{id}")]
        [Authorize(Roles = "operator")]

        public async Task<ActionResult<Alert>> ReadAlert(int id) =>
        Ok(await _alertService.ReadAlertAsync(id));

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteAlert(int id)
        {
            await _alertService.DeleteAlertAsync(id);
            return NoContent();
        }
    }
}
