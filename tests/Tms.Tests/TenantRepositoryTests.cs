using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tms.Domain.Entities;
using Tms.Infrastructure.Persistence;
using Tms.Infrastructure.Repositories;
using Xunit;

namespace Tms.Tests;

public class TenantRepositoryTests : IDisposable
{
    private readonly TmsDbContext _context;
    private readonly TenantRepository _repository;

    public TenantRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<TmsDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new TmsDbContext(options);
        _repository = new TenantRepository(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddTenant()
    {
        var tenant = new Tenant
        {
            Id = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@test.com",
            Phone = "0821234567",
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(tenant);
        var result = await _repository.GetByIdAsync(tenant.Id);

        result.Should().NotBeNull();
        result!.Email.Should().Be("john.doe@test.com");
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnTenant()
    {
        var tenant = new Tenant
        {
            Id = Guid.NewGuid(),
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane@test.com",
            Phone = "0827654321",
            CreatedAt = DateTime.UtcNow
        };
        await _repository.AddAsync(tenant);

        var result = await _repository.GetByEmailAsync("jane@test.com");

        result.Should().NotBeNull();
        result!.FirstName.Should().Be("Jane");
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}