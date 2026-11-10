using CRM.Application.Common;
using CRM.Application.DTOs;
using CRM.Domain.Enums;
namespace CRM.Application.Interfaces;

public interface ICustomerService
{
    Task<Result<CustomerResponse>> CreateAsync(CreateCustomerRequest request, CancellationToken cancellationToken);
    Task<Result<List<CustomerResponse>>> GetAllAsync(CancellationToken cancellationToken);
    Task<Result<CustomerResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Result<CustomerResponse>> UpdateAsync(Guid id, UpdateCustomerRequest request, CancellationToken cancellationToken);
    Task<Result<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<Result<List<CustomerResponse>>> GetByStatusAsync(CustomerStatus status, CancellationToken cancellationToken);
}
