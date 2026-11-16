using CRM.Application.Interfaces;
using CRM.Domain.Entities;
using CRM.Domain.Enums;
using CRM.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CRM.Infrastructure.Repositories;

public class LeadRepository : ILeadRepository
{
    private readonly CrmDbContext _context;
    public LeadRepository(CrmDbContext context)
    {
        _context = context;
    }
    public async Task AddAsync(Lead lead, CancellationToken cancellationToken)
    {
        await _context.AddAsync(lead);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsByDocumentAsync(string document, CancellationToken cancellationToken)
    {
        var result = await _context.Leads.FirstOrDefaultAsync(x => x.Document == document, cancellationToken);
        if (result == null)
            return false;

        return true;
    }

    public async Task<List<Lead>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Leads.ToListAsync(cancellationToken);
    }

    public async Task<Lead?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Leads.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<List<Lead>> GetByStatusAsync(LeadStatus status, CancellationToken cancellationToken)
    {
        return await _context.Leads.Where(x => x.Status == status).ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(Lead lead, CancellationToken cancellationToken)
    {
        _context.Leads.Update(lead);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
