using Microsoft.EntityFrameworkCore;
using Tms.Application.Interfaces;
using Tms.Domain.Entities;
using Tms.Infrastructure.Persistence;

namespace Tms.Infrastructure.Repositories;

public class LeaseRepository : ILeaseRepository
{
    private readonly TmsDbContext _context;

    public LeaseRepository(TmsDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Lease>> GetByTenantIdAsync(Guid tenantId)
    {
        return await _context.Leases
            .Where(l => l.TenantId == tenantId)
            .OrderBy(l => l.StartDate)
            .ToListAsync();
    }

    public async Task<Lease?> GetByIdAsync(Guid id)
    {
        return await _context.Leases
            .Include(l => l.Tenant)
            .FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task AddAsync(Lease lease)
    {
        await _context.Leases.AddAsync(lease);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Lease lease)
    {
        _context.Leases.Update(lease);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var lease = await _context.Leases.FindAsync(id);
        if (lease != null)
        {
            _context.Leases.Remove(lease);
            await _context.SaveChangesAsync();
        }
    }
}