using AutoMapper;
using CRM.Application.Common;
using CRM.Application.DTOs;
using CRM.Application.Interfaces;
using CRM.Domain.Entities;
using CRM.Domain.Enums;

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

    public async Task<Result<CustomerResponse>> CreateAsync(CreateCustomerRequest request)
    {
        if (request.Document == null)
            return Result<CustomerResponse>.Failure("The document field is empty or null, please enter one.");
        if (await _customerRepository.ExistCustomerByDocument(request.Document))
            return Result<CustomerResponse>.Failure("Already exist a costumer with this document.");

        var customer = _mapper.Map<Customer>(request);

        customer.Id = Guid.NewGuid();
        customer.Status = CustomerStatus.Active;
        customer.CreatedAt = DateTime.UtcNow;

        await _customerRepository.AddAsync(customer);
        var response = _mapper.Map<CustomerResponse>(customer);

        return Result<CustomerResponse>.Success(response);
    }

    public async Task<Result<List<CustomerResponse>>> GetAllAsync()
    {
        var customers = await _customerRepository.GetAllAsync();
        var response = _mapper.Map<List<CustomerResponse>>(customers);
        return Result<List<CustomerResponse>>.Success(response);
    }

    public async Task<Result<CustomerResponse>> GetByIdAsync(Guid id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null)
            return Result<CustomerResponse>.Failure("Customer not found or do not exist");

        var response = _mapper.Map<CustomerResponse>(customer);
        return Result<CustomerResponse>.Success(response);
    }

    public async Task<Result<CustomerResponse>> UpdateAsync(Guid id, UpdateCustomerRequest request)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null)
            return Result<CustomerResponse>.Failure("Customer not found or does not exist");

        customer.Name = request.Name;
        customer.Email = request.Email;
        customer.Phone = request.Phone;
        customer.UpdatedAt = DateTime.UtcNow;

        await _customerRepository.UpdateAsync(customer);

        return Result<CustomerResponse>.Success(_mapper.Map<CustomerResponse>(customer));
    }

    public async Task<Result<bool>> DeleteAsync(Guid id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null)
            return Result<bool>.Failure("Customer not found or does not exist");

        customer.Status = CustomerStatus.Inactive;
        customer.UpdatedAt = DateTime.UtcNow;

        await _customerRepository.UpdateAsync(customer);

        return Result<bool>.Success(true);

    }
    public async Task<Result<List<CustomerResponse>>> GetByStatusAsync(CustomerStatus status)
    {
        var customers = await _customerRepository.GetByStatusAsync(status);

        var response = _mapper.Map<List<CustomerResponse>>(customers);
        return Result<List<CustomerResponse>>.Success(response);
    }
}
