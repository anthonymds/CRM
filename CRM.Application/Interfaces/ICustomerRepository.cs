using CRM.Domain.Entities;
using CRM.Domain.Enums;
namespace CRM.Application.Interfaces;

public interface ICustomerRepository
{
    Task AddAsync(Customer customer, CancellationToken cancellationToken);
    Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<Customer>> GetAllAsync(CancellationToken cancellationToken);
    Task UpdateAsync(Customer customer, CancellationToken cancellationToken);
    Task<bool> ExistCustomerByDocument(string customerDocument, CancellationToken cancellationToken);
    Task<List<Customer>> GetByStatusAsync(CustomerStatus status, CancellationToken cancellationToken);
}