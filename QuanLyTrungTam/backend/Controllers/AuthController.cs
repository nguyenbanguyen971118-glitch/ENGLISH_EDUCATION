using Microsoft.AspNetCore.Mvc;
using backend.Services.Interfaces;
using backend.DTOs;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginData)
        {
            var result = await _userService.AuthenticateAsync(loginData);
            
            // Check if authentication was successful
            var resultDict = result as dynamic;
            if (resultDict?.success == false)
            {
                return Unauthorized(result);
            }

            return Ok(result);
        }
    }

    public class LoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}