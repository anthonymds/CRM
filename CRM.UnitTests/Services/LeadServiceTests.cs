using AutoMapper;
using CRM.Application.DTOs;
using CRM.Application.Interfaces;
using CRM.Application.Services;
using CRM.Domain.Entities;
using CRM.Domain.Enums;
using Moq;

namespace CRM.UnitTests.Services;

public class LeadServiceTests
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILeadRepository> _repositoryMock;
    private readonly Mock<ICustomerService> _serviceCustomerMock;
    private readonly LeadService _service;


    public LeadServiceTests()
    {
        _serviceCustomerMock = new Mock<ICustomerService>();
        _repositoryMock = new Mock<ILeadRepository>();
        _mapperMock = new Mock<IMapper>();

        _service = new LeadService(
            _mapperMock.Object,
            _repositoryMock.Object,
            _serviceCustomerMock.Object);
    }


    [Fact]
    public async Task CreateAsync_ShouldCreateLead_WhenDocumentDoesNotExist()
    {
        // Arrange
        var request = new CreateLeadRequest
        {
            Name = "Empresa Teste",
            Email = "teste@email.com",
            Phone = "79999999999",
            Document = "12345678900",
            Source = LeadSource.SocialMedia
        };

        var lead = new Lead();

        var response = new LeadResponse
        {
            Name = request.Name,
            Email = request.Email,
            Document = request.Document
        };

        _repositoryMock
            .Setup(x => x.ExistsByDocumentAsync(request.Document, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _mapperMock
            .Setup(x => x.Map<Lead>(request))
            .Returns(lead);

        _mapperMock
            .Setup(x => x.Map<LeadResponse>(lead))
            .Returns(response);

        // Act
        var result = await _service.CreateAsync(request, It.IsAny<CancellationToken>());

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);

        Assert.Equal(
            request.Name,
            result.Data.Name);

        _repositoryMock.Verify(
            x => x.AddAsync(lead, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldFail_WhenDocumentAlreadyExists()
    {
        // Arrange
        var request = new CreateLeadRequest
        {
            Name = "Empresa Teste",
            Email = "teste@email.com",
            Phone = "79999999999",
            Document = "12345678900",
            Source = LeadSource.SocialMedia
        };

        var lead = new Lead();

        var response = new LeadResponse
        {
            Name = request.Name,
            Email = request.Email,
            Document = request.Document
        };

        _repositoryMock
            .Setup(x => x.ExistsByDocumentAsync(request.Document, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _mapperMock
            .Setup(x => x.Map<Lead>(request))
            .Returns(lead);

        _mapperMock
            .Setup(x => x.Map<LeadResponse>(lead))
            .Returns(response);

        // Act
        var result = await _service.CreateAsync(request, It.IsAny<CancellationToken>());

        // Assert
        Assert.False(result.IsSuccess);

        Assert.Equal(
        "Already exist a Lead with this document.",
        result.Error);

        _repositoryMock.Verify(
        x => x.AddAsync(It.IsAny<Lead>(), It.IsAny<CancellationToken>()),
        Times.Never);
    }

    [Fact]
    public async Task CreateAsync_ShouldFail_WhenDocumentIsNull()
    {
        // Arrange
        var request = new CreateLeadRequest
        {
            Name = "Empresa Teste",
            Email = "teste@email.com",
            Phone = "79999999999",
            Document = null,
            Source = LeadSource.SocialMedia
        };

        var lead = new Lead();

        var response = new LeadResponse
        {
            Name = request.Name,
            Email = request.Email,
            Document = request.Document
        };

        _repositoryMock
            .Setup(x => x.ExistsByDocumentAsync(request.Document!, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _mapperMock
            .Setup(x => x.Map<Lead>(request))
            .Returns(lead);

        _mapperMock
            .Setup(x => x.Map<LeadResponse>(lead))
            .Returns(response);

        // Act
        var result = await _service.CreateAsync(request, It.IsAny<CancellationToken>());

        // Assert
        Assert.False(result.IsSuccess);

        Assert.Equal(
        "The document field is empty or null, please enter one.",
        result.Error);

        _repositoryMock.Verify(
        x => x.AddAsync(It.IsAny<Lead>(), It.IsAny<CancellationToken>()),
        Times.Never);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnLead_WhenLeadExists()
    {
        // Arrange
        Guid leadId = Guid.NewGuid();

        var lead = new Lead
        {
            Id = leadId,
            Name = "Empresa Teste",
            Email = "teste@email.com",
            Document = "12345678900",
            Source = LeadSource.SocialMedia
        };

        var response = new LeadResponse
        {
            Id = leadId,
            Name = lead.Name,
            Email = lead.Email,
            Document = lead.Document,
            Status = lead.Status
        };

        _repositoryMock
            .Setup(x => x.GetByIdAsync(leadId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(lead);

        _mapperMock
            .Setup(x => x.Map<LeadResponse>(lead))
            .Returns(response);

        // Act
        var result = await _service.GetByIdAsync(leadId, It.IsAny<CancellationToken>());

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(leadId, result.Data.Id);
        Assert.Equal(lead.Name, result.Data.Name);
        Assert.Equal(lead.Email, result.Data.Email);

        _repositoryMock.Verify(
            x => x.GetByIdAsync(leadId, It.IsAny<CancellationToken>()),
            Times.Once);

        _mapperMock.Verify(
            x => x.Map<LeadResponse>(lead),
            Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldFail_WhenLeadDoesNotExist()
    {
        // Arrange
        var leadId = Guid.NewGuid();

        _repositoryMock
            .Setup(x => x.GetByIdAsync(leadId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Lead?)null);

        // Act
        var result = await _service.GetByIdAsync(leadId, It.IsAny<CancellationToken>());

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Null(result.Data);

        Assert.Equal(
        "Lead not found or do not exist",
        result.Error);

        _repositoryMock.Verify(
            x => x.GetByIdAsync(leadId, It.IsAny<CancellationToken>()),
            Times.Once);

        _mapperMock.Verify(
            x => x.Map<LeadResponse>(
                It.IsAny<Lead>()),
            Times.Never);
    }

    [Fact]
    public async Task GetByStatusAsync_ShouldReturnLeadsWithRequestedStatus()
    {
        //arrange
        LeadStatus leadStatusActive = LeadStatus.New;

        var lead = new Lead
        {
            Id = Guid.NewGuid(),
            Name = "teste 1",
            Email = "teste123@whitehouse.com",
            Phone = "7999999999",
            CreatedAt = DateTime.Now,
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
            CreatedAt = DateTime.Now,
            Document = "123123123123",
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
            CreatedAt = DateTime.Now,
            Document = "123123123123",
            LastUpdatedAt = DateTime.Now,
            Source = LeadSource.Advertisement,
            Status = LeadStatus.New
        };

        var leadsList = new List<Lead>
        {
            lead,
            lead2,
            lead3
        };

        var responses = new List<LeadResponse>
        {
            new LeadResponse
            {
                Id = lead.Id,
                Name = lead.Name,
                Email = lead.Email,
                Document = lead.Document,
                Status = lead.Status
            },
            new LeadResponse
            {
                Id = lead2.Id,
                Name = lead2.Name,
                Email = lead2.Email,
                Document = lead2.Document,
                Status = lead2.Status
            },
            new LeadResponse
            {
                Id = lead3.Id,
                Name = lead3.Name,
                Email = lead3.Email,
                Document = lead3.Document,
                Status = lead3.Status
            }
        };

        _repositoryMock
            .Setup(x => x.GetByStatusAsync(leadStatusActive, It.IsAny<CancellationToken>()))
            .ReturnsAsync(leadsList);

        _mapperMock
            .Setup(x => x.Map<List<LeadResponse>>(leadsList))
            .Returns(responses);

        //act
        var result = await _service.GetByStatusAsync(leadStatusActive, It.IsAny<CancellationToken>());

        // Assert
        Assert.NotNull(result.Data!.FirstOrDefault());
        Assert.Equal(3, result.Data!.Count());

        Assert.Equal(
            lead.Id,
            result.Data![0].Id);

        Assert.Equal(
            lead2.Id,
            result.Data![1].Id);

        Assert.Equal(
            lead3.Id,
            result.Data![2].Id);

        _repositoryMock.Verify(
            x => x.GetByStatusAsync(leadStatusActive, It.IsAny<CancellationToken>()),
            Times.Once);

        _mapperMock.Verify(
            x => x.Map<List<LeadResponse>>(leadsList),
            Times.Once);

    }

    [Fact]
    public async Task GetAll_ShouldReturnAllLeads()
    {
        var lead = new Lead
        {
            Id = Guid.NewGuid(),
            Name = "teste 1",
            Email = "teste123@whitehouse.com",
            Phone = "7999999999",
            CreatedAt = DateTime.Now,
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
            CreatedAt = DateTime.Now,
            Document = "123123123123",
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
            CreatedAt = DateTime.Now,
            Document = "123123123123",
            LastUpdatedAt = DateTime.Now,
            Source = LeadSource.Advertisement,
            Status = LeadStatus.New
        };

        var leadsList = new List<Lead>
        {
            lead,
            lead2,
            lead3
        };

        var responses = new List<LeadResponse>
        {
            new LeadResponse
            {
                Id = lead.Id,
                Name = lead.Name,
                Email = lead.Email,
                Document = lead.Document,
                Status = lead.Status
            },
            new LeadResponse
            {
                Id = lead2.Id,
                Name = lead2.Name,
                Email = lead2.Email,
                Document = lead2.Document,
                Status = lead2.Status
            },
            new LeadResponse
            {
                Id = lead3.Id,
                Name = lead3.Name,
                Email = lead3.Email,
                Document = lead3.Document,
                Status = lead3.Status
            }
        };

        _repositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(leadsList);

        _mapperMock
            .Setup(x => x.Map<List<LeadResponse>>(leadsList))
            .Returns(responses);

        //act
        var result = await _service.GetAllAsync(It.IsAny<CancellationToken>());

        // Assert
        Assert.NotNull(result.Data!.First());
        Assert.Equal(3, result.Data!.Count());

        Assert.Equal(
            lead.Id,
            result.Data![0].Id);

        Assert.Equal(
            lead2.Id,
            result.Data![1].Id);

        Assert.Equal(
            lead3.Id,
            result.Data![2].Id);

        _repositoryMock.Verify(
            x => x.GetAllAsync(It.IsAny<CancellationToken>()),
            Times.Once);

        _mapperMock.Verify(
            x => x.Map<List<LeadResponse>>(leadsList),
            Times.Once);
    }

}
