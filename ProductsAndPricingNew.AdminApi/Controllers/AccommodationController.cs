using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductsAndPricingNew.AdminApi.Contracts.Accommodation;
using ProductsAndPricingNew.AdminApi.Extensions;
using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.Accommodation.Commands.CreateAccommodation;
using ProductsAndPricingNew.Application.Features.Accommodation.Commands.UpdateAccommodation;
using ProductsAndPricingNew.Application.Features.Accommodation.Models;
using ProductsAndPricingNew.Application.Features.Accommodation.Queries.GetAccommodationById;
using ProductsAndPricingNew.Application.Features.Accommodation.Queries.GetAccommodations;

namespace ProductsAndPricingNew.AdminApi.Controllers;

/// <summary>
/// Provides endpoints for managing accommodations.
/// </summary>
[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
[Route("api/v1/accommodations")]
[Produces("application/json")]
public class AccommodationController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public AccommodationController(ISender sender, IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets a paged list of accommodations.
    /// </summary>
    /// <param name="request">Filtering and paging options.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A paged list of accommodations.</returns>
    /// <response code="200">Returns the requested page of accommodations.</response>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<AccommodationListItemDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetList([FromQuery] GetAccommodationsRequest request, CancellationToken ct)
    {
        var query = _mapper.Map<GetAccommodationsQuery>(request);
        Result<PagedResult<AccommodationListItemDto>> result = await _sender.Send(query, ct);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Gets accommodation by identifier.
    /// </summary>
    /// <param name="id">Accommodation identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Accommodation details.</returns>
    /// <response code="200">Returns the accommodation details.</response>
    /// <response code="404">Accommodation was not found.</response>
    [HttpGet("{id:int:min(1)}", Name = "GetAccommodationById")]
    [ProducesResponseType(typeof(AccommodationDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetById(int id, CancellationToken ct)
    {
        Result<AccommodationDetailsDto> result = await _sender.Send(new GetAccommodationByIdQuery(id), ct);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Creates accommodation.
    /// </summary>
    /// <param name="request">Accommodation creation payload.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The created accommodation identifier.</returns>
    /// <response code="201">Accommodation was created.</response>
    /// <response code="400">Request validation failed.</response>
    /// <response code="409">Accommodation conflicts with current state, for example duplicate name.</response>
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult> Create([FromBody] CreateAccommodationRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<CreateAccommodationCommand>(request);
        Result<int> result = await _sender.Send(command, ct);

        return result.ToActionResult(
            this,
            createdId => CreatedAtRoute("GetAccommodationById", new { id = createdId }, new { id = createdId }));
    }

    /// <summary>
    /// Updates an existing accommodation.
    /// </summary>
    /// <param name="id">Accommodation identifier.</param>
    /// <param name="request">Accommodation update payload.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>No content when update succeeds.</returns>
    /// <response code="204">Accommodation was updated.</response>
    /// <response code="400">Request validation failed.</response>
    /// <response code="404">Accommodation was not found.</response>
    /// <response code="409">Accommodation conflicts with current state, for example duplicate name or concurrency conflict.</response>
    [HttpPut("{id:int:min(1)}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateAccommodationRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<UpdateAccommodationCommand>(request) with { Id = id };
        Result<Unit> result = await _sender.Send(command, ct);
        return result.ToActionResult(this);
    }
}