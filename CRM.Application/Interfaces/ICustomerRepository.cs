using CRM.Domain.Entities;
using CRM.Domain.Enums;
namespace CRM.Application.Interfaces;

public interface ICustomerRepository
{
    Task AddAsync(Customer customer);
    Task<Customer?> GetByIdAsync(Guid id);
    Task<List<Customer>> GetAllAsync();
    Task UpdateAsync(Customer customer);
    Task<bool> ExistCustomerByDocument(string customerDocument);
    Task<List<Customer>> GetByStatusAsync(CustomerStatus status);
}