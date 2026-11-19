using AutoMapper;
using CRM.Application.Common;
using CRM.Application.DTOs;
using CRM.Application.Interfaces;
using CRM.Domain.Entities;
using CRM.Domain.Enums;

namespace CRM.Application.Services;

public class LeadService : ILeadService
{
    private readonly IMapper _mapper;
    private readonly ILeadRepository _leadRepository;
    private readonly ICustomerService _customerService;
    public LeadService(IMapper mapper, ILeadRepository leadRepository, ICustomerService customerService)
    {
        _mapper = mapper;
        _leadRepository = leadRepository;
        _customerService = customerService;
    }

    public async Task<Result<LeadResponse>> ConvertAsync(Guid id, ConvertLeadRequest request, CancellationToken cancellationToken)
    {
        var lead = await _leadRepository.GetByIdAsync(id, cancellationToken);

        if (lead == null)
            return Result<LeadResponse>.Failure("Lead not found or does not exist");

        if (lead.Status == LeadStatus.Lost || lead.Status == LeadStatus.Converted)
            return Result<LeadResponse>.Failure("Lead already lost or already converted");

        var customerRequest = new CreateCustomerRequest
        {
            Name = lead.Name,
            Document = lead.Document,
            Email = lead.Email,
            Phone = lead.Phone,
            Type = request.CustomerType
        };


        var leadConverted = await _customerService.CreateAsync(customerRequest, cancellationToken);
        if (!leadConverted.IsSuccess)
            return Result<LeadResponse>.Failure(leadConverted.Error!);

        lead.Status = LeadStatus.Converted;
        lead.LastUpdatedAt = DateTime.UtcNow;
        lead.CustomerId = leadConverted.Data!.Id;

        await _leadRepository.UpdateAsync(lead, cancellationToken);

        var response = _mapper.Map<LeadResponse>(lead);

        return Result<LeadResponse>.Success(response);
    }

    public async Task<Result<LeadResponse>> CreateAsync(CreateLeadRequest request, CancellationToken cancellationToken)
    {
        if (request.Document == null)
            return Result<LeadResponse>.Failure("The document field is empty or null, please enter one.");
        if (await _leadRepository.ExistsByDocumentAsync(request.Document, cancellationToken))
            return Result<LeadResponse>.Failure("Already exist a Lead with this document.");

        var lead = _mapper.Map<Lead>(request);

        lead.Id = Guid.NewGuid();
        lead.Status = LeadStatus.New;
        lead.CreatedAt = DateTime.UtcNow;

        await _leadRepository.AddAsync(lead, cancellationToken);

        var response = _mapper.Map<LeadResponse>(lead);

        return Result<LeadResponse>.Success(response);
    }

    public async Task<Result<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var lead = await _leadRepository.GetByIdAsync(id, cancellationToken);
        if (lead == null)
            return Result<bool>.Failure("Lead not found or does not exist");

        lead.Status = LeadStatus.Lost;
        lead.LastUpdatedAt = DateTime.UtcNow;

        await _leadRepository.UpdateAsync(lead, cancellationToken);

        return Result<bool>.Success(true);
    }

    public async Task<Result<List<LeadResponse>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var lead = await _leadRepository.GetAllAsync(cancellationToken);
        var response = _mapper.Map<List<LeadResponse>>(lead);
        return Result<List<LeadResponse>>.Success(response);
    }

    public async Task<Result<LeadResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var lead = await _leadRepository.GetByIdAsync(id, cancellationToken);
        if (lead == null)
            return Result<LeadResponse>.Failure("Lead not found or do not exist");

        var response = _mapper.Map<LeadResponse>(lead);

        return Result<LeadResponse>.Success(response);
    }

    public async Task<Result<List<LeadResponse>>> GetByStatusAsync(LeadStatus status, CancellationToken cancellationToken)
    {
        var lead = await _leadRepository.GetByStatusAsync(status, cancellationToken);
        var response = _mapper.Map<List<LeadResponse>>(lead);
        return Result<List<LeadResponse>>.Success(response);
    }

    public async Task<Result<LeadResponse>> UpdateAsync(Guid id, UpdateLeadRequest request, CancellationToken cancellationToken)
    {
        var lead = await _leadRepository.GetByIdAsync(id, cancellationToken);
        if (lead == null)
            return Result<LeadResponse>.Failure("Lead not found or does not exist");

        lead.Name = request.Name;
        lead.Email = request.Email;
        lead.Phone = request.Phone;
        lead.LastUpdatedAt = DateTime.UtcNow;

        await _leadRepository.UpdateAsync(lead, cancellationToken);

        var response = _mapper.Map<LeadResponse>(lead);
        return Result<LeadResponse>.Success(response);
    }
}
