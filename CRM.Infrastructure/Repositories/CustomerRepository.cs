using CRM.Application.Interfaces;
using CRM.Domain.Entities;

namespace CRM.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    public Task AddAsync(Customer customer)
    {
        // futuramente Entity Framework aqui

        return Task.CompletedTask;
    }


    public Task<Customer?> GetByIdAsync(Guid id)
    {
        return Task.FromResult<Customer?>(null);
    }
}