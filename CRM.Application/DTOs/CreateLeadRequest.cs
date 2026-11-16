using CRM.Domain.Enums;

namespace CRM.Application.DTOs;

public class CreateLeadRequest
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Document { get; set; }
    public LeadSource Source { get; set; }
}
