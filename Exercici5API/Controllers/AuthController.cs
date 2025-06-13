using Exercici5API.DTOs;
using Exercici5API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace Exercici5API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        public AuthController(UserManager<User> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] ClientRegisterDTO userDTO) // Register a new user
        {
            var user = new User { UserName = userDTO.Email, Email = userDTO.Email, CEOName = userDTO.CEOName, CompanyName = userDTO.CompanyName, IsVip  = userDTO.IsVip, NumberOfAttendees = userDTO.NumberOfAttendees};
            var result = await _userManager.CreateAsync(user, userDTO.Password);
            var roleResult = new IdentityResult();

            if (result.Succeeded)
            {
                roleResult = await _userManager.AddToRoleAsync(user, "Client");
            }
            if (result.Succeeded && roleResult.Succeeded)
            {
                return Ok("Client registered");
            }

            return BadRequest(result.Errors);
        }
        [HttpPost("registerSells")]
        public async Task<IActionResult> RegisterSells([FromBody] WorkerRegisterDTO userDTO) // Register a new user
        {
            var user = new User { UserName = userDTO.Email, Email = userDTO.Email};
            var result = await _userManager.CreateAsync(user, userDTO.Password);
            var roleResult = new IdentityResult();

            if (result.Succeeded)
            {
                roleResult = await _userManager.AddToRoleAsync(user, "Sells");
            }
            if (result.Succeeded && roleResult.Succeeded)
            {
                return Ok("Client registered");
            }

            return BadRequest(result.Errors);
        }
        [HttpPost("registerAdmin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] WorkerRegisterDTO userDTO) // Register a new user
        {
            var user = new User { UserName = userDTO.Email, Email = userDTO.Email};
            var result = await _userManager.CreateAsync(user, userDTO.Password);
            var roleResult = new IdentityResult();

            if (result.Succeeded)
            {
                roleResult = await _userManager.AddToRoleAsync(user, "Admin");
            }
            if (result.Succeeded && roleResult.Succeeded)
            {
                return Ok("Client registered");
            }

            return BadRequest(result.Errors);
        }
    }
}
