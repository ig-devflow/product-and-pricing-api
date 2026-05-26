using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductsAndPricingNew.AdminApi.Contracts.AccommodationRoom;
using ProductsAndPricingNew.AdminApi.Extensions;
using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.AccommodationRoom.Commands.CreateAccommodationRoom;
using ProductsAndPricingNew.Application.Features.AccommodationRoom.Commands.UpdateAccommodationRoom;
using ProductsAndPricingNew.Application.Features.AccommodationRoom.Models;
using ProductsAndPricingNew.Application.Features.AccommodationRoom.Queries.GetAccommodationRoomById;
using ProductsAndPricingNew.Application.Features.AccommodationRoom.Queries.GetAccommodationRooms;

namespace ProductsAndPricingNew.AdminApi.Controllers;

/// <summary>
/// Provides endpoints for managing accommodation rooms.
/// </summary>
[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
[Produces("application/json")]
public class AccommodationRoomController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public AccommodationRoomController(ISender sender, IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets a paged list of accommodation rooms.
    /// </summary>
    /// <param name="divisionId">Division identifier.</param>
    /// <param name="accommodationId">Accommodation identifier.</param>
    /// <param name="request">Filtering and paging options.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A paged list of accommodation rooms.</returns>
    /// <response code="200">Returns the requested page of accommodation rooms.</response>
    [HttpGet("/api/v1/divisions/{divisionId:int:min(1)}/accommodations/{accommodationId:int:min(1)}/rooms")]
    [ProducesResponseType(typeof(PagedResult<AccommodationRoomListItemDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetList(int divisionId, int accommodationId, [FromQuery] GetAccommodationRoomsRequest request, CancellationToken ct)
    {
        var query = _mapper.Map<GetAccommodationRoomsQuery>(request) with { DivisionId = divisionId, AccommodationId = accommodationId };
        Result<PagedResult<AccommodationRoomListItemDto>> result = await _sender.Send(query, ct);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Gets accommodation room by identifier.
    /// </summary>
    /// <param name="id">Accommodation room identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Accommodation room details.</returns>
    /// <response code="200">Returns the accommodation room details.</response>
    /// <response code="404">Accommodation room was not found.</response>
    [HttpGet("/api/v1/accommodation-rooms/{id:int:min(1)}", Name = "GetAccommodationRoomById")]
    [ProducesResponseType(typeof(AccommodationRoomDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetById(int id, CancellationToken ct)
    {
        Result<AccommodationRoomDetailsDto> result = await _sender.Send(new GetAccommodationRoomByIdQuery(id), ct);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Creates accommodation room.
    /// </summary>
    /// <param name="divisionId">Division identifier.</param>
    /// <param name="accommodationId">Accommodation identifier.</param>
    /// <param name="request">Accommodation room creation payload.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The created accommodation room identifier.</returns>
    /// <response code="201">Accommodation room was created.</response>
    /// <response code="400">Request validation failed.</response>
    /// <response code="409">Accommodation room conflicts with current state, for example duplicate name.</response>
    [HttpPost("/api/v1/divisions/{divisionId:int:min(1)}/accommodations/{accommodationId:int:min(1)}/rooms")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult> Create(int divisionId, int accommodationId, [FromBody] CreateAccommodationRoomRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<CreateAccommodationRoomCommand>(request) with { DivisionId = divisionId, AccommodationId = accommodationId }; ;
        Result<int> result = await _sender.Send(command, ct);

        return result.ToActionResult(
            this,
            createdId => CreatedAtRoute("GetAccommodationRoomById", new { id = createdId }, new { id = createdId }));
    }

    /// <summary>
    /// Updates an existing accommodation room.
    /// </summary>
    /// <param name="id">Accommodation room identifier.</param>
    /// <param name="request">Accommodation room update payload.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>No content when update succeeds.</returns>
    /// <response code="204">Accommodation room was updated.</response>
    /// <response code="400">Request validation failed.</response>
    /// <response code="404">Accommodation room was not found.</response>
    /// <response code="409">Accommodation room conflicts with current state, for example duplicate name or concurrency conflict.</response>
    [HttpPut("/api/v1/accommodation-rooms/{id:int:min(1)}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateAccommodationRoomRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<UpdateAccommodationRoomCommand>(request) with { Id = id };
        Result<Unit> result = await _sender.Send(command, ct);
        return result.ToActionResult(this);
    }
}