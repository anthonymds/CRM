using CRM.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CRM.IntegrationTests.Fixtures;

public class DatabaseFixture : IDisposable
{
    public CrmDbContext Context { get; }

    public DatabaseFixture()
    {
        var options = new DbContextOptionsBuilder<CrmDbContext>()
            .UseNpgsql(
                "Host=localhost;" +
                "Port=5432;" +
                "Database=crm;" +
                "Username=postgres;" +
                "Password=postgres")
            .Options;

        Context = new CrmDbContext(options);

        Context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        Context.Dispose();
    }
}