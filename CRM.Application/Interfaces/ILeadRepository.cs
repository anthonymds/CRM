using CRM.Domain.Entities;
using CRM.Domain.Enums;

namespace CRM.Application.Interfaces;

public interface ILeadRepository
{
    Task AddAsync(Lead lead, CancellationToken cancellationToken);
    Task<Lead?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<Lead>> GetAllAsync(CancellationToken cancellationToken);
    Task<List<Lead>> GetByStatusAsync(LeadStatus status, CancellationToken cancellationToken);
    Task<bool> ExistsByDocumentAsync(string document, CancellationToken cancellationToken);
    Task UpdateAsync(Lead lead, CancellationToken cancellationToken);
}
