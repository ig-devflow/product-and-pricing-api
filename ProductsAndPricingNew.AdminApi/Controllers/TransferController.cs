using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductsAndPricingNew.AdminApi.Contracts.Transfer;
using ProductsAndPricingNew.AdminApi.Extensions;
using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.Transfer.Commands.CreateTransfer;
using ProductsAndPricingNew.Application.Features.Transfer.Commands.UpdateTransfer;
using ProductsAndPricingNew.Application.Features.Transfer.Models;
using ProductsAndPricingNew.Application.Features.Transfer.Queries.GetTransferById;
using ProductsAndPricingNew.Application.Features.Transfer.Queries.GetTransfers;

namespace ProductsAndPricingNew.AdminApi.Controllers;

/// <summary>
/// Provides endpoints for managing transfers.
/// </summary>
[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
[Produces("application/json")]
public class TransferController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public TransferController(ISender sender, IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets a paged list of transfers.
    /// </summary>
    /// <param name="divisionId">Division identifier.</param>
    /// <param name="request">Filtering and paging options.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A paged list of transfers.</returns>
    /// <response code="200">Returns the requested page of transfers.</response>
    [HttpGet("/api/v1/divisions/{divisionId:int:min(1)}/transfers")]
    [ProducesResponseType(typeof(PagedResult<TransferListItemDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetList(int divisionId, [FromQuery] GetTransfersRequest request, CancellationToken ct)
    {
        GetTransfersQuery query = _mapper.Map<GetTransfersQuery>(request) with { DivisionId = divisionId };
        Result<PagedResult<TransferListItemDto>> result = await _sender.Send(query, ct);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Gets transfer by identifier.
    /// </summary>
    /// <param name="id">Transfer identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Transfer details.</returns>
    /// <response code="200">Returns the transfer details.</response>
    /// <response code="404">Transfer was not found.</response>
    [HttpGet("/api/v1/transfers/{id:int:min(1)}", Name = "GetTransferById")]
    [ProducesResponseType(typeof(TransferDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetById(int id, CancellationToken ct)
    {
        Result<TransferDetailsDto> result = await _sender.Send(new GetTransferByIdQuery(id), ct);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Creates a transfer.
    /// </summary>
    /// <param name="divisionId">Division identifier.</param>
    /// <param name="request">Transfer creation payload.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The created transfer identifier.</returns>
    /// <response code="201">Transfer was created.</response>
    /// <response code="400">Request validation failed.</response>
    /// <response code="409">Transfer conflicts with current state, for example duplicate name.</response>
    [HttpPost("/api/v1/divisions/{divisionId:int:min(1)}/transfers")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult> Create(int divisionId, [FromBody] CreateTransferRequest request, CancellationToken ct)
    {
        CreateTransferCommand command = _mapper.Map<CreateTransferCommand>(request) with { DivisionId = divisionId };
        Result<int> result = await _sender.Send(command, ct);

        return result.ToActionResult(
            this,
            createdId => CreatedAtRoute("GetTransferById", new { id = createdId }, new { id = createdId }));
    }

    /// <summary>
    /// Updates an existing transfer.
    /// </summary>
    /// <param name="id">Transfer identifier.</param>
    /// <param name="request">Transfer update payload.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>No content when update succeeds.</returns>
    /// <response code="204">Transfer was updated.</response>
    /// <response code="400">Request validation failed.</response>
    /// <response code="404">Transfer was not found.</response>
    /// <response code="409">Transfer conflicts with current state, for example duplicate name or concurrency conflict.</response>
    [HttpPut("/api/v1/transfers/{id:int:min(1)}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateTransferRequest request, CancellationToken ct)
    {
        UpdateTransferCommand command = _mapper.Map<UpdateTransferCommand>(request) with { Id = id };
        Result<Unit> result = await _sender.Send(command, ct);
        return result.ToActionResult(this);
    }
}
