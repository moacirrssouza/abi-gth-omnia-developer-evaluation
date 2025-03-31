using Ambev.DeveloperEvaluation.Application.Common.Events;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;

/// <summary>
/// Handles the deletion of sales by processing the <see cref="DeleteSaleCommand"/> command
/// and returning the result encapsulated in a <see cref="DeleteSaleResponse"/>.
/// </summary>
public class DeleteSaleHandler : IRequestHandler<DeleteSaleCommand, DeleteSaleResponse>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IEventPublisher _eventPublisher;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteSaleHandler"/> class.
    /// </summary>
    /// <param name="saleRepository">The repository responsible for managing sale data.</param>
    /// <param name="eventPublisher">The event publisher for publishing domain events.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when any of the injected dependencies are null.
    /// </exception>
    public DeleteSaleHandler(ISaleRepository saleRepository, IEventPublisher eventPublisher)
    {
        _saleRepository = saleRepository ?? throw new ArgumentNullException(nameof(saleRepository));
        _eventPublisher = eventPublisher ?? throw new ArgumentNullException(nameof(eventPublisher));
    }

    /// <summary>
    /// Handles the deletion of a sale based on the provided command.
    /// </summary>
    /// <param name="request">The command containing the sale ID to be deleted.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation, containing the result of the delete operation.</returns>
    /// <exception cref="ValidationException">
    /// Thrown when the command validation fails.
    /// </exception>
    /// <exception cref="KeyNotFoundException">
    /// Thrown when a sale with the specified ID is not found.
    /// </exception>
    public async Task<DeleteSaleResponse> Handle(DeleteSaleCommand request, CancellationToken cancellationToken)
    {
        var validator = new DeleteSaleValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var success = await _saleRepository.DeleteAsync(request.Id, cancellationToken);
        if (!success)
            throw new KeyNotFoundException($"Sale with ID {request.Id} not found");

        var saleCancelledEvent = new SaleCancelledEvent(request.Id);
        await _eventPublisher.PublishAsync(saleCancelledEvent);
        return new DeleteSaleResponse { Success = true };
    }
}