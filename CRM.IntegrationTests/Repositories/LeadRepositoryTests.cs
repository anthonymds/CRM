using CRM.Domain.Entities;
using CRM.Domain.Enums;
using CRM.Infrastructure.Repositories;
using CRM.IntegrationTests.Fixtures;

namespace CRM.IntegrationTests.Repositories;

public class LeadRepositoryTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;

    public LeadRepositoryTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task AddAsync_ShouldPersistLead()
    {
        // Arrange

        var cancellationToken = CancellationToken.None;

        var lead = new Lead
        {
            Id = Guid.NewGuid(),
            Name = "Integration Test",
            Email = "integration@test.com",
            Phone = "79999999999",
            Document = "81818181818181",
            LastUpdatedAt = DateTime.Now,
            Source = LeadSource.Advertisement,
            Status = LeadStatus.New
        };

        var repository = new LeadRepository(_fixture.Context);

        // Act
        await repository.AddAsync(lead, cancellationToken);

        // Assert
        var savedLead =
            await _fixture.Context.Leads
                .FindAsync(lead.Id, cancellationToken);

        Assert.NotNull(savedLead);

        Assert.Equal(
            lead.Id,
            savedLead.Id);

        Assert.Equal(
            lead.Name,
            savedLead.Name);

        Assert.Equal(
            lead.Email,
            savedLead.Email);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnLead_WhenLeadExists()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;

        var lead = new Lead
        {
            Id = Guid.NewGuid(),
            Name = "Integration Test",
            Email = "integration@test.com",
            Phone = "79999999999",
            Document = "127384023789",
            LastUpdatedAt = DateTime.Now,
            Source = LeadSource.Advertisement,
            Status = LeadStatus.New
        };

        var repository = new LeadRepository(_fixture.Context);

        var alreadyExist = await repository.ExistsByDocumentAsync(lead.Document, cancellationToken);
        if (alreadyExist == false)
            await repository.AddAsync(lead, cancellationToken);

        // Act
        var result = await repository.GetByIdAsync(lead.Id, cancellationToken);

        // Assert
        Assert.NotNull(result);

        Assert.Equal(
            lead.Id,
            result.Id);

        Assert.Equal(
            lead.Name,
            result.Name);

        Assert.Equal(
            lead.Email,
            result.Email);

        Assert.Equal(
            lead.Document,
            result.Document);

    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenLeadDoesNotExist()
    {
        // Arrange
        var leadId = Guid.NewGuid();
        var cancellationToken = CancellationToken.None;
        var repository = new LeadRepository(_fixture.Context);

        // Act
        var result = await repository.GetByIdAsync(leadId, cancellationToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByStatusAsync_ShouldReturnLeadsWithRequestedStatus()
    {
        //arrange
        LeadStatus leadStatusActive = LeadStatus.New;
        var cancellationToken = CancellationToken.None;
        var repository = new LeadRepository(_fixture.Context);

        var lead = new Lead
        {
            Id = Guid.NewGuid(),
            Name = "teste 1",
            Email = "teste123@whitehouse.com",
            Phone = "7999999999",
            CreatedAt = DateTime.UtcNow,
            Document = "123123123123",
            LastUpdatedAt = DateTime.Now,
            Source = LeadSource.Advertisement,
            Status = LeadStatus.New
        };
        var lead2 = new Lead
        {
            Id = Guid.NewGuid(),
            Name = "teste 2",
            Email = "teste222@whitehouse.com",
            Phone = "7999999999",
            CreatedAt = DateTime.UtcNow,
            Document = "456456456456",
            LastUpdatedAt = DateTime.Now,
            Source = LeadSource.Advertisement,
            Status = LeadStatus.New
        };
        var lead3 = new Lead
        {
            Id = Guid.NewGuid(),
            Name = "teste 3",
            Email = "teste321@whitehouse.com",
            Phone = "7999999999",
            CreatedAt = DateTime.UtcNow,
            Document = "789789789789",
            LastUpdatedAt = DateTime.Now,
            Source = LeadSource.Advertisement,
            Status = LeadStatus.New
        };

        var alreadyExist = await repository.ExistsByDocumentAsync(lead.Document, cancellationToken);
        if (alreadyExist == false)
            await repository.AddAsync(lead, cancellationToken);

        var alreadyExist2 = await repository.ExistsByDocumentAsync(lead2.Document, cancellationToken);
        if (alreadyExist2 == false)
            await repository.AddAsync(lead2, cancellationToken);

        var alreadyExist3 = await repository.ExistsByDocumentAsync(lead3.Document, cancellationToken);
        if (alreadyExist3 == false)
            await repository.AddAsync(lead3, cancellationToken);

        //act
        var result = await repository.GetByStatusAsync(leadStatusActive, cancellationToken);

        // Assert

        Assert.Contains(result,
         x => x.Id == lead.Id);

        Assert.Contains(result,
            x => x.Id == lead2.Id);

        Assert.Contains(result,
            x => x.Id == lead3.Id);

    }

    [Fact]
    public async Task UpdateAsync_ShouldChangeLeadFields()
    {
        //arrange

        var cancellationToken = CancellationToken.None;
        var repository = new LeadRepository(_fixture.Context);

        var lead = new Lead
        {
            Id = Guid.NewGuid(),
            Name = "teste 3",
            Email = "teste321@whitehouse.com",
            Phone = "7999999999",
            CreatedAt = DateTime.UtcNow,
            Document = "789789789789",
            LastUpdatedAt = DateTime.Now,
            Source = LeadSource.Advertisement,
            Status = LeadStatus.New
        };

        //act
        var alreadyExist = await repository.ExistsByDocumentAsync(lead.Document, cancellationToken);
        if (alreadyExist == false)
            await repository.AddAsync(lead, cancellationToken);
        var response = await repository.GetByIdAsync(lead.Id, cancellationToken);

        response.Name = "update test integration";
        response.Status = LeadStatus.Lost;
        response.LastUpdatedAt = DateTime.UtcNow;

        await repository.UpdateAsync(response, cancellationToken);
        var result = await repository.GetByIdAsync(lead.Id, cancellationToken);

        //assert
        Assert.NotNull(result);

        Assert.Equal(
            response.Id,
            result.Id);

        Assert.Equal(
            response.Name,
            result.Name);

        Assert.Equal(
            response.Email,
            result.Email);

        Assert.Equal(
            response.Document,
            result.Document);
    }
}
