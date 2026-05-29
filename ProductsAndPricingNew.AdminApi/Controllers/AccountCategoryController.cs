using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductsAndPricingNew.AdminApi.Contracts.AccountCategory;
using ProductsAndPricingNew.AdminApi.Extensions;
using ProductsAndPricingNew.Application.Features.AccountCategory.Commands.CreateAccountCategory;
using ProductsAndPricingNew.Application.Features.AccountCategory.Commands.UpdateAccountCategory;
using ProductsAndPricingNew.Application.Features.AccountCategory.Models;
using ProductsAndPricingNew.Application.Features.AccountCategory.Queries.GetAccountCategories;
using ProductsAndPricingNew.Application.Features.AccountCategory.Queries.GetAccountCategoryById;

namespace ProductsAndPricingNew.AdminApi.Controllers;

/// <summary>
/// Provides endpoints for managing account categories.
/// </summary>
[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
[Produces("application/json")]
public class AccountCategoryController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public AccountCategoryController(ISender sender, IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets all account categories that belong to the specified division.
    /// </summary>
    /// <param name="divisionId">Division identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A list of account categories (id and name) for the division.</returns>
    /// <response code="200">Returns account categories for the division.</response>
    [HttpGet("api/v1/divisions/{divisionId:int:min(1)}/account-categories")]
    [ProducesResponseType(typeof(IReadOnlyCollection<AccountCategoryListItemDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetList(int divisionId, CancellationToken ct)
    {
        Result<IReadOnlyCollection<AccountCategoryListItemDto>> result = await _sender.Send(new GetAccountCategoriesQuery(divisionId), ct);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Gets an account category by identifier.
    /// </summary>
    /// <param name="id">Account category identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Account category details.</returns>
    /// <response code="200">Returns the account category details.</response>
    /// <response code="404">Account category was not found.</response>
    [HttpGet("api/v1/account-categories/{id:int:min(1)}", Name = "GetAccountCategoryById")]
    [ProducesResponseType(typeof(AccountCategoryDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetById(int id, CancellationToken ct)
    {
        Result<AccountCategoryDetailsDto> result = await _sender.Send(new GetAccountCategoryByIdQuery(id), ct);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Creates an account category for the specified division.
    /// </summary>
    /// <param name="divisionId">Division identifier.</param>
    /// <param name="request">Account category creation payload.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The created account category identifier.</returns>
    /// <response code="201">Account category was created.</response>
    /// <response code="400">Request validation failed.</response>
    /// <response code="409">Account category conflicts with current state, for example duplicate name.</response>
    [HttpPost("api/v1/divisions/{divisionId:int:min(1)}/account-categories")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult> Create(int divisionId, [FromBody] CreateAccountCategoryRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<CreateAccountCategoryCommand>(request) with { DivisionId = divisionId };
        Result<int> result = await _sender.Send(command, ct);

        return result.ToActionResult(
            this,
            createdId => CreatedAtRoute("GetAccountCategoryById", new { id = createdId }, new { id = createdId }));
    }

    /// <summary>
    /// Updates the name of an existing account category.
    /// </summary>
    /// <param name="id">Account category identifier.</param>
    /// <param name="request">Account category update payload.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>No content when update succeeds.</returns>
    /// <response code="204">Account category was updated.</response>
    /// <response code="400">Request validation failed.</response>
    /// <response code="404">Account category was not found.</response>
    /// <response code="409">Account category conflicts with current state, for example duplicate name or concurrency conflict.</response>
    [HttpPut("api/v1/account-categories/{id:int:min(1)}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateAccountCategoryRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<UpdateAccountCategoryCommand>(request) with { Id = id };
        Result<Unit> result = await _sender.Send(command, ct);
        return result.ToActionResult(this);
    }
}
