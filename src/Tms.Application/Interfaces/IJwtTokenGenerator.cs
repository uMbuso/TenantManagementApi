namespace Tms.Application.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(string userId, string username, string role);
}