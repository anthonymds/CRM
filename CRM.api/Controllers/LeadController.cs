using CRM.Application.DTOs;
using CRM.Application.Interfaces;
using CRM.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CRM.api.Controllers;
/// <summary>
///
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class LeadController : ControllerBase
{

    private readonly ILeadService _service;

    /// <summary>
    ///
    /// </summary>
    /// <param name="service"></param>
    public LeadController(ILeadService service)
    {
        _service = service;
    }

    ///<summary>
    ///Create Lead
    /// </summary>
    ///<param name="request">
    ///Lead information: name,document,type, email, phone
    ///</param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    /// Created Lead.
    /// </returns>
    /// <response code="200">
    /// Lead created.
    /// </response>
    /// <response code="400">
    /// Invalid Data.
    /// </response>
    [HttpPost]
    [ProducesResponseType(typeof(LeadResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CreateLeadRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.CreateAsync(request, cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(new { message = result.Error });

        return Ok(result.Data);
    }

    /// <summary>
    /// Returns all Leads.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<LeadResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _service.GetAllAsync(cancellationToken);
        return Ok(result.Data);
    }
    /// <summary>
    /// Returns Lead with the id.
    /// </summary>
    /// <param name="id">Id of the Lead</param>
    /// <param name="cancellationToken"></param>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(LeadResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(new { message = result.Error });

        return Ok(result.Data);
    }

    /// <summary>
    /// Updates the Lead data with the respective Id
    /// </summary>
    /// <param name="id">Id of the Lead</param>
    /// <param name="request">Data to be updated: name, phone, email</param>
    /// <param name="cancellationToken"></param>
    [ProducesResponseType(typeof(LeadResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateLeadRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.UpdateAsync(id, request, cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(new { message = result.Error });

        return Ok(result.Data);
    }

    /// <summary>
    /// Turns a Lead status to inactive.
    /// </summary>
    /// <param name="id">Lead's Id.</param>
    /// <param name="cancellationToken"></param>
    /// <response code="204">Lead successfully deactivated.</response>
    /// <response code="400">Lead not found or does not exist</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.DeleteAsync(id, cancellationToken);
        if (!result.IsSuccess)
        {
            return BadRequest(new
            {
                message = result.Error
            });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Returns Leads according to the reported status.
    /// </summary>
    /// <param name="status">Status of Leads who will be returned.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>List of clients with the specified status.</returns>
    [HttpGet("status/{status}")]
    public async Task<IActionResult> GetByStatus(LeadStatus status, CancellationToken cancellationToken)
    {
        var result = await _service.GetByStatusAsync(status, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Converts the lead to customer and updates his status to "converted".
    /// </summary>
    /// <param name="id">Id of Lead who will be converted to customer.</param>
    /// <param name="request">the type of customer : individual or company </param>
    /// <param name="cancellationToken"></param>
    /// <returns>List of clients with the specified status.</returns>
    [HttpPost("{id}/convert")]
    public async Task<IActionResult> ConvertLeadToCustomer(Guid id, [FromBody] ConvertLeadRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.ConvertAsync(id, request, cancellationToken);
        return Ok(result);
    }
}
