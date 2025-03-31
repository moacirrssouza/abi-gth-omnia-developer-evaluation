using Ambev.DeveloperEvaluation.Application.Common.Events;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Handles the creation of sales by processing the <see cref="CreateSaleCommand"/> command
/// and returning the result encapsulated in a <see cref="CreateSaleResult"/>.
/// </summary>
public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IEventPublisher _eventPublisher;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateSaleHandler"/> class.
    /// </summary>
    /// <param name="saleRepository">The repository responsible for managing sale data.</param>
    /// <param name="mapper">The mapper used for object-object mappings.</param>
    /// <param name="eventPublisher">The event publisher for publishing domain events.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when any of the injected dependencies are null.
    /// </exception>
    public CreateSaleHandler(
        ISaleRepository saleRepository,
        IMapper mapper,
        IEventPublisher eventPublisher)
    {
        _saleRepository = saleRepository ?? throw new ArgumentNullException(nameof(saleRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _eventPublisher = eventPublisher ?? throw new ArgumentNullException(nameof(eventPublisher));
    }

    /// <summary>
    /// Handles the creation of a sale based on the provided command.
    /// </summary>
    /// <param name="command">The command containing the sale details.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation, containing the result of the sale creation.</returns>
    /// <exception cref="ValidationException">
    /// Thrown when the command validation fails.
    /// </exception>
    public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new CreateSaleCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var sale = _mapper.Map<Sale>(command);
        var createdSale = await _saleRepository.CreateAsync(sale, cancellationToken);
        var saleCreatedEvent = new SaleCreatedEvent(createdSale.Id);
        await _eventPublisher.PublishAsync(saleCreatedEvent);
        return _mapper.Map<CreateSaleResult>(createdSale);
    }
}