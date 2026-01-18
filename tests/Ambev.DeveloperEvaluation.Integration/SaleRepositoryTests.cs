using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration;

/// <summary>
/// Integration tests for the sale repository.
/// </summary>
public class SaleRepositoryTests
{
    /// <summary>
    /// Tests whether the sale repository can create and retrieve a sale with items using EF Core.
    /// </summary>
    [Fact(DisplayName = "Create and retrieve sale with items using EF Core")]
    public async Task CreateAndGetById_ReturnsPersistedSale()
    {
        // Arrange: Setup the in-memory database and the repository
        var options = new DbContextOptionsBuilder<DefaultContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var context = new DefaultContext(options);
        var repository = new SaleRepository(context);

        // Act: Create a sale and add it to the repository
        var sale = new Sale
        {
            Customer = "Customer",
            Branch = "Branch",
            Items =
            [
                new SaleItem { Product = "Product", Quantity = 5, UnitPrice = 10 }
            ]
        };

        await repository.CreateAsync(sale, CancellationToken.None);

        // Retrieve the sale from the repository
        var stored = await repository.GetByIdAsync(sale.Id, CancellationToken.None);

        // Assert: Verify that the sale and its items were persisted correctly
        stored.Should().NotBeNull();
        stored!.Items.Should().HaveCount(1);
        stored.Items[0].Product.Should().Be("Product");
        stored.Items[0].Quantity.Should().Be(5);
        stored.Items[0].UnitPrice.Should().Be(10);
    }
}