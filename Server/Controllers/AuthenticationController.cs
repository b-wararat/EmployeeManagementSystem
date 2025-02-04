using BaseLibrary.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Helpers;
using ServerLibrary.Repositoies.Contracts;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserAccount userAccount;
        public AuthenticationController(IUserAccount userAccount)
        {
            this.userAccount = userAccount;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateAsync(Register user) {
            if (user is null) return BadRequest("Model is empty");
            var result = await userAccount.CreateAsync(user);
            if (!result.Flag) {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> SignInAsync(Login user)
        {
            if (user is null) return BadRequest("Model is empty");
            var result = await userAccount.SignInAsync(user);
            if (!result.Flag)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshTokenAsync(RefreshToken token)
        {
            if (token is null) return BadRequest("Model is empty");
            var result = await userAccount.RefreshTokenAsync(token);
            if (!result.Flag)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("users")]
        [Authorize]
        public async Task<IActionResult> GetAllUsers() {
            var result = await userAccount.GetUsersAsync();
            if(result is null) return NotFound();
            return Ok(result);
        }

        [HttpGet("roles")]
        [Authorize]
        public async Task<IActionResult> GetAllRoles()
        {
            var result = await userAccount.GetRoles();
            if (result is null) return NotFound();
            return Ok(result);
        }

        [HttpPut("update-user")]
        [Authorize(Constants.Admin)]
        public async Task<IActionResult> UpdateUesr(ManageUser user)
        {
            if (user is null) return BadRequest("user is null");
            var result = await userAccount.UpdateUser(user);
            if (!result.Flag) return NotFound(result);
            return Ok(result);
        }

        [HttpDelete("delete-user/{id}")]
        [Authorize(Constants.Admin)]
        public async Task<IActionResult> UpdateUesr([FromRoute] int id)
        {
            if (id == 0) return BadRequest("user is null");
            var result = await userAccount.DeleteUser(id);
            if (!result.Flag) return NotFound(result);
            return Ok(result);
        }
    }
}
