using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSales;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales;

/// <summary>
/// API controller responsible for managing sales.
/// Provides endpoints for creating, retrieving, updating and deleting sales.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class SalesController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="SalesController"/> class.
    /// </summary>
    /// <param name="mediator">Mediator used to dispatch commands and queries.</param>
    /// <param name="mapper">Mapper used to convert between requests, commands and responses.</param>
    public SalesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Creates a new sale.
    /// </summary>
    /// <param name="request">Sale creation request payload.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The created sale.</returns>
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponseWithData<CreateSaleResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSale(
        [FromBody] CreateSaleRequest request,
        CancellationToken cancellationToken)
    {
        var validator = new CreateSaleRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var command = _mapper.Map<CreateSaleCommand>(request);
        var result = await _mediator.Send(command, cancellationToken);

        if (!result.Success)
        {
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "Failed to create sale",
                Errors = result.Errors.Select(e =>
                    new ValidationErrorDetail { Error = string.Empty, Detail = e })
            });
        }

        var response = _mapper.Map<CreateSaleResponse>(result);

        return Created(string.Empty, new ApiResponseWithData<CreateSaleResponse>
        {
            Success = true,
            Message = "Sale created successfully",
            Data = response
        });
    }

    /// <summary>
    /// Retrieves a sale by its identifier.
    /// </summary>
    /// <param name="id">Sale identifier.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The requested sale.</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponseWithData<GetSaleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSale(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var request = new GetSaleRequest { Id = id };
        var validator = new GetSaleRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var command = _mapper.Map<GetSaleCommand>(id);
        var result = await _mediator.Send(command, cancellationToken);

        if (!result.Success && !result.Errors.Any())
        {
            return NotFound(new ApiResponse
            {
                Success = false,
                Message = "Sale not found"
            });
        }

        if (!result.Success)
        {
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "Failed to retrieve sale",
                Errors = result.Errors.Select(e =>
                    new ValidationErrorDetail { Error = string.Empty, Detail = e })
            });
        }

        var response = _mapper.Map<GetSaleResponse>(result);

        return Ok(new ApiResponseWithData<GetSaleResponse>
        {
            Success = true,
            Message = "Sale retrieved successfully",
            Data = response
        });
    }

    /// <summary>
    /// Retrieves a paginated list of sales.
    /// </summary>
    /// <param name="page">Page number.</param>
    /// <param name="size">Number of items per page.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A paginated list of sales.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponse<GetSaleResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSales(
        [FromQuery(Name = "_page")] int page = 1,
        [FromQuery(Name = "_size")] int size = 10,
        CancellationToken cancellationToken = default)
    {
        int skip = (page - 1) * size;
        int take = size;

        var command = new GetSalesCommand(skip, take);
        var result = await _mediator.Send(command, cancellationToken);

        if (!result.Success)
        {
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "Failed to retrieve sales",
                Errors = result.Errors.Select(e =>
                    new ValidationErrorDetail { Error = string.Empty, Detail = e })
            });
        }

        var sales = result.Sales ?? Enumerable.Empty<GetSaleCommandResult>();
        var responses = _mapper.Map<IEnumerable<GetSaleResponse>>(sales).ToList();

        var paginatedResponse = new PaginatedResponse<GetSaleResponse>
        {
            Success = true,
            Message = "Sales retrieved successfully",
            Data = responses,
            CurrentPage = page,
            TotalPages = responses.Count == 0
                ? 0
                : (int)Math.Ceiling((double)(skip + responses.Count) / size),
            TotalCount = skip + responses.Count
        };

        return Ok(paginatedResponse);
    }

    /// <summary>
    /// Updates an existing sale.
    /// </summary>
    /// <param name="id">Sale identifier.</param>
    /// <param name="request">Sale update request payload.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The updated sale.</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponseWithData<UpdateSaleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSale(
        [FromRoute] Guid id,
        [FromBody] UpdateSaleRequest request,
        CancellationToken cancellationToken)
    {
        var validator = new UpdateSaleRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var command = _mapper.Map<UpdateSaleCommand>(request);
        command.Id = id;

        var result = await _mediator.Send(command, cancellationToken);

        if (!result.Success && result.Errors.Contains("Resource Not Found"))
        {
            return NotFound(new ApiResponse
            {
                Success = false,
                Message = "Sale not found"
            });
        }

        if (!result.Success)
        {
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "Failed to update sale",
                Errors = result.Errors.Select(e =>
                    new ValidationErrorDetail { Error = string.Empty, Detail = e })
            });
        }

        var response = _mapper.Map<UpdateSaleResponse>(result);

        return Ok(new ApiResponseWithData<UpdateSaleResponse>
        {
            Success = true,
            Message = "Sale updated successfully",
            Data = response
        });
    }

    /// <summary>
    /// Deletes a sale by its identifier.
    /// </summary>
    /// <param name="id">Sale identifier.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>Operation result.</returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteSale(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteSaleCommand(id);
        var result = await _mediator.Send(command, cancellationToken);

        if (!result.Success)
        {
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "Failed to delete sale",
                Errors = result.Errors.Select(e =>
                    new ValidationErrorDetail { Error = string.Empty, Detail = e })
            });
        }

        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Sale deleted successfully"
        });
    }
}