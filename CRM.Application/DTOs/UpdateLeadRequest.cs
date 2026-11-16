using CRM.Domain.Enums;

namespace CRM.Application.DTOs;

public class UpdateLeadRequest
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public LeadSource Source { get; set; }

}
