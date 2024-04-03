using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MintCartWebApi.Service.Auth;

namespace MintCartWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost, Route("login")]
        public async Task<IActionResult> login(string userEmail, string password)
        {
            if (!(string.IsNullOrWhiteSpace(userEmail) && string.IsNullOrWhiteSpace(password)))
            {

                var data = await _authService.Authenticate(userEmail, password);
                if (data == null)
                {
                    return Ok(new { success = false, statusCode = 400, error = "Invalid credentials" });
                }
                return Ok(new { success = true, statusCode = 200, data = new { Token = data.ToString() } });
            }
            return BadRequest(new { success = false, statusCode = 400, errors = "please provide valid email/password" });
        }
    }
}
