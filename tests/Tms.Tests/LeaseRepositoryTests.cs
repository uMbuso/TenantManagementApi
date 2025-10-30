using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tms.Domain.Entities;
using Tms.Infrastructure.Persistence;
using Tms.Infrastructure.Repositories;
using Xunit;

namespace Tms.Tests;

public class LeaseRepositoryTests : IDisposable
{
    private readonly TmsDbContext _context;
    private readonly LeaseRepository _leaseRepository;
    private readonly TenantRepository _tenantRepository;

    public LeaseRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<TmsDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new TmsDbContext(options);
        _leaseRepository = new LeaseRepository(_context);
        _tenantRepository = new TenantRepository(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddLease()
    {
        var tenant = await CreateTestTenant();
        var lease = new Lease
        {
            Id = Guid.NewGuid(),
            TenantId = tenant.Id,
            PropertyAddress = "123 Test Street",
            StartDate = DateTime.UtcNow,
            MonthlyRent = 15000m
        };

        await _leaseRepository.AddAsync(lease);
        var result = await _leaseRepository.GetByIdAsync(lease.Id);

        result.Should().NotBeNull();
        result!.PropertyAddress.Should().Be("123 Test Street");
    }

    private async Task<Tenant> CreateTestTenant()
    {
        var tenant = new Tenant
        {
            Id = Guid.NewGuid(),
            FirstName = "Test",
            LastName = "Tenant",
            Email = $"test{Guid.NewGuid()}@test.com",
            Phone = "0821234567",
            CreatedAt = DateTime.UtcNow
        };
        await _tenantRepository.AddAsync(tenant);
        return tenant;
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}