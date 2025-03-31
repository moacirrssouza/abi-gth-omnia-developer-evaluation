using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
/// Provides methods for generating test data using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class SaleTestData
{
    private static readonly Faker<Sale> SaleFaker = new Faker<Sale>()
        .CustomInstantiator(f => new Sale(
            f.Random.Guid(),
            f.Random.Guid(),
            new List<SaleItem>
            {
                new SaleItem(f.Random.Guid(), f.Random.Int(1, 10), f.Random.Decimal(1, 100))
            }
        ))
        .RuleFor(s => s.SaleDate, f => f.Date.Past())
        .RuleFor(s => s.IsCancelled, f => f.Random.Bool());

    /// <summary>
    /// Generates a valid Sale entity with randomized data.
    /// </summary>
    /// <returns>A valid Sale entity.</returns>
    public static Sale GenerateValidSale()
    {
        return SaleFaker.Generate();
    }

    /// <summary>
    /// Generates a Sale entity with a future SaleDate to test validation errors.
    /// </summary>
    /// <returns>An invalid Sale entity.</returns>
    public static Sale GenerateSaleWithFutureDate()
    {
        var sale = SaleFaker.Generate();
        sale.SaleDate = DateTime.UtcNow.AddDays(1);
        return sale;
    }

    /// <summary>
    /// Generates a Sale entity with an empty CustomerId to test validation errors.
    /// </summary>
    /// <returns>An invalid Sale entity.</returns>
    public static Sale GenerateSaleWithEmptyCustomerId()
    {
        var sale = SaleFaker.Generate();
        sale.CustomerId = Guid.Empty;
        return sale;
    }

    /// <summary>
    /// Generates a Sale entity with an empty BranchId to test validation errors.
    /// </summary>
    /// <returns>An invalid Sale entity.</returns>
    public static Sale GenerateSaleWithEmptyBranchId()
    {
        var sale = SaleFaker.Generate();
        sale.BranchId = Guid.Empty;
        return sale;
    }

    /// <summary>
    /// Generates a Sale entity with a negative TotalAmount to test validation errors.
    /// </summary>
    /// <returns>An invalid Sale entity.</returns>
    public static Sale GenerateSaleWithNegativeTotalAmount()
    {
        var sale = SaleFaker.Generate();
        sale.SaleItems = new List<SaleItem> { new SaleItem(Guid.NewGuid(), 1, -10) };
        sale.RecalculateTotal();
        return sale;
    }
}