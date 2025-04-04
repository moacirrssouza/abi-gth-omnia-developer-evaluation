﻿using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.ListSales;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales;

/// <summary>
/// Controller for managing sale operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
	private readonly IMediator _mediator;
	private readonly IMapper _mapper;
	/// <summary>
	/// Initializes a new instance of SalesController
	/// </summary>
	/// <param name="mediator">The mediator instance</param>
	/// <param name="mapper">The AutoMapper instance</param>
	public SalesController(IMediator mediator, IMapper mapper)
	{
		_mediator = mediator;
		_mapper = mapper;
	}
	/// <summary>
	/// Creates a new sale
	/// </summary>
	/// <param name="request">The sale creation request</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>The created sale details</returns>
	[HttpPost]
	[ProducesResponseType(typeof(ApiResponseWithData<CreateSaleResponse>), StatusCodes.Status201Created)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> CreateSale([FromBody] CreateSaleRequest request, CancellationToken cancellationToken)
	{
		var validator = new CreateSaleRequestValidator();
		var validationResult = await validator.ValidateAsync(request, cancellationToken);

		if (!validationResult.IsValid)
			return BadRequest(validationResult.Errors);
		var command = _mapper.Map<CreateSaleCommand>(request);
		var response = await _mediator.Send(command, cancellationToken);

		return Created(string.Empty, new ApiResponseWithData<CreateSaleResponse>
		{
			Success = true,
			Message = "Sale created successfully",
			Data = _mapper.Map<CreateSaleResponse>(response)
		});
	}

	/// <summary>
	/// Retrieves a list of sale
	/// </summary>
	/// <param name="cancellationToken"></param>
	/// <param name="pageSize"></param>
	/// <param name="pageNumber"></param>
	/// <returns></returns>
	[HttpGet]
	[ProducesResponseType(typeof(PaginatedResponse<ListSalesResponse>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetSales(CancellationToken cancellationToken,
		[FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
	{
		var command = new ListSalesCommand
		{
			PageNumber = pageNumber,
			PageSize = pageSize
		};

		var paginatedSales = await _mediator.Send(command, cancellationToken);
		var response = new PaginatedResponse<ListSalesResponse>
		{
			Success = true,
			Message = paginatedSales.TotalCount > 0 ? "Sales retrieved successfully" : "No sales available",
			Data = _mapper.Map<IEnumerable<ListSalesResponse>>(paginatedSales),
			CurrentPage = paginatedSales.CurrentPage,
			TotalPages = paginatedSales.TotalPages,
			TotalCount = paginatedSales.TotalCount
		};

		return Ok(response);
	}

	/// <summary>
	/// Retrieves a sale by their ID
	/// </summary>
	/// <param name="id">The unique identifier of the sale</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>The sale details if found</returns>
	[HttpGet("{id}")]
	[ProducesResponseType(typeof(ApiResponseWithData<GetSaleResponse>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetSaleById([FromRoute] Guid id, CancellationToken cancellationToken)
	{
		var request = new GetSaleRequest { Id = id };
		var validator = new GetSaleRequestValidator();
		var validationResult = await validator.ValidateAsync(request, cancellationToken);

		if (!validationResult.IsValid)
			return BadRequest(validationResult.Errors);

		var command = _mapper.Map<GetSaleCommand>(request.Id);
		var response = await _mediator.Send(command, cancellationToken);

		return Ok(new ApiResponseWithData<GetSaleResponse>
		{
			Success = true,
			Message = "Sale retrieved successfully",
			Data = _mapper.Map<GetSaleResponse>(response)
		});
	}

	/// <summary>
	/// Deletes a sale by their ID
	/// </summary>
	/// <param name="id">The unique identifier of the sale to delete</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Success response if the sale was deleted</returns>
	[HttpDelete("{id}")]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
	public async Task<IActionResult> DeleteSale([FromRoute] Guid id, CancellationToken cancellationToken)
	{
		var request = new DeleteSaleRequest { Id = id };
		var validator = new DeleteSaleRequestValidator();
		var validationResult = await validator.ValidateAsync(request, cancellationToken);

		if (!validationResult.IsValid)
			return BadRequest(validationResult.Errors);

		var command = _mapper.Map<DeleteSaleCommand>(request);
		await _mediator.Send(command, cancellationToken);

		return Ok(new ApiResponse
		{
			Success = true,
			Message = "Sale deleted successfully"
		});
	}

	/// <summary>
	/// Updates an existing sale
	/// </summary>
	/// <param name="request">The sale update request</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>The updated sale details</returns>
	[HttpPut("{id}")]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
	public async Task<IActionResult> UpdateSale(Guid id, [FromBody] UpdateSaleRequest request, CancellationToken cancellationToken)
	{
		var validator = new UpdateSaleRequestValidator();
		var validationResult = await validator.ValidateAsync(request, cancellationToken);

		if (!validationResult.IsValid)
			return BadRequest(validationResult.Errors);

		var command = _mapper.Map<UpdateSaleCommand>(request);
		command.Id = id;
		var response = await _mediator.Send(command, cancellationToken);

		return Ok(new ApiResponse
		{
			Success = true,
			Message = "Sale updated successfully"
		});
	}
	
	/// <summary>
	/// Cancels a sale by its ID.
	/// </summary>
	/// <param name="id">The ID of the sale.</param>
	/// <param name="cancellationToken">Cancellation token.</param>
	/// <returns>Response indicating the success of the cancellation.</returns>

	[HttpPost("{id}/cancel")]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
	public async Task<IActionResult> CancelSale(Guid id, CancellationToken cancellationToken)
	{
		var command = new CancelSaleCommand { SaleId = id };
		var result = await _mediator.Send(command, cancellationToken);

		if (!result.Success)
			return NotFound(result);

		return Ok(result);
	}
}