using CRM.Domain.Entities;
namespace CRM.Application.Interfaces;

public interface ICustomerRepository
{
    Task AddAsync(Customer customer);
    Task<Customer?> GetByIdAsync(Guid id);
}