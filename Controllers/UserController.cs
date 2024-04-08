using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MintCartWebApi.ModelDto;
using MintCartWebApi.Service;

namespace MintCartWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService userService) : ControllerBase
    {
        [HttpPost, Route("RegisterUser")]
        public async Task<IActionResult> RegisterUser([FromForm] RegisterUserDto registerUserDto)
        {
            if (ModelState.IsValid)
            {
                var data = await userService.createUserAsync(registerUserDto);
                if (data != null)
                {
                    return Ok(new { success = true, statusCode = 200, data = data });
                }
                else
                {
                    return Ok(new { success = false, statusCode = 400, error = 4  });
                }
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors)
                                     .Select(e => e.ErrorMessage)
                                     .ToList();

            return BadRequest(new { success = false, statusCode = 400, errors = errors });
        }
        [HttpPost, Route("GetUserById")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetUser(int id)
        {
            if (id > 0)
            {
                var data = await userService.getUserAsync(id);
                if (data != null)
                {
                    return Ok(new { success = true, statusCode = 200, data = data });
                }
                else
                {
                    return Ok(new { success = false, statusCode = 400, error = "Failed to retrive user" });
                }
            }

            return BadRequest(new { success = false, statusCode = 400, errors = $"Invalid userId {id}" });
        }

    }
}
