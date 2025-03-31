using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Validation;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the Sale entity class.
/// Tests cover validation scenarios and business logic.
/// </summary>
public class SaleTests
{
    private readonly SaleValidator _validator = new();

    /// <summary>
    /// Tests that validation passes when all sale properties are valid.
    /// </summary>
    [Fact(DisplayName = "Validation should pass for valid sale data")]
    public void Given_ValidSaleData_When_Validated_Then_ShouldReturnValid()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();

        // Act
        var result = _validator.Validate(sale);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    /// <summary>
    /// Tests that validation fails when sale date is in the future.
    /// </summary>
    [Fact(DisplayName = "Validation should fail for future sale date")]
    public void Given_SaleWithFutureDate_When_Validated_Then_ShouldReturnInvalid()
    {
        // Arrange
        var sale = SaleTestData.GenerateSaleWithFutureDate();

        // Act
        var result = _validator.Validate(sale);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == "Sale date cannot be in the future.");
    }

    /// <summary>
    /// Tests that validation fails when CustomerId is empty.
    /// </summary>
    [Fact(DisplayName = "Validation should fail for empty CustomerId")]
    public void Given_SaleWithEmptyCustomerId_When_Validated_Then_ShouldReturnInvalid()
    {
        // Arrange
        var sale = SaleTestData.GenerateSaleWithEmptyCustomerId();

        // Act
        var result = _validator.Validate(sale);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == "Customer ID is required.");
    }

    /// <summary>
    /// Tests that validation fails when BranchId is empty.
    /// </summary>
    [Fact(DisplayName = "Validation should fail for empty BranchId")]
    public void Given_SaleWithEmptyBranchId_When_Validated_Then_ShouldReturnInvalid()
    {
        // Arrange
        var sale = SaleTestData.GenerateSaleWithEmptyBranchId();

        // Act
        var result = _validator.Validate(sale);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == "Branch ID is required.");
    }

    /// <summary>
    /// Tests that validation fails when TotalAmount is negative.
    /// </summary>
    [Fact(DisplayName = "Validation should fail for negative TotalAmount")]
    public void Given_SaleWithNegativeTotalAmount_When_Validated_Then_ShouldReturnInvalid()
    {
        // Arrange
        var sale = SaleTestData.GenerateSaleWithNegativeTotalAmount();

        // Act
        var result = _validator.Validate(sale);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == "Total amount must be non-negative.");
    }
}