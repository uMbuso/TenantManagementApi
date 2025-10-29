using Tms.Domain.Entities;

namespace Tms.Application.Interfaces;

public interface ITenantRepository
{
    Task<IEnumerable<Tenant>> GetAllAsync(int pageNumber, int pageSize);
    Task<Tenant?> GetByIdAsync(Guid id);
    Task<Tenant?> GetByEmailAsync(string email);
    Task AddAsync(Tenant tenant);
    Task UpdateAsync(Tenant tenant);
    Task DeleteAsync(Guid id);
    Task<int> CountAsync();
}