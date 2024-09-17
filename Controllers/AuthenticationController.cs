using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using Tech_Test_Backend.Models.Dtos;
using Tech_Test_Backend.Services;

namespace Tech_Test_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IAuthService _authService;

        public AuthenticationController(IAuthService authService, ILogger<AuthenticationController> logger)
        {
            _logger = logger;
            _authService = authService;
        }

        [HttpPost(Name = "Login")]
        public IActionResult Login(string returnUrl)
        {
            return Ok();
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(registerDto);

            if (result.Succeeded)
                return Ok("User registered succesfully");
            else
                return BadRequest(result.Errors);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var token = await _authService.AuthenticateAsync(loginDto);

            if (token != "Invalid login attempt")
                return Ok(new { Token = token });
            else
                return Unauthorized(token);
        }

        [Authorize]  // Require authentication
        [HttpGet("WhoAmI")]
        public async Task<IActionResult> WhoAmI()
        {
            var userIdentity = await _authService.WhoAmIAsync();
            if (userIdentity == null)
            {
                return NotFound("User not found");
            }

            return Ok(userIdentity);
        }

    }
}
