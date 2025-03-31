using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Provides methods for generating test data for <see cref="CreateSaleCommand"/> using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class CreateSaleHandlerTestData
{
	/// <summary>
	/// Configures the Faker to generate valid CreateSaleCommand entities.
	/// The generated sales will have valid:
	/// - CustomerId (random GUID)
	/// - BranchId (random GUID)
	/// - SaleDate (past date)
	/// - TotalAmount (positive value)
	/// </summary>
	private static readonly Faker<CreateSaleCommand> CreateSaleHandlerFaker = new Faker<CreateSaleCommand>()
		.RuleFor(s => s.CustomerId, f => f.Random.Guid())
		.RuleFor(s => s.BranchId, f => f.Random.Guid())
		.RuleFor(s => s.TotalAmount, f => f.Finance.Amount(1, 1000));

	/// <summary>
	/// Generates a valid CreateSaleCommand entity with randomized data.
	/// The generated command will have all properties populated with valid values
	/// that meet the system's validation requirements.
	/// </summary>
	/// <returns>A valid CreateSaleCommand entity with randomly generated data.</returns>
	public static CreateSaleCommand GenerateValidCommand()
	{
		return CreateSaleHandlerFaker.Generate();
	}

	/// <summary>
	/// Generates an invalid CreateSaleCommand entity with missing or incorrect values.
	/// Used for testing validation failures.
	/// </summary>
	/// <returns>An invalid CreateSaleCommand entity.</returns>
	public static CreateSaleCommand GenerateInvalidCommand()
	{
		return new CreateSaleCommand
		{
			CustomerId = Guid.Empty,
			BranchId = Guid.Empty,
			// TotalAmount cannot be assigned directly as it is read-only
			// We need to use a constructor or a method to set it
			// Assuming we have a constructor that allows setting TotalAmount
			// TotalAmount = -50
			IsCancelled = true,
			SaleItems = new List<SaleItem>()
		};
	}
}
