using CRM.Domain.Entities;
using CRM.Domain.Enums;
using CRM.Infrastructure.Repositories;
using CRM.IntegrationTests.Fixtures;

namespace CRM.IntegrationTests.Repositories;

public class CustomerRepositoryTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;

    public CustomerRepositoryTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task AddAsync_ShouldPersistCustomer()
    {
        // Arrange

        var cancellationToken = CancellationToken.None;

        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = "Integration Test",
            Email = "integration@test.com",
            Phone = "79999999999",
            Document = "127384023789",
            CreatedAt = DateTime.UtcNow,
            Type = CustomerType.Individual,
            Status = CustomerStatus.Active
        };

        var repository = new CustomerRepository(_fixture.Context);

        // Act
        await repository.AddAsync(customer, cancellationToken);

        // Assert
        var savedCustomer =
            await _fixture.Context.Customers
                .FindAsync(customer.Id, cancellationToken);

        Assert.NotNull(savedCustomer);

        Assert.Equal(
            customer.Id,
            savedCustomer.Id);

        Assert.Equal(
            customer.Name,
            savedCustomer.Name);

        Assert.Equal(
            customer.Email,
            savedCustomer.Email);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCustomer_WhenCustomerExists()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;

        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = "Integration Test",
            Email = "integration@test.com",
            Phone = "79999999999",
            Document = "127384023789",
            CreatedAt = DateTime.UtcNow,
            Type = CustomerType.Individual,
            Status = CustomerStatus.Active
        };

        var repository = new CustomerRepository(_fixture.Context);

        // already been aded to DB
        //await repository.AddAsync(customer, cancellationToken);

        // Act
        var result = await repository.GetByIdAsync(customer.Id, cancellationToken);

        // Assert
        Assert.NotNull(result);

        Assert.Equal(
            customer.Id,
            result.Id);

        Assert.Equal(
            customer.Name,
            result.Name);

        Assert.Equal(
            customer.Email,
            result.Email);

        Assert.Equal(
            customer.Document,
            result.Document);

    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenCustomerDoesNotExist()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var cancellationToken = CancellationToken.None;
        var repository = new CustomerRepository(_fixture.Context);

        // Act
        var result = await repository.GetByIdAsync(customerId, cancellationToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByStatusAsync_ShouldReturnCustomersWithRequestedStatus()
    {
        //arrange
        CustomerStatus customerStatusActive = CustomerStatus.Active;
        var cancellationToken = CancellationToken.None;
        var repository = new CustomerRepository(_fixture.Context);

        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = "teste 1",
            Email = "teste123@whitehouse.com",
            Phone = "7999999999",
            CreatedAt = DateTime.UtcNow,
            Document = "123123123123",
            UpdatedAt = DateTime.UtcNow,
            Type = CustomerType.Individual,
            Status = CustomerStatus.Active
        };
        var customer2 = new Customer
        {
            Id = Guid.NewGuid(),
            Name = "teste 2",
            Email = "teste222@whitehouse.com",
            Phone = "7999999999",
            CreatedAt = DateTime.UtcNow,
            Document = "456456456456",
            UpdatedAt = DateTime.UtcNow,
            Type = CustomerType.Company,
            Status = CustomerStatus.Active
        };
        var customer3 = new Customer
        {
            Id = Guid.NewGuid(),
            Name = "teste 3",
            Email = "teste321@whitehouse.com",
            Phone = "7999999999",
            CreatedAt = DateTime.UtcNow,
            Document = "789789789789",
            UpdatedAt = DateTime.UtcNow,
            Type = CustomerType.Individual,
            Status = CustomerStatus.Active
        };

        // already been aded to DB
        //await repository.AddAsync(customer, cancellationToken);
        //await repository.AddAsync(customer2, cancellationToken);
        //await repository.AddAsync(customer3, cancellationToken);
        //act
        var result = await repository.GetByStatusAsync(customerStatusActive, cancellationToken);

        // Assert

        Assert.Contains(result,
         x => x.Id == customer.Id);

        Assert.Contains(result,
            x => x.Id == customer2.Id);

        Assert.Contains(result,
            x => x.Id == customer3.Id);

    }
}