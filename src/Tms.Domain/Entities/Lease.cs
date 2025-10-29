namespace Tms.Domain.Entities;

public class Lease
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string PropertyAddress { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal MonthlyRent { get; set; }
    public decimal? Deposit { get; set; }

    // Navigation property
    public Tenant Tenant { get; set; } = null!;
}