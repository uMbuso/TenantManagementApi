using Microsoft.AspNetCore.Mvc;
using Tms.Application.DTOs;
using Tms.Application.Interfaces;
using Tms.Domain.Entities;
using Tms.Infrastructure.Auth;

namespace Tms.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _tokenGenerator;

    public AuthController(IUserRepository userRepository, IJwtTokenGenerator tokenGenerator)
    {
        _userRepository = userRepository;
        _tokenGenerator = tokenGenerator;
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto loginDto)
    {
        var user = await _userRepository.GetByUsernameAsync(loginDto.Username);

        if (user == null || !PasswordHasher.VerifyPassword(loginDto.Password, user.PasswordHash))
        {
            return Unauthorized(new { message = "Invalid username or password" });
        }

        var token = _tokenGenerator.GenerateToken(user.Id.ToString(), user.Username, user.Role);

        return Ok(new AuthResponseDto(token, user.Username, user.Role));
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto registerDto)
    {
        var existingUser = await _userRepository.GetByUsernameAsync(registerDto.Username);
        if (existingUser != null)
        {
            return BadRequest(new { message = "Username already exists" });
        }

        var existingEmail = await _userRepository.GetByEmailAsync(registerDto.Email);
        if (existingEmail != null)
        {
            return BadRequest(new { message = "Email already exists" });
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = registerDto.Username,
            Email = registerDto.Email,
            PasswordHash = PasswordHasher.HashPassword(registerDto.Password),
            Role = registerDto.Role ?? "Viewer",
            CreatedAt = DateTime.UtcNow
        };

        await _userRepository.AddAsync(user);

        var token = _tokenGenerator.GenerateToken(user.Id.ToString(), user.Username, user.Role);

        return Ok(new AuthResponseDto(token, user.Username, user.Role));
    }
}