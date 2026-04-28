using Eyerone.Application.DTOs;
using Eyerone.Application.ServicesImplementation;
using Eyerone.Domain.Models;
using Eyerone.Domain.ServicesInterfaces;
using EyeroneApi.FCM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EyeroneApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    public class UsersController : ControllerBase 
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(UserRegisterDTO registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userDto = await _userService.RegisterAsync(registerDto);

                return CreatedAtAction(nameof(GetUserById), new { id = userDto.UserId }, userDto);
            }
            catch (UserServiceException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] UserLoginDTO loginDto)
        {
            try
            {
                var loginDtoResponse = await _userService.LoginAsync(loginDto);

                if (!string.IsNullOrEmpty(loginDto.FcmDeviceToken))
                {
                    TokenStorage.UserTokens[loginDtoResponse.User.UserId] = loginDto.FcmDeviceToken;
                }

                return Ok(loginDtoResponse);
            }
            catch (UserServiceException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("getUsers")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound();
            return Ok(user);

        }

        [HttpGet("{id}/alerts")]
        public async Task<IActionResult> GetUsersAlerts(int id)
        {
            try
            {
                var alerts = await _userService.GetUserAlertsAsync(id);
                return Ok(alerts);
            }
            catch (UserServiceException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpPut("{id}/role")]
        [Authorize(Roles = "admin")]

        public async Task<IActionResult> ChangeRole(int id, [FromBody] string role)
        {
            await _userService.ChangeRoleAsync(id, role);
            return NoContent();
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await _userService.DeleteUserAsync(id);
                return NoContent();
            }
            catch (UserServiceException)
            {
                return NotFound();
            }
        }
    }
}