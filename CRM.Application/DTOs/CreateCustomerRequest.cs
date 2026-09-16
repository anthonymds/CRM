using CRM.Domain.Enums;

namespace CRM.Application.DTOs;

public class CreateCustomerRequest
{
    public string? Name { get; set; }
    public string? Document { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public CustomerType Type { get; set; }
}
