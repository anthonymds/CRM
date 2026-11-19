using CRM.Application.Common;
using CRM.Application.DTOs;
using CRM.Domain.Enums;

namespace CRM.Application.Interfaces;

public interface ILeadService
{
    Task<Result<LeadResponse>> CreateAsync(CreateLeadRequest request, CancellationToken cancellationToken);
    Task<Result<List<LeadResponse>>> GetAllAsync(CancellationToken cancellationToken);
    Task<Result<LeadResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Result<LeadResponse>> UpdateAsync(Guid id, UpdateLeadRequest request, CancellationToken cancellationToken);
    Task<Result<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<Result<List<LeadResponse>>> GetByStatusAsync(LeadStatus status, CancellationToken cancellationToken);
    Task<Result<LeadResponse>> ConvertAsync(Guid id, ConvertLeadRequest convertLeadRequest, CancellationToken cancellationToken);
}
