using AutoMapper;
using CRM.Application.DTOs;
using CRM.Application.Interfaces;
using CRM.Domain.Entities;
namespace CRM.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly IMapper _mapper;
    private readonly ICustomerRepository _customerRepository;
    public CustomerService(IMapper mapper, ICustomerRepository customerRepository)
    {
        _mapper = mapper;
        _customerRepository = customerRepository;
    }

    public async Task<CustomerResponse> CreateAsync(CreateCustomerRequest request)
    {
        var customer = _mapper.Map<Customer>(request);

        customer.Id = Guid.NewGuid();
        customer.Status = Domain.Enums.CustomerStatus.Active;
        customer.CreatedAt = DateTime.UtcNow;
        await _customerRepository.AddAsync(customer);

        return _mapper.Map<CustomerResponse>(customer);
    }
}
