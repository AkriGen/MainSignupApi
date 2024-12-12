using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using Microsoft.Extensions.Configuration;
using System.Linq;
using WebAPINatureHub3.Models;
using Microsoft.CodeAnalysis.Scripting;
using System;

namespace WebAPINatureHub3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly NatureHub3Context _context;
        private readonly IConfiguration _configuration;

        public AuthController(NatureHub3Context context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        [HttpPost("adminlogin")]
        public IActionResult Login([FromBody] AdminLoginDto loginDto)
        {
            var admin = _context.Admins.SingleOrDefault(u => u.Username == loginDto.Username && u.Email == loginDto.Email && u.Password == loginDto.Password);
            if (admin == null) { return Unauthorized("Admin not found."); }
            var token = GenerateJwtToken(admin.Username, "Admin");
            return Ok(new { token });

        }
        
        [HttpPost("userlogin")]
        public IActionResult Login([FromBody] UserLoginDto loginDto)
        {
            var user = _context.Users.SingleOrDefault(u => u.UserName == loginDto.UserName && u.Email == loginDto.Email && u.Password == loginDto.Password );
            if (user == null) { return Unauthorized("User not found."); }
            var token = GenerateJwtToken(user.UserName,"User");
            return Ok(new { token });

        }
        private string GenerateJwtToken(string username, string role)
        {
            var claims = new[]
            {
               new Claim(ClaimTypes.Name,username),
               new Claim(ClaimTypes.Role,role),
           };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(50),
            signingCredentials: credential
                );
            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    
    }
    // DTO for User Login
    public class AdminLoginDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
    // DTO for User Login
    public class UserLoginDto
    {

        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
