using Microsoft.AspNetCore.Mvc;
using ResumeScreener.Data;
using ResumeScreener.DTOs.Auth;
using ResumeScreener.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ResumeScreener.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthController(ApplicationDbContext context,IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        // Check if email already exists
        var existingUser = _context.Users
            .FirstOrDefault(x => x.Email == dto.Email);

        if (existingUser != null)
        {
            return BadRequest("Email already exists");
        }

        // Hash password
        string hashedPassword =
            BCrypt.Net.BCrypt.HashPassword(dto.Password);

        // Create user
        var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email,
            PasswordHash = hashedPassword,
            Role = dto.Role
        };

        _context.Users.Add(user);

        await _context.SaveChangesAsync();

        return Ok("User registered successfully");
    }

    [HttpPost("login")]
    public IActionResult Login(LoginDto dto)
    {
        var user = _context.Users
            .FirstOrDefault(x => x.Email == dto.Email);

        if (user == null)
        {
            return Unauthorized("Invalid email or password");
        }

        bool isPasswordValid =
            BCrypt.Net.BCrypt.Verify(
                dto.Password,
                user.PasswordHash
            );

        if (!isPasswordValid)
        {
            return Unauthorized("Invalid email or password");
        }

        // Claims
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        // Secret key
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                _configuration["Jwt:Key"]!
            )
        );

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256
        );

        // Token
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(
                Convert.ToDouble(
                    _configuration["Jwt:ExpireHours"]
                )
            ),
            signingCredentials: credentials
        );

        var jwtToken =
            new JwtSecurityTokenHandler()
                .WriteToken(token);

        return Ok(new
        {
            token = jwtToken,
            role = user.Role,
            fullName = user.FullName
        });
    }
}