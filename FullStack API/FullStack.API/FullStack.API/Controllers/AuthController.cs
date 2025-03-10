using FullStack.API.Models;
using FullStack.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FullStack.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;

        public AuthController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] Login login)
        {
            if (login.Username == "admin" && login.Password == "admin")
            {
                var employee = new Employee
                {
                    Name = "Test User",
                    Email = "testuser@example.com"
                };

                var token = _tokenService.GenerateToken(employee);
                return Ok(new { Token = token });
            }
            return Unauthorized();
        }
    }
}
