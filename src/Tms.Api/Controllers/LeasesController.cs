using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tms.Application.DTOs;
using Tms.Application.Interfaces;
using Tms.Domain.Entities;

namespace Tms.Api.Controllers;

[ApiController]
[Route("api")]
[Authorize]
public class LeasesController : ControllerBase
{
    private readonly ILeaseRepository _leaseRepository;
    private readonly ITenantRepository _tenantRepository;
    private readonly IMapper _mapper;

    public LeasesController(
        ILeaseRepository leaseRepository,
        ITenantRepository tenantRepository,
        IMapper mapper)
    {
        _leaseRepository = leaseRepository;
        _tenantRepository = tenantRepository;
        _mapper = mapper;
    }

    [HttpGet("tenants/{tenantId}/leases")]
    public async Task<ActionResult<IEnumerable<LeaseDto>>> GetLeasesByTenant(Guid tenantId)
    {
        var tenant = await _tenantRepository.GetByIdAsync(tenantId);
        if (tenant == null)
        {
            return NotFound(new { message = "Tenant not found" });
        }

        var leases = await _leaseRepository.GetByTenantIdAsync(tenantId);
        var leaseDtos = _mapper.Map<IEnumerable<LeaseDto>>(leases);

        return Ok(leaseDtos);
    }

    [HttpGet("leases/{id}")]
    public async Task<ActionResult<LeaseDto>> GetLease(Guid id)
    {
        var lease = await _leaseRepository.GetByIdAsync(id);

        if (lease == null)
        {
            return NotFound(new { message = "Lease not found" });
        }

        return Ok(_mapper.Map<LeaseDto>(lease));
    }

    [HttpPost("tenants/{tenantId}/leases")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<LeaseDto>> CreateLease(Guid tenantId, [FromBody] CreateLeaseDto createLeaseDto)
    {
        var tenant = await _tenantRepository.GetByIdAsync(tenantId);
        if (tenant == null)
        {
            return NotFound(new { message = "Tenant not found" });
        }

        var lease = _mapper.Map<Lease>(createLeaseDto);
        lease.Id = Guid.NewGuid();
        lease.TenantId = tenantId;

        await _leaseRepository.AddAsync(lease);

        var leaseDto = _mapper.Map<LeaseDto>(lease);

        return CreatedAtAction(nameof(GetLease), new { id = lease.Id }, leaseDto);
    }

    [HttpPut("leases/{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<LeaseDto>> UpdateLease(Guid id, [FromBody] UpdateLeaseDto updateLeaseDto)
    {
        var lease = await _leaseRepository.GetByIdAsync(id);

        if (lease == null)
        {
            return NotFound(new { message = "Lease not found" });
        }

        _mapper.Map(updateLeaseDto, lease);

        await _leaseRepository.UpdateAsync(lease);

        return Ok(_mapper.Map<LeaseDto>(lease));
    }

    [HttpDelete("leases/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteLease(Guid id)
    {
        var lease = await _leaseRepository.GetByIdAsync(id);

        if (lease == null)
        {
            return NotFound(new { message = "Lease not found" });
        }

        await _leaseRepository.DeleteAsync(id);

        return NoContent();
    }
}