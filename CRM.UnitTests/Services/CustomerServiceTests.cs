using AutoMapper;
using CRM.Application.DTOs;
using CRM.Application.Interfaces;
using CRM.Application.Services;
using CRM.Domain.Entities;
using CRM.Domain.Enums;
using Moq;

namespace CRM.UnitTests.Services;

public class CustomerServiceTests
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ICustomerRepository> _repositoryMock;
    private readonly CustomerService _service;

    public CustomerServiceTests()
    {
        _repositoryMock = new Mock<ICustomerRepository>();
        _mapperMock = new Mock<IMapper>();

        _service = new CustomerService(
            _mapperMock.Object,
            _repositoryMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateCustomer_WhenDocumentDoesNotExist()
    {
        // Arrange
        var request = new CreateCustomerRequest
        {
            Name = "Empresa Teste",
            Email = "teste@email.com",
            Phone = "79999999999",
            Document = "12345678900",
            Type = CustomerType.Company
        };

        var customer = new Customer();

        var response = new CustomerResponse
        {
            Name = request.Name,
            Email = request.Email,
            Document = request.Document
        };

        _repositoryMock
            .Setup(x => x.ExistCustomerByDocument(request.Document))
            .ReturnsAsync(false);

        _mapperMock
            .Setup(x => x.Map<Customer>(request))
            .Returns(customer);

        _mapperMock
            .Setup(x => x.Map<CustomerResponse>(customer))
            .Returns(response);

        // Act
        var result = await _service.CreateAsync(request);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);

        Assert.Equal(
            request.Name,
            result.Data.Name);

        _repositoryMock.Verify(
            x => x.AddAsync(customer),
            Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldFail_WhenDocumentAlreadyExists()
    {
        // Arrange
        var request = new CreateCustomerRequest
        {
            Name = "Empresa Teste",
            Email = "teste@email.com",
            Phone = "79999999999",
            Document = "12345678900",
            Type = CustomerType.Company
        };

        var customer = new Customer();

        var response = new CustomerResponse
        {
            Name = request.Name,
            Email = request.Email,
            Document = request.Document
        };

        _repositoryMock
            .Setup(x => x.ExistCustomerByDocument(request.Document))
            .ReturnsAsync(true);

        _mapperMock
            .Setup(x => x.Map<Customer>(request))
            .Returns(customer);

        _mapperMock
            .Setup(x => x.Map<CustomerResponse>(customer))
            .Returns(response);

        // Act
        var result = await _service.CreateAsync(request);

        // Assert
        Assert.False(result.IsSuccess);

        Assert.Equal(
        "Already exist a costumer with this document.",
        result.Error);

        _repositoryMock.Verify(
        x => x.AddAsync(It.IsAny<Customer>()),
        Times.Never);
    }

    [Fact]
    public async Task CreateAsync_ShouldFail_WhenDocumentIsNull()
    {
        // Arrange
        var request = new CreateCustomerRequest
        {
            Name = "Empresa Teste",
            Email = "teste@email.com",
            Phone = "79999999999",
            Document = null,
            Type = CustomerType.Company
        };

        var customer = new Customer();

        var response = new CustomerResponse
        {
            Name = request.Name,
            Email = request.Email,
            Document = request.Document
        };

        _repositoryMock
            .Setup(x => x.ExistCustomerByDocument(request.Document))
            .ReturnsAsync(false);

        _mapperMock
            .Setup(x => x.Map<Customer>(request))
            .Returns(customer);

        _mapperMock
            .Setup(x => x.Map<CustomerResponse>(customer))
            .Returns(response);

        // Act
        var result = await _service.CreateAsync(request);

        // Assert
        Assert.False(result.IsSuccess);

        Assert.Equal(
        "The document field is empty or null, please enter one.",
        result.Error);

        _repositoryMock.Verify(
        x => x.AddAsync(It.IsAny<Customer>()),
        Times.Never);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCustomer_WhenCustomerExists()
    {
        // Arrange
        Guid customerId = Guid.NewGuid();

        var customer = new Customer
        {
            Id = customerId,
            Name = "Empresa Teste",
            Email = "teste@email.com",
            Document = "12345678900",
            Status = CustomerStatus.Active
        };

        var response = new CustomerResponse
        {
            Id = customerId,
            Name = customer.Name,
            Email = customer.Email,
            Document = customer.Document,
            Status = customer.Status
        };

        _repositoryMock
            .Setup(x => x.GetByIdAsync(customerId))
            .ReturnsAsync(customer);

        _mapperMock
            .Setup(x => x.Map<CustomerResponse>(customer))
            .Returns(response);

        // Act
        var result = await _service.GetByIdAsync(customerId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(customerId, result.Data.Id);
        Assert.Equal(customer.Name, result.Data.Name);
        Assert.Equal(customer.Email, result.Data.Email);

        _repositoryMock.Verify(
            x => x.GetByIdAsync(customerId),
            Times.Once);

        _mapperMock.Verify(
            x => x.Map<CustomerResponse>(customer),
            Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldFail_WhenCustomerDoesNotExist()
    {
        // Arrange
        var customerId = Guid.NewGuid();

        _repositoryMock
            .Setup(x => x.GetByIdAsync(customerId))
            .ReturnsAsync((Customer?)null);

        // Act
        var result = await _service.GetByIdAsync(customerId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Null(result.Data);

        Assert.Equal(
        "Customer not found or do not exist",
        result.Error);

        _repositoryMock.Verify(
            x => x.GetByIdAsync(customerId),
            Times.Once);

        _mapperMock.Verify(
            x => x.Map<CustomerResponse>(
                It.IsAny<Customer>()),
            Times.Never);
    }

    [Fact]
    public async Task GetByStatusAsync_ShouldReturnCustomersWithRequestedStatus()
    {
        //arrange
        CustomerStatus customerStatusActive = CustomerStatus.Active;

        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = "teste 1",
            Email = "teste123@whitehouse.com",
            Phone = "7999999999",
            CreatedAt = DateTime.Now,
            Document = "123123123123",
            UpdatedAt = DateTime.Now,
            Type = CustomerType.Individual,
            Status = CustomerStatus.Active
        };
        var customer2 = new Customer
        {
            Id = Guid.NewGuid(),
            Name = "teste 2",
            Email = "teste222@whitehouse.com",
            Phone = "7999999999",
            CreatedAt = DateTime.Now,
            Document = "123123123123",
            UpdatedAt = DateTime.Now,
            Type = CustomerType.Company,
            Status = CustomerStatus.Active
        };
        var customer3 = new Customer
        {
            Id = Guid.NewGuid(),
            Name = "teste 3",
            Email = "teste321@whitehouse.com",
            Phone = "7999999999",
            CreatedAt = DateTime.Now,
            Document = "123123123123",
            UpdatedAt = DateTime.Now,
            Type = CustomerType.Individual,
            Status = CustomerStatus.Active
        };

        var customersList = new List<Customer>
        {
            customer,
            customer2,
            customer3
        };

        var responses = new List<CustomerResponse>
        {
            new CustomerResponse
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                Document = customer.Document,
                Status = customer.Status
            },
            new CustomerResponse
            {
                Id = customer2.Id,
                Name = customer2.Name,
                Email = customer2.Email,
                Document = customer2.Document,
                Status = customer2.Status
            },
            new CustomerResponse
            {
                Id = customer3.Id,
                Name = customer3.Name,
                Email = customer3.Email,
                Document = customer3.Document,
                Status = customer3.Status
            }
        };

        _repositoryMock
            .Setup(x => x.GetByStatusAsync(customerStatusActive))
            .ReturnsAsync(customersList);

        _mapperMock
            .Setup(x => x.Map<List<CustomerResponse>>(customersList))
            .Returns(responses);

        //act
        var result = await _service.GetByStatusAsync(customerStatusActive);

        // Assert
        Assert.NotNull(result.Data!.FirstOrDefault());
        Assert.Equal(3, result.Data!.Count());

        Assert.Equal(
            customer.Id,
            result.Data![0].Id);

        Assert.Equal(
            customer2.Id,
            result.Data![1].Id);

        Assert.Equal(
            customer3.Id,
            result.Data![2].Id);

        _repositoryMock.Verify(
            x => x.GetByStatusAsync(customerStatusActive),
            Times.Once);

        _mapperMock.Verify(
            x => x.Map<List<CustomerResponse>>(customersList),
            Times.Once);

    }

    [Fact]
    public async Task GetAll_ShouldReturnAllCustomers()
    {
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = "teste 1",
            Email = "teste123@whitehouse.com",
            Phone = "7999999999",
            CreatedAt = DateTime.Now,
            Document = "123123123123",
            UpdatedAt = DateTime.Now,
            Type = CustomerType.Individual,
            Status = CustomerStatus.Active
        };
        var customer2 = new Customer
        {
            Id = Guid.NewGuid(),
            Name = "teste 2",
            Email = "teste222@whitehouse.com",
            Phone = "7999999999",
            CreatedAt = DateTime.Now,
            Document = "123123123123",
            UpdatedAt = DateTime.Now,
            Type = CustomerType.Company,
            Status = CustomerStatus.Active
        };
        var customer3 = new Customer
        {
            Id = Guid.NewGuid(),
            Name = "teste 3",
            Email = "teste321@whitehouse.com",
            Phone = "7999999999",
            CreatedAt = DateTime.Now,
            Document = "123123123123",
            UpdatedAt = DateTime.Now,
            Type = CustomerType.Individual,
            Status = CustomerStatus.Active
        };

        var customersList = new List<Customer>
        {
            customer,
            customer2,
            customer3
        };

        var responses = new List<CustomerResponse>
        {
            new CustomerResponse
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                Document = customer.Document,
                Status = customer.Status
            },
            new CustomerResponse
            {
                Id = customer2.Id,
                Name = customer2.Name,
                Email = customer2.Email,
                Document = customer2.Document,
                Status = customer2.Status
            },
            new CustomerResponse
            {
                Id = customer3.Id,
                Name = customer3.Name,
                Email = customer3.Email,
                Document = customer3.Document,
                Status = customer3.Status
            }
        };

        _repositoryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(customersList);

        _mapperMock
            .Setup(x => x.Map<List<CustomerResponse>>(customersList))
            .Returns(responses);

        //act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result.Data!.First());
        Assert.Equal(3, result.Data!.Count());

        Assert.Equal(
            customer.Id,
            result.Data![0].Id);

        Assert.Equal(
            customer2.Id,
            result.Data![1].Id);

        Assert.Equal(
            customer3.Id,
            result.Data![2].Id);

        _repositoryMock.Verify(
            x => x.GetAllAsync(),
            Times.Once);

        _mapperMock.Verify(
            x => x.Map<List<CustomerResponse>>(customersList),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateCustomerById()
    {
        //Arrange
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = "teste 1",
            Email = "teste123@whitehouse.com",
            Phone = "7999999999",
            CreatedAt = DateTime.Now,
            Document = "123123123123",
            UpdatedAt = DateTime.Now,
            Type = CustomerType.Individual,
            Status = CustomerStatus.Active
        };

        var customerUpdated = new UpdateCustomerRequest()
        {
            Name = "teste 1 update",
            Email = "teste123Update@whitehouse.com",
            Phone = "76767676767"
        };

        var response = new CustomerResponse()
        {
            Id = customer.Id,
            Name = customerUpdated.Name,
            Email = customerUpdated.Email,
            Phone = customerUpdated.Phone,
            CreatedAt = customer.CreatedAt,
            Document = customer.Document,
            UpdatedAt = customer.UpdatedAt,
            Type = customer.Type,
            Status = customer.Status
        };

        _repositoryMock
            .Setup(x => x.GetByIdAsync(customer.Id))
            .ReturnsAsync(customer);
        _repositoryMock
            .Setup(x => x.UpdateAsync(customer))
            .Returns(Task.CompletedTask);

        _mapperMock
            .Setup(x => x.Map<CustomerResponse>(customer))
            .Returns(response);

        //act
        var result = await _service.UpdateAsync(customer.Id, customerUpdated);

        //assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);

        Assert.Equal(
            result.Data!.Name,
            customerUpdated.Name);
        Assert.Equal(
            result.Data!.Phone,
            customerUpdated.Phone);
        Assert.Equal(
            result.Data!.Email,
            customerUpdated.Email);

        _repositoryMock.Verify(
           x => x.GetByIdAsync(customer.Id),
           Times.Once);

        _repositoryMock.Verify(
            x => x.UpdateAsync(customer),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldFail_UpdateCustomerWithIdNotFound()
    {
        //Arrange
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = "teste 1",
            Email = "teste123@whitehouse.com",
            Phone = "7999999999",
            CreatedAt = DateTime.Now,
            Document = "123123123123",
            UpdatedAt = DateTime.Now,
            Type = CustomerType.Individual,
            Status = CustomerStatus.Active
        };

        var customerUpdated = new UpdateCustomerRequest()
        {
            Name = "teste 1 update",
            Email = "teste123Update@whitehouse.com",
            Phone = "76767676767"
        };

        var response = new CustomerResponse()
        {
            Id = customer.Id,
            Name = customerUpdated.Name,
            Email = customerUpdated.Email,
            Phone = customerUpdated.Phone,
            CreatedAt = customer.CreatedAt,
            Document = customer.Document,
            UpdatedAt = customer.UpdatedAt,
            Type = customer.Type,
            Status = customer.Status
        };

        _repositoryMock
            .Setup(x => x.GetByIdAsync(customer.Id))
            .ReturnsAsync((Customer?)null);

        _repositoryMock
            .Setup(x => x.UpdateAsync(customer))
            .Returns(Task.CompletedTask);

        _mapperMock
            .Setup(x => x.Map<CustomerResponse>(customer))
            .Returns(response);

        //act
        var result = await _service.UpdateAsync(customer.Id, customerUpdated);

        //assert
        Assert.False(result.IsSuccess);
        Assert.Null(result.Data);
        Assert.Equal("Customer not found or does not exist", result.Error);

        _repositoryMock.Verify(
           x => x.GetByIdAsync(customer.Id),
           Times.Once);

    }

    [Fact]
    public async Task DeleteCustomer_ShouldInactiveCustomer_WithCorrespondedId()
    {
        //Arrange
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = "teste 1",
            Email = "teste123@whitehouse.com",
            Phone = "7999999999",
            CreatedAt = DateTime.Now,
            Document = "123123123123",
            UpdatedAt = DateTime.Now,
            Type = CustomerType.Individual,
            Status = CustomerStatus.Active
        };

        var response = new CustomerResponse()
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email,
            Phone = customer.Phone,
            CreatedAt = customer.CreatedAt,
            Document = customer.Document,
            UpdatedAt = customer.UpdatedAt,
            Type = customer.Type,
            Status = CustomerStatus.Inactive
        };

        _repositoryMock
            .Setup(x => x.GetByIdAsync(customer.Id))
            .ReturnsAsync(customer);

        _repositoryMock
            .Setup(x => x.UpdateAsync(customer))
            .Returns(Task.CompletedTask);

        _mapperMock
            .Setup(x => x.Map<CustomerResponse>(customer))
            .Returns(response);

        //act
        var result = await _service.DeleteAsync(customer.Id);

        //assert
        Assert.True(result.IsSuccess);

        Assert.Equal(
            CustomerStatus.Inactive,
            customer.Status);

        _repositoryMock.Verify(
           x => x.GetByIdAsync(customer.Id),
           Times.Once);

        _repositoryMock.Verify(
            x => x.UpdateAsync(customer),
            Times.Once);
    }
}
