using Ambev.DeveloperEvaluation.Application.Common.Events;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Tests the CreateSaleHandler class for various scenarios including valid and invalid sale data handling. 
/// Verifies mapping, repository calls, and event publishing.
/// </summary>
public class CreateSaleHandlerTests
{
	/// <summary>
	/// A private readonly field that holds a reference to an ISaleRepository instance. It is used to interact with sale data.
	/// </summary>
	private readonly ISaleRepository _saleRepository;
	private readonly IMapper _mapper;
	private readonly IEventPublisher _eventPublisher;
	private readonly CreateSaleHandler _handler;

	/// <summary>
	/// Initializes a new instance of the CreateSaleHandlerTests class. Sets up substitutes for ISaleRepository, IMapper, and IEventPublisher.
	/// </summary>
	public CreateSaleHandlerTests()
	{
		_saleRepository = Substitute.For<ISaleRepository>();
		_mapper = Substitute.For<IMapper>();
		_eventPublisher = Substitute.For<IEventPublisher>();
		_handler = new CreateSaleHandler(_saleRepository, _mapper, _eventPublisher);
	}

	/// <summary>
	/// Handles a valid sale request and returns a success response with the sale ID.
	/// </summary>
	/// <returns>Returns a CreateSaleResult containing the ID of the created sale.</returns>
	[Fact(DisplayName = "Given valid sale data When creating sale Then returns success response")]
	public async Task Handle_ValidRequest_ReturnsSuccessResponse()
	{
		var command = CreateSaleHandlerTestData.GenerateValidCommand();
		// Replace the line causing the error with the following code
		var sale = new Sale(command.CustomerId, command.BranchId, command.SaleItems)
		{
			SaleDate = DateTime.UtcNow,
			IsCancelled = command.IsCancelled
		};

		sale.RecalculateTotal();

		var result = new CreateSaleResult { Id = sale.Id };

		_mapper.Map<Sale>(command).Returns(sale);
		_mapper.Map<CreateSaleResult>(sale).Returns(result);

		_saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
			.Returns(sale);

		var createSaleResult = await _handler.Handle(command, CancellationToken.None);

		createSaleResult.Should().NotBeNull();
		createSaleResult.Id.Should().Be(sale.Id);
		await _saleRepository.Received(1).CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
	}

	/// <summary>
	/// Tests that an invalid sale command triggers a validation exception when handled.
	/// </summary>
	/// <returns>No return value as the method is a test that expects an exception to be thrown.</returns>
	[Fact(DisplayName = "Given invalid sale data When creating sale Then throws validation exception")]
	public async Task Handle_InvalidRequest_ThrowsValidationException()
	{
		var command = CreateSaleHandlerTestData.GenerateInvalidCommand(); 

		var act = () => _handler.Handle(command, CancellationToken.None);

		await act.Should().ThrowAsync<FluentValidation.ValidationException>();
	}

	/// <summary>
	/// Handles a valid sale creation request by mapping the command to a sale entity and saving it.
	/// </summary>
	/// <returns>Returns a task that represents the asynchronous operation.</returns>
	[Fact(DisplayName = "Given valid sale creation request When handling Then maps command to sale entity")]
	public async Task Handle_ValidRequest_MapsCommandToSale()
	{
		var command = CreateSaleHandlerTestData.GenerateValidCommand();
		var sale = new Sale(command.CustomerId, command.BranchId, command.SaleItems)
		{
			SaleDate = DateTime.UtcNow,
			IsCancelled = command.IsCancelled
		};

		_mapper.Map<Sale>(command).Returns(sale);
		_saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
			.Returns(sale);

		await _handler.Handle(command, CancellationToken.None);

		_mapper.Received(1).Map<Sale>(Arg.Is<CreateSaleCommand>(c =>
			c.CustomerId == command.CustomerId &&
			c.BranchId == command.BranchId &&
			c.TotalAmount == command.TotalAmount));
	}

	/// <summary>
	/// Tests that a valid sale request correctly triggers a call to the sale repository to create a sale.
	/// </summary>
	/// <returns>No return value as this is a void asynchronous method.</returns>
	[Fact(DisplayName = "Given valid sale request When handling Then calls sale repository")]
	public async Task Handle_ValidRequest_CallsSaleRepository()
	{
		var command = CreateSaleHandlerTestData.GenerateValidCommand();
		var sale = new Sale(command.CustomerId, command.BranchId, command.SaleItems)
		{
			SaleDate = DateTime.UtcNow,
			IsCancelled = command.IsCancelled
		};

		_mapper.Map<Sale>(command).Returns(sale);
		_saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
			.Returns(sale);

		await _handler.Handle(command, CancellationToken.None);

		await _saleRepository.Received(1).CreateAsync(Arg.Is<Sale>(s =>
			s.CustomerId == command.CustomerId &&
			s.BranchId == command.BranchId &&
			s.TotalAmount == command.TotalAmount), Arg.Any<CancellationToken>());
	}

	/// <summary>
	/// Handles a valid sale creation request and publishes a SaleCreatedEvent upon successful processing.
	/// </summary>
	/// <returns>No return value as this is a void method.</returns>
	[Fact(DisplayName = "Given valid sale creation When handling Then publishes SaleCreatedEvent")]
	public async Task Handle_ValidRequest_PublishesSaleCreatedEvent()
	{
		var command = CreateSaleHandlerTestData.GenerateValidCommand();
		var sale = new Sale(command.CustomerId, command.BranchId, command.SaleItems)
		{
			SaleDate = DateTime.UtcNow,
			IsCancelled = command.IsCancelled
		};

		var saleCreatedEvent = new SaleCreatedEvent(sale.Id);

		_mapper.Map<Sale>(command).Returns(sale);
		_saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
			.Returns(sale);

		await _handler.Handle(command, CancellationToken.None);

		await _eventPublisher.Received(1).PublishAsync(Arg.Is<SaleCreatedEvent>(e => e.SaleId == sale.Id));
	}

	/// <summary>
	/// Tests that an exception is thrown when the repository fails during the handling of a command.
	/// </summary>
	/// <returns>No return value as it is a test method.</returns>
	[Fact(DisplayName = "Given repository failure When handling Then throws exception")]
	public async Task Handle_RepositoryFailure_ThrowsException()
	{
		var command = CreateSaleHandlerTestData.GenerateValidCommand();
		_saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
			.Throws(new Exception("Database error"));

		var act = () => _handler.Handle(command, CancellationToken.None);

		await act.Should().ThrowAsync<Exception>().WithMessage("Database error");
	}
}