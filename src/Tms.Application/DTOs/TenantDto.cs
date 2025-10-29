namespace Tms.Application.DTOs;

public record TenantDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string Phone,
    DateTime CreatedAt,
    DateTime? ModifiedAt
);

public record CreateTenantDto(
    string FirstName,
    string LastName,
    string Email,
    string Phone
);

public record UpdateTenantDto(
    string FirstName,
    string LastName,
    string Email,
    string Phone
);