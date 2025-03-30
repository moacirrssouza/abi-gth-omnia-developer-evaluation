using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
///  Handler for processing UpdateSaleCommand requests
/// </summary>
public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
{
	private readonly ISaleRepository _saleRepository;
	private readonly IMapper _mapper;

	/// <summary>
	/// Initializes a new instance of UpdateHandler
	/// </summary>
	/// <param name="saleRepository">The sale repository</param>
	/// <param name="mapper">The AutoMapper instance</param>
	/// <param name="validator">The validator for UpdateSaleCommand</param>
	public UpdateSaleHandler(ISaleRepository saleRepository, IMapper mapper)
	{
		_saleRepository = saleRepository;
		_mapper = mapper;
	}

	/// <summary>
	/// Handles the UpdateSaleCommand request
	/// </summary>
	/// <param name="command">The UpdateSale command</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>The updated sale details</returns>
	public async Task<UpdateSaleResult> Handle(UpdateSaleCommand command, CancellationToken cancellationToken)
	{
		var validator = new UpdateSaleCommandValidator();
		var validationResult = await validator.ValidateAsync(command, cancellationToken);
		if (!validationResult.IsValid)
			throw new ValidationException(validationResult.Errors);

		var existingSale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);
		if (existingSale == null)
			throw new InvalidOperationException($"Sale with ID {command.Id} not found");

		existingSale.SaleDate = command.SaleDate;
		existingSale.CustomerId = command.CustomerId;
		existingSale.BranchId = command.BranchId;
		existingSale.IsCancelled = command.IsCancelled;
		
		if (command.SaleItems != null && command.SaleItems.Any())
		{
			existingSale.SaleItems = command.SaleItems;
		}

		existingSale.RecalculateTotal();

		var updatedSale = await _saleRepository.UpdateAsync(existingSale, cancellationToken);
		var result = _mapper.Map<UpdateSaleResult>(updatedSale);
		return result;
	}
}