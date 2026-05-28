using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductsAndPricingNew.AdminApi.Contracts.AddOn;
using ProductsAndPricingNew.AdminApi.Extensions;
using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.AddOn.Commands.CreateAddOn;
using ProductsAndPricingNew.Application.Features.AddOn.Commands.UpdateAddOn;
using ProductsAndPricingNew.Application.Features.AddOn.Models;
using ProductsAndPricingNew.Application.Features.AddOn.Queries.GetAddOnById;
using ProductsAndPricingNew.Application.Features.AddOn.Queries.GetAddOns;

namespace ProductsAndPricingNew.AdminApi.Controllers;

/// <summary>
/// Provides endpoints for managing add-ons.
/// </summary>
[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
[Produces("application/json")]
public class AddOnController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public AddOnController(ISender sender, IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets a paged list of add-ons.
    /// </summary>
    /// <param name="divisionId">Division identifier.</param>
    /// <param name="request">Filtering and paging options.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A paged list of add-ons.</returns>
    /// <response code="200">Returns the requested page of add-ons.</response>
    [HttpGet("/api/v1/divisions/{divisionId:int:min(1)}/add-ons")]
    [ProducesResponseType(typeof(PagedResult<AddOnListItemDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetList(int divisionId, [FromQuery] GetAddOnsRequest request, CancellationToken ct)
    {
        GetAddOnsQuery query = _mapper.Map<GetAddOnsQuery>(request) with { DivisionId = divisionId };
        Result<PagedResult<AddOnListItemDto>> result = await _sender.Send(query, ct);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Gets add-on by identifier.
    /// </summary>
    /// <param name="id">Add-on identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Add-on details.</returns>
    /// <response code="200">Returns the add-on details.</response>
    /// <response code="404">Add-on was not found.</response>
    [HttpGet("/api/v1/add-ons/{id:int:min(1)}", Name = "GetAddOnById")]
    [ProducesResponseType(typeof(AddOnDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetById(int id, CancellationToken ct)
    {
        Result<AddOnDetailsDto> result = await _sender.Send(new GetAddOnByIdQuery(id), ct);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Creates an add-on.
    /// </summary>
    /// <param name="divisionId">Division identifier.</param>
    /// <param name="request">Add-on creation payload.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The created add-on identifier.</returns>
    /// <response code="201">Add-on was created.</response>
    /// <response code="400">Request validation failed.</response>
    /// <response code="409">Add-on conflicts with current state, for example duplicate name.</response>
    [HttpPost("/api/v1/divisions/{divisionId:int:min(1)}/add-ons")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult> Create(int divisionId, [FromBody] CreateAddOnRequest request, CancellationToken ct)
    {
        CreateAddOnCommand command = _mapper.Map<CreateAddOnCommand>(request) with { DivisionId = divisionId };
        Result<int> result = await _sender.Send(command, ct);

        return result.ToActionResult(
            this,
            createdId => CreatedAtRoute("GetAddOnById", new { id = createdId }, new { id = createdId }));
    }

    /// <summary>
    /// Updates an existing add-on.
    /// </summary>
    /// <param name="id">Add-on identifier.</param>
    /// <param name="request">Add-on update payload.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>No content when update succeeds.</returns>
    /// <response code="204">Add-on was updated.</response>
    /// <response code="400">Request validation failed.</response>
    /// <response code="404">Add-on was not found.</response>
    /// <response code="409">Add-on conflicts with current state, for example duplicate name or concurrency conflict.</response>
    [HttpPut("/api/v1/add-ons/{id:int:min(1)}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateAddOnRequest request, CancellationToken ct)
    {
        UpdateAddOnCommand command = _mapper.Map<UpdateAddOnCommand>(request) with { Id = id };
        Result<Unit> result = await _sender.Send(command, ct);
        return result.ToActionResult(this);
    }
}
