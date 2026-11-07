using CRM.Application.Common;
using CRM.Application.DTOs;
using CRM.Domain.Entities;
using CRM.Domain.Enums;
namespace CRM.Application.Interfaces;

public interface ICustomerService
{
    Task<Result<CustomerResponse>> CreateAsync(CreateCustomerRequest request);
    Task<Result<List<CustomerResponse>>> GetAllAsync();
    Task<Result<CustomerResponse>> GetByIdAsync(Guid id);
    Task<Result<CustomerResponse>> UpdateAsync(Guid id, UpdateCustomerRequest request);
    Task<Result<bool>> DeleteAsync(Guid id);
    Task<List<Customer>> GetByStatusAsync(CustomerStatus status);
}
