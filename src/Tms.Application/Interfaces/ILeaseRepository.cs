using Tms.Domain.Entities;

namespace Tms.Application.Interfaces;

public interface ILeaseRepository
{
    Task<IEnumerable<Lease>> GetByTenantIdAsync(Guid tenantId);
    Task<Lease?> GetByIdAsync(Guid id);
    Task AddAsync(Lease lease);
    Task UpdateAsync(Lease lease);
    Task DeleteAsync(Guid id);
}