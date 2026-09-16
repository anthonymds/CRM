using AutoMapper;
using CRM.Application.DTOs;
using CRM.Domain.Entities;

namespace CRM.Application.Mappings;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<CreateCustomerRequest, Customer>();
        CreateMap<Customer, CustomerResponse>();
    }
}