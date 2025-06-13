using Exercici5API.DTOs;
using Exercici5API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
        [Authorize(Roles = "Sales,Admin")]
        [HttpPost("registerSells")]
        public async Task<IActionResult> RegisterSells([FromBody] WorkerRegisterDTO userDTO) // Register a new user
        {
            var user = new User { UserName = userDTO.Email, Email = userDTO.Email};
            var result = await _userManager.CreateAsync(user, userDTO.Password);
            var roleResult = new IdentityResult();

            if (result.Succeeded)
            {
                roleResult = await _userManager.AddToRoleAsync(user, "Sales");
            }
            if (result.Succeeded && roleResult.Succeeded)
            {
                return Ok("Client registered");
            }

            return BadRequest(result.Errors);
        }
        [Authorize(Roles = "Admin")]
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
        [HttpPost("login")] // Login as an user or admin
        public async Task<IActionResult> Login([FromBody] LoginDTO userDTO)
        {
            var user = await _userManager.FindByEmailAsync(userDTO.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, userDTO.Password))
            {
                return Unauthorized("Wrong mail or password");
            }

            var claims = new List<Claim>();

            if (!string.IsNullOrEmpty(user.UserName))
            {
                claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            }

            if (!string.IsNullOrEmpty(user.Id))
            {
                claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
            }

            var roles = await _userManager.GetRolesAsync(user);

            if (roles != null && roles.Count > 0)
            {
                foreach (var rol in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, rol));
                }
            }

            var token = CreateToken(claims.ToArray());

            return Ok(token);
        }
        [Authorize(Roles = "Admin, Sales")]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser([FromBody]ClientUpdateDTO client)
        {
            var user = await _userManager.FindByEmailAsync(client.Email);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            if (!string.IsNullOrEmpty(client.CompanyName))
            {
                user.CompanyName = client.CompanyName;
            }
            if (!string.IsNullOrEmpty(client.CEOName))
            {
                user.CEOName = client.CEOName;
            }
            if (client.NumberOfAttendees.HasValue)
            {
                user.NumberOfAttendees = client.NumberOfAttendees.Value;
            }
            if (client.IsVip.HasValue)
            {
                user.IsVip = client.IsVip.Value;
            }
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return Ok("User updated successfully.");
            }
            return BadRequest(result.Errors);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{email}")]
        public async Task<IActionResult> DeleteUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return Ok("User deleted successfully.");
            }

            return BadRequest(result.Errors);
        }

        private string CreateToken(Claim[] claims) // Creates the token for the current user
        {
            var jwtConfig = _configuration.GetSection("JwtSettings");
            var secretKey = jwtConfig["Key"];
            var issuer = jwtConfig["Issuer"];
            var audience = jwtConfig["Audience"];
            var expirationMinutes = int.Parse(jwtConfig["ExpirationMinutes"]);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(expirationMinutes),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
