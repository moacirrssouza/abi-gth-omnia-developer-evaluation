using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// 
/// </summary>
public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
	private readonly ISaleRepository _saleRepository;
	private readonly IMapper _mapper;
	private readonly ILogger<CreateSaleHandler> _logger;

	public CreateSaleHandler(
		ISaleRepository saleRepository,
		IMapper mapper,
		ILogger<CreateSaleHandler> logger)
	{
		_saleRepository = saleRepository ?? throw new ArgumentNullException(nameof(saleRepository));
		_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="command"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	/// <exception cref="ValidationException"></exception>
	public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
	{
		var validator = new CreateSaleCommandValidator();
		var validationResult = await validator.ValidateAsync(command, cancellationToken);
		if (!validationResult.IsValid)
		{
			throw new ValidationException(validationResult.Errors);
		}

		var sale = _mapper.Map<Sale>(command);
		var createdSale = await _saleRepository.CreateAsync(sale, cancellationToken);
		
		return _mapper.Map<CreateSaleResult>(createdSale);
	}
}