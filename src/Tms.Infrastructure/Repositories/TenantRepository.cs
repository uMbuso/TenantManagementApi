using Microsoft.EntityFrameworkCore;
using Tms.Application.Interfaces;
using Tms.Domain.Entities;
using Tms.Infrastructure.Persistence;

namespace Tms.Infrastructure.Repositories;

public class TenantRepository : ITenantRepository
{
    private readonly TmsDbContext _context;

    public TenantRepository(TmsDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Tenant>> GetAllAsync(int pageNumber, int pageSize)
    {
        return await _context.Tenants
            .OrderBy(t => t.LastName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<Tenant?> GetByIdAsync(Guid id)
    {
        return await _context.Tenants
            .Include(t => t.Leases)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<Tenant?> GetByEmailAsync(string email)
    {
        return await _context.Tenants
            .FirstOrDefaultAsync(t => t.Email == email);
    }

    public async Task AddAsync(Tenant tenant)
    {
        await _context.Tenants.AddAsync(tenant);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Tenant tenant)
    {
        _context.Tenants.Update(tenant);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var tenant = await _context.Tenants.FindAsync(id);
        if (tenant != null)
        {
            _context.Tenants.Remove(tenant);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<int> CountAsync()
    {
        return await _context.Tenants.CountAsync();
    }
}