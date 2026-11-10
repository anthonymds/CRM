using CRM.Application.Interfaces;
using CRM.Domain.Entities;
using CRM.Domain.Enums;
using CRM.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CRM.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly CrmDbContext _context;
    public CustomerRepository(
        CrmDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Customer customer, CancellationToken cancellationToken)
    {
        await _context.Customers.AddAsync(customer, cancellationToken);
        await _context.SaveChangesAsync();
    }


    public async Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Customers.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<List<Customer>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Customers.ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(Customer customer, CancellationToken cancellationToken)
    {
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistCustomerByDocument(string customerDocument, CancellationToken cancellationToken)
    {
        var result = await _context.Customers.FirstOrDefaultAsync(x => x.Document == customerDocument, cancellationToken);

        if (result == null)
            return false;

        return true;
    }
    public async Task<List<Customer>> GetByStatusAsync(CustomerStatus status, CancellationToken cancellationToken)
    {
        return await _context.Customers.Where(x => x.Status == status).ToListAsync(cancellationToken);
    }
}