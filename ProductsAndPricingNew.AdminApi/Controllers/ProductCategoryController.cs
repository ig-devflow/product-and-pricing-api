using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductsAndPricingNew.AdminApi.Contracts.ProductCategory;
using ProductsAndPricingNew.AdminApi.Extensions;
using ProductsAndPricingNew.Application.Features.ProductCategory.Commands.CreateProductCategory;
using ProductsAndPricingNew.Application.Features.ProductCategory.Commands.UpdateProductCategory;
using ProductsAndPricingNew.Application.Features.ProductCategory.Models;
using ProductsAndPricingNew.Application.Features.ProductCategory.Queries.GetProductCategories;
using ProductsAndPricingNew.Application.Features.ProductCategory.Queries.GetProductCategoryById;

namespace ProductsAndPricingNew.AdminApi.Controllers;

/// <summary>
/// Provides endpoints for managing product categories.
/// </summary>
[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
[Produces("application/json")]
public class ProductCategoryController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public ProductCategoryController(ISender sender, IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets all product categories that belong to the specified division.
    /// </summary>
    /// <param name="divisionId">Division identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A list of product categories (id and name) for the division.</returns>
    /// <response code="200">Returns product categories for the division.</response>
    [HttpGet("api/v1/divisions/{divisionId:int:min(1)}/product-categories")]
    [ProducesResponseType(typeof(IReadOnlyCollection<ProductCategoryListItemDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetList(int divisionId, CancellationToken ct)
    {
        Result<IReadOnlyCollection<ProductCategoryListItemDto>> result = await _sender.Send(new GetProductCategoriesQuery(divisionId), ct);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Gets a product category by identifier.
    /// </summary>
    /// <param name="id">Product category identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Product category details.</returns>
    /// <response code="200">Returns the product category details.</response>
    /// <response code="404">Product category was not found.</response>
    [HttpGet("api/v1/product-categories/{id:int:min(1)}", Name = "GetProductCategoryById")]
    [ProducesResponseType(typeof(ProductCategoryDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetById(int id, CancellationToken ct)
    {
        Result<ProductCategoryDetailsDto> result = await _sender.Send(new GetProductCategoryByIdQuery(id), ct);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Creates a product category for the specified division.
    /// </summary>
    /// <param name="divisionId">Division identifier.</param>
    /// <param name="request">Product category creation payload.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The created product category identifier.</returns>
    /// <response code="201">Product category was created.</response>
    /// <response code="400">Request validation failed.</response>
    /// <response code="409">Product category conflicts with current state, for example duplicate name.</response>
    [HttpPost("api/v1/divisions/{divisionId:int:min(1)}/product-categories")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult> Create(int divisionId, [FromBody] CreateProductCategoryRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<CreateProductCategoryCommand>(request) with { DivisionId = divisionId };
        Result<int> result = await _sender.Send(command, ct);

        return result.ToActionResult(
            this,
            createdId => CreatedAtRoute("GetProductCategoryById", new { id = createdId }, new { id = createdId }));
    }

    /// <summary>
    /// Updates the name of an existing product category.
    /// </summary>
    /// <param name="id">Product category identifier.</param>
    /// <param name="request">Product category update payload.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>No content when update succeeds.</returns>
    /// <response code="204">Product category was updated.</response>
    /// <response code="400">Request validation failed.</response>
    /// <response code="404">Product category was not found.</response>
    /// <response code="409">Product category conflicts with current state, for example duplicate name or concurrency conflict.</response>
    [HttpPut("api/v1/product-categories/{id:int:min(1)}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateProductCategoryRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<UpdateProductCategoryCommand>(request) with { Id = id };
        Result<Unit> result = await _sender.Send(command, ct);
        return result.ToActionResult(this);
    }
}
