using CRM.Domain.Enums;
namespace CRM.Domain.Entities;

public class Customer
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? Document { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public CustomerType Type { get; set; }
    public CustomerStatus Status { get; set; }
}
