using CRM.Application.DTOs;
using CRM.Application.Interfaces;
using CRM.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Api.Controllers;

/// <summary>
///
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _service;
    /// <summary>
    ///
    /// </summary>
    /// <param name="service"></param>
    public CustomersController(ICustomerService service)
    {
        _service = service;
    }

    ///<summary>
    ///Create Customer
    /// </summary>
    ///<param name="request">
    ///customer information: name,document,type, email, phone
    ///</param>
    /// <returns>
    /// Created customer.
    /// </returns>
    /// <response code="200">
    /// Customer created.
    /// </response>
    /// <response code="400">
    /// Invalid Data.
    /// </response>
    [HttpPost]
    [ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CreateCustomerRequest request)
    {
        var result = await _service.CreateAsync(request);

        if (!result.IsSuccess)
            return BadRequest(new { message = result.Error });

        return Ok(result.Data);
    }

    /// <summary>
    /// Returns all customers.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<CustomerResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result.Data);
    }
    /// <summary>
    /// Returns customer with the id.
    /// </summary>
    /// <param name="id">Id of the costumer</param>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _service.GetByIdAsync(id);
        if (!result.IsSuccess)
            return BadRequest(new { message = result.Error });

        return Ok(result.Data);
    }

    /// <summary>
    /// Updates the customer data with the respective Id
    /// </summary>
    /// <param name="id">Id of the costumer</param>
    /// <param name="request">Data to be updated: name, phone, email</param>
    [ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateCustomerRequest request)
    {
        var result = await _service.UpdateAsync(id, request);
        if (!result.IsSuccess)
            return BadRequest(new { message = result.Error });

        return Ok(result.Data);
    }

    /// <summary>
    /// Turns a customer status to inactive.
    /// </summary>
    /// <param name="id">Costumer's Id.</param>
    /// <response code="204">Customer successfully deactivated.</response>
    /// <response code="400">Customer not found or does not exist</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _service.DeleteAsync(id);
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
    /// Returns customers according to the reported status.
    /// </summary>
    /// <param name="status">Status of customers who will be returned.</param>
    /// <returns>List of clients with the specified status.</returns>
    [HttpGet("status/{status}")]
    public async Task<IActionResult> GetByStatus(CustomerStatus status)
    {
        var result = await _service.GetByStatusAsync(status);
        return Ok(result);
    }
}