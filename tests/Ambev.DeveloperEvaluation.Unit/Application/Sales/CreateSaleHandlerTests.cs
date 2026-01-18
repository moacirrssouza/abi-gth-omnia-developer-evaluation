using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

/// <summary>
/// Unit tests for the <see cref="CreateSaleHandler"/>.
/// </summary>
public class CreateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateSaleHandler> _logger;
    private readonly IPublisher _publisher;
    private readonly CreateSaleHandler _handler;

    /// <summary>
    /// Initializes the test class and configures mocked dependencies.
    /// </summary>
    public CreateSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<CreateSaleHandler>>();
        _publisher = Substitute.For<IPublisher>();
        _handler = new CreateSaleHandler(_saleRepository, _mapper, _logger, _publisher);
    }

    /// <summary>
    /// Tests that given a valid sale, the handler applies discount rules
    /// and correctly calculates the total amount.
    /// </summary>
    [Fact(DisplayName = "Given valid sale When creating Then applies discount rules and calculates total")]
    public async Task Handle_ValidSale_AppliesDiscountsAndCalculatesTotal()
    {
        // Arrange
        var command = new CreateSaleCommand
        {
            Date = DateTime.UtcNow,
            Customer = "Customer",
            Branch = "Branch",
            Items =
            [
                new CreateSaleItemCommand
                {
                    Product = "Product",
                    Quantity = 5,
                    UnitPrice = 100
                }
            ]
        };

        _mapper.Map<Sale>(command).Returns(call =>
        {
            var c = (CreateSaleCommand)call[0]!;
            return new Sale
            {
                Date = c.Date,
                Customer = c.Customer,
                Branch = c.Branch,
                Items = c.Items!
                    .Select(i => new SaleItem
                    {
                        Product = i.Product,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice,
                        Discount = i.Discount,
                        IsCancelled = i.IsCancelled
                    }).ToList()
            };
        });

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _saleRepository.Received(1).CreateAsync(
            Arg.Is<Sale>(s =>
                s.TotalAmount == 450 &&
                s.Items.Single().Discount == 50),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that when an item quantity exceeds the allowed limit,
    /// the handler returns an error and does not persist the sale.
    /// </summary>
    [Fact(DisplayName = "Given quantity above limit When creating Then returns error")]
    public async Task Handle_QuantityAboveLimit_ReturnsError()
    {
        // Arrange
        var command = new CreateSaleCommand
        {
            Date = DateTime.UtcNow,
            Customer = "Customer",
            Branch = "Branch",
            Items =
            [
                new CreateSaleItemCommand
                {
                    Product = "Product",
                    Quantity = 21,
                    UnitPrice = 10
                }
            ]
        };

        _mapper.Map<Sale>(command).Returns(call =>
        {
            var c = (CreateSaleCommand)call[0]!;
            return new Sale
            {
                Date = c.Date,
                Customer = c.Customer,
                Branch = c.Branch,
                Items = c.Items!
                    .Select(i => new SaleItem
                    {
                        Product = i.Product,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice,
                        Discount = i.Discount,
                        IsCancelled = i.IsCancelled
                    }).ToList()
            };
        });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
        await _saleRepository.DidNotReceive()
            .CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }
}