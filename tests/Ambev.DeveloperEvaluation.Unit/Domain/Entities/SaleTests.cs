using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Xunit;
using FluentAssertions;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Unit tests for the <see cref="Sale"/> domain entity.
/// </summary>
public class SaleTests
{
    /// <summary>
    /// Tests that a 10% discount is applied when the item quantity is between 4 and 9.
    /// </summary>
    [Fact(DisplayName = "CalculateDiscountAndTotal should apply 10% discount for 4-9 items")]
    public void Given_4Items_When_CalculateDiscountAndTotal_Then_Apply10PercentDiscount()
    {
        // Arrange
        var sale = new Sale();
        sale.Items.Add(new SaleItem { Quantity = 4, UnitPrice = 100 });

        // Act
        sale.CalculateDiscountAndTotal();

        // Assert
        sale.Items[0].Discount.Should().Be(40); // 4 * 100 * 0.10
        sale.TotalAmount.Should().Be(360);     // 400 - 40
    }

    /// <summary>
    /// Tests that a 20% discount is applied when the item quantity is between 10 and 20.
    /// </summary>
    [Fact(DisplayName = "CalculateDiscountAndTotal should apply 20% discount for 10-20 items")]
    public void Given_10Items_When_CalculateDiscountAndTotal_Then_Apply20PercentDiscount()
    {
        // Arrange
        var sale = new Sale();
        sale.Items.Add(new SaleItem { Quantity = 10, UnitPrice = 100 });

        // Act
        sale.CalculateDiscountAndTotal();

        // Assert
        sale.Items[0].Discount.Should().Be(200); // 10 * 100 * 0.20
        sale.TotalAmount.Should().Be(800);      // 1000 - 200
    }

    /// <summary>
    /// Tests that no discount is applied when the item quantity is less than 4.
    /// </summary>
    [Fact(DisplayName = "CalculateDiscountAndTotal should apply no discount for < 4 items")]
    public void Given_3Items_When_CalculateDiscountAndTotal_Then_ApplyNoDiscount()
    {
        // Arrange
        var sale = new Sale();
        sale.Items.Add(new SaleItem { Quantity = 3, UnitPrice = 100 });

        // Act
        sale.CalculateDiscountAndTotal();

        // Assert
        sale.Items[0].Discount.Should().Be(0);
        sale.TotalAmount.Should().Be(300);
    }

    /// <summary>
    /// Tests that cancelling a sale raises a <see cref="SaleCancelledEvent"/>.
    /// </summary>
    [Fact(DisplayName = "Cancel should raise SaleCancelledEvent")]
    public void Given_Sale_When_Cancel_Then_RaiseSaleCancelledEvent()
    {
        // Arrange
        var sale = new Sale();

        // Act
        sale.Cancel();

        // Assert
        sale.DomainEvents.Should()
            .ContainItemsAssignableTo<SaleCancelledEvent>();
    }
}