using AutoMapper;
using CRM.Application.DTOs;
using CRM.Domain.Entities;

namespace CRM.Application.Mapping;

public class LeadProfile : Profile
{
    public LeadProfile()
    {
        CreateMap<CreateLeadRequest, Lead>();
        CreateMap<UpdateLeadRequest, Lead>();
        CreateMap<Lead, LeadResponse>();
    }
}
