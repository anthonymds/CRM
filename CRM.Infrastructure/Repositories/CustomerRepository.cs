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

    public async Task AddAsync(Customer customer)
    {
        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();
    }


    public async Task<Customer?> GetByIdAsync(Guid id)
    {
        return await _context.Customers.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<Customer>> GetAllAsync()
    {
        return await _context.Customers.ToListAsync();
    }

    public async Task UpdateAsync(Customer customer)
    {
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistCustomerByDocument(string customerDocument)
    {
        var result = await _context.Customers.FirstOrDefaultAsync(x => x.Document == customerDocument);

        if (result == null)
            return false;

        return true;
    }
    public async Task<List<Customer>> GetByStatusAsync(CustomerStatus status)
    {
        return await _context.Customers.Where(x => x.Status == status).ToListAsync();
    }
}