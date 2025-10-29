using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tms.Application.DTOs;
using Tms.Application.Interfaces;
using Tms.Domain.Entities;

namespace Tms.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TenantsController : ControllerBase
{
    private readonly ITenantRepository _tenantRepository;
    private readonly IMapper _mapper;

    public TenantsController(ITenantRepository tenantRepository, IMapper mapper)
    {
        _tenantRepository = tenantRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TenantDto>>> GetTenants(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
        {
            return BadRequest(new { message = "Invalid pagination parameters" });
        }

        var tenants = await _tenantRepository.GetAllAsync(pageNumber, pageSize);
        var totalCount = await _tenantRepository.CountAsync();

        var tenantDtos = _mapper.Map<IEnumerable<TenantDto>>(tenants);

        Response.Headers.Add("X-Total-Count", totalCount.ToString());
        Response.Headers.Add("X-Page-Number", pageNumber.ToString());
        Response.Headers.Add("X-Page-Size", pageSize.ToString());

        return Ok(tenantDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TenantDto>> GetTenant(Guid id)
    {
        var tenant = await _tenantRepository.GetByIdAsync(id);

        if (tenant == null)
        {
            return NotFound(new { message = "Tenant not found" });
        }

        return Ok(_mapper.Map<TenantDto>(tenant));
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<TenantDto>> CreateTenant([FromBody] CreateTenantDto createTenantDto)
    {
        var existingTenant = await _tenantRepository.GetByEmailAsync(createTenantDto.Email);
        if (existingTenant != null)
        {
            return BadRequest(new { message = "Email already exists" });
        }

        var tenant = _mapper.Map<Tenant>(createTenantDto);
        tenant.Id = Guid.NewGuid();
        tenant.CreatedAt = DateTime.UtcNow;

        await _tenantRepository.AddAsync(tenant);

        var tenantDto = _mapper.Map<TenantDto>(tenant);

        return CreatedAtAction(nameof(GetTenant), new { id = tenant.Id }, tenantDto);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<TenantDto>> UpdateTenant(Guid id, [FromBody] UpdateTenantDto updateTenantDto)
    {
        var tenant = await _tenantRepository.GetByIdAsync(id);

        if (tenant == null)
        {
            return NotFound(new { message = "Tenant not found" });
        }

        if (tenant.Email != updateTenantDto.Email)
        {
            var existingTenant = await _tenantRepository.GetByEmailAsync(updateTenantDto.Email);
            if (existingTenant != null)
            {
                return BadRequest(new { message = "Email already exists" });
            }
        }

        _mapper.Map(updateTenantDto, tenant);
        tenant.ModifiedAt = DateTime.UtcNow;

        await _tenantRepository.UpdateAsync(tenant);

        return Ok(_mapper.Map<TenantDto>(tenant));
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteTenant(Guid id)
    {
        var tenant = await _tenantRepository.GetByIdAsync(id);

        if (tenant == null)
        {
            return NotFound(new { message = "Tenant not found" });
        }

        await _tenantRepository.DeleteAsync(id);

        return NoContent();
    }
}