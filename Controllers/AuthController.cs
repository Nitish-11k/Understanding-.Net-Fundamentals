using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Text;
using TodoApi.Core.Models;
using TodoApi.DTOs;
using BCrypt.Net;

namespace TodoApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    private readonly AppDBContext _context;
    private readonly IConfiguration _configuration;

    public AuthController(AppDBContext context, IConfiguration configuration)
    {
      _context = context;
      _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
      if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
      {
        return BadRequest("Email already in use.");
      }

      var user = new UserModel
      {
        Name = registerDto.Name,
        Email = registerDto.Email,
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
        Role = "User"
      };

      _context.Users.Add(user);
      await _context.SaveChangesAsync();

      return Ok("User registered successfully.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
      var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
      if(user == null) return BadRequest("Invalid email or password.");
      if(!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
      {
        return BadRequest("Invalid email or password.");
      }
      string token = CreateToken(user);
      return Ok(new { token = token });
    }
    
    [HttpGet("admin-only")]
    [Authorize(Roles = "Admin")]
    public IActionResult AdminDashboard()
    {
      return Ok("Welcome, Admin. You have full access.");
    }

    private string CreateToken(UserModel user)
    {
      var claims = new List<Claim>
      {
        // new Claim(ClaimTypes.Name, user.Username),
        // new Claim(ClaimTypes.Role, "Admin"),
           
      
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Role, user.Role ?? "User")
      };

      var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
        _configuration.GetSection("AppSettings:Token").Value!));

      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(claims),
        Expires = DateTime.Now.AddDays(1),
        SigningCredentials = creds
      };

      var tokenHandler = new JwtSecurityTokenHandler();
      var token = tokenHandler.CreateToken(tokenDescriptor);

      return tokenHandler.WriteToken(token);
    }
  }
}