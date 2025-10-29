namespace Tms.Application.DTOs;

public record LeaseDto(
    Guid Id,
    Guid TenantId,
    string PropertyAddress,
    DateTime StartDate,
    DateTime? EndDate,
    decimal MonthlyRent,
    decimal? Deposit
);

public record CreateLeaseDto(
    string PropertyAddress,
    DateTime StartDate,
    DateTime? EndDate,
    decimal MonthlyRent,
    decimal? Deposit
);

public record UpdateLeaseDto(
    string PropertyAddress,
    DateTime StartDate,
    DateTime? EndDate,
    decimal MonthlyRent,
    decimal? Deposit
);