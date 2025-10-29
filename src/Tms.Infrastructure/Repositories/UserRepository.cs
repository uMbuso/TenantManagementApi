using Microsoft.EntityFrameworkCore;
using Tms.Application.Interfaces;
using Tms.Domain.Entities;
using Tms.Infrastructure.Persistence;

namespace Tms.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly TmsDbContext _context;

    public UserRepository(TmsDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }
}