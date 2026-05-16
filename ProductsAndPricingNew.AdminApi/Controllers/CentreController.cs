using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductsAndPricingNew.AdminApi.Contracts.Centre;
using ProductsAndPricingNew.AdminApi.Extensions;
using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.Centre.Commands.CreateCentre;
using ProductsAndPricingNew.Application.Features.Centre.Commands.UpdateCentre;
using ProductsAndPricingNew.Application.Features.Centre.Models;
using ProductsAndPricingNew.Application.Features.Centre.Queries.GetCentreById;
using ProductsAndPricingNew.Application.Features.Centre.Queries.GetCentres;

namespace ProductsAndPricingNew.AdminApi.Controllers;

/// <summary>
/// Provides endpoints for managing centres.
/// </summary>
[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
[Route("api/v1/centres")]
[Produces("application/json")]
public class CentreController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public CentreController(ISender sender, IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets a paged list of centres.
    /// </summary>
    /// <param name="request">Filtering and paging options.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A paged list of centres.</returns>
    /// <response code="200">Returns the requested page of centres.</response>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<CentreListItemDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetList([FromQuery] GetCentresRequest request, CancellationToken ct)
    {
        var query = _mapper.Map<GetCentresQuery>(request);
        Result<PagedResult<CentreListItemDto>> result = await _sender.Send(query, ct);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Gets a centre by identifier.
    /// </summary>
    /// <param name="id">Centre identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Centre details.</returns>
    /// <response code="200">Returns the centre details.</response>
    /// <response code="404">Centre was not found.</response>
    [HttpGet("{id:int}", Name = "GetCentreById")]
    [ProducesResponseType(typeof(CentreDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetById(int id, CancellationToken ct)
    {
        Result<CentreDetailsDto> result = await _sender.Send(new GetCentreByIdQuery(id), ct);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Creates a centre.
    /// </summary>
    /// <param name="request">Centre creation payload.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The created centre identifier.</returns>
    /// <response code="201">Centre was created.</response>
    /// <response code="400">Request validation failed.</response>
    /// <response code="409">Centre conflicts with current state, for example duplicate name.</response>
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult> Create([FromBody] CreateCentreRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<CreateCentreCommand>(request);
        Result<int> result = await _sender.Send(command, ct);

        return result.ToActionResult(
            this,
            createdId => CreatedAtAction(nameof(GetById), new { id = createdId }, new { id = createdId }));
    }

    /// <summary>
    /// Updates an existing centre.
    /// </summary>
    /// <param name="id">Centre identifier.</param>
    /// <param name="request">Centre update payload.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>No content when update succeeds.</returns>
    /// <response code="204">Centre was updated.</response>
    /// <response code="400">Request validation failed.</response>
    /// <response code="404">Centre was not found.</response>
    /// <response code="409">Centre conflicts with current state, for example duplicate name or concurrency conflict.</response>
    [HttpPut("{id:int}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateCentreRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<UpdateCentreCommand>(request) with { Id = id };
        Result<Unit> result = await _sender.Send(command, ct);
        return result.ToActionResult(this);
    }
}