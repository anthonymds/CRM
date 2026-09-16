using CRM.Application.DTOs;
namespace CRM.Application.Interfaces;

public interface ICustomerService
{
    Task<CustomerResponse> CreateAsync(CreateCustomerRequest request);
}
