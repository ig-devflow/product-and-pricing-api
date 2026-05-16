using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductsAndPricingNew.AdminApi.Contracts.Division;
using ProductsAndPricingNew.AdminApi.Extensions;
using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.Division.Commands.CreateDivision;
using ProductsAndPricingNew.Application.Features.Division.Commands.UpdateDivision;
using ProductsAndPricingNew.Application.Features.Division.Models;
using ProductsAndPricingNew.Application.Features.Division.Queries.GetDivisionById;
using ProductsAndPricingNew.Application.Features.Division.Queries.GetDivisions;

namespace ProductsAndPricingNew.AdminApi.Controllers;

/// <summary>
/// Provides endpoints for managing divisions.
/// </summary>
[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
[Route("api/v1/divisions")]
[Produces("application/json")]
public sealed class DivisionController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public DivisionController(ISender sender, IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets a paged list of divisions.
    /// </summary>
    /// <param name="request">Filtering and paging options.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A paged list of divisions.</returns>
    /// <response code="200">Returns the requested page of divisions.</response>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<DivisionListItemDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetList([FromQuery] GetDivisionsRequest request, CancellationToken ct)
    {
        var query = _mapper.Map<GetDivisionsQuery>(request);
        Result<PagedResult<DivisionListItemDto>> result = await _sender.Send(query, ct);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Gets a division by identifier.
    /// </summary>
    /// <param name="id">Division identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Division details.</returns>
    /// <response code="200">Returns the division details.</response>
    /// <response code="404">Division was not found.</response>
    [HttpGet("{id:int}", Name = "GetDivisionById")]
    [ProducesResponseType(typeof(DivisionDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetById(int id, CancellationToken ct)
    {
        Result<DivisionDetailsDto> result = await _sender.Send(new GetDivisionByIdQuery(id), ct);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Creates a division.
    /// </summary>
    /// <param name="request">Division creation payload.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The created division identifier.</returns>
    /// <response code="201">Division was created.</response>
    /// <response code="400">Request validation failed.</response>
    /// <response code="409">Division conflicts with current state, for example duplicate name.</response>
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult> Create([FromBody] CreateDivisionRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<CreateDivisionCommand>(request);
        Result<int> result = await _sender.Send(command, ct);

        return result.ToActionResult(
            this,
            createdId => CreatedAtAction(nameof(GetById), new { id = createdId }, new { id = createdId }));
    }

    /// <summary>
    /// Updates an existing division.
    /// </summary>
    /// <param name="id">Division identifier.</param>
    /// <param name="request">Division update payload.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>No content when update succeeds.</returns>
    /// <response code="204">Division was updated.</response>
    /// <response code="400">Request validation failed.</response>
    /// <response code="404">Division was not found.</response>
    /// <response code="409">Division conflicts with current state, for example duplicate name or concurrency conflict.</response>
    [HttpPut("{id:int}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateDivisionRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<UpdateDivisionCommand>(request) with { Id = id };
        Result<Unit> result = await _sender.Send(command, ct);
        return result.ToActionResult(this);
    }
}