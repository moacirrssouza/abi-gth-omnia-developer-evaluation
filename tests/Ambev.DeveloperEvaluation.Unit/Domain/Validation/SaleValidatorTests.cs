using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Validation;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation;

/// <summary>
/// Contains unit tests for validating sales data. Tests check for empty CustomerId, BranchId, future SaleDate, and
/// negative TotalAmount.
/// </summary>
public class SaleValidatorTests
{
    /// <summary>
    /// Holds an instance of SaleValidator for validating sales. It is marked as private and readonly, ensuring it cannot be modified after initialization.
    /// </summary>
    private readonly SaleValidator _validator;

    /// <summary>
    /// Initializes a new instance of the SaleValidatorTests class. Sets up a SaleValidator object for testing.
    /// </summary>
    public SaleValidatorTests()
    {
        _validator = new SaleValidator();
    }

    /// <summary>
    /// Validates that an error is generated when the CustomerId is empty in a Sale object. Ensures proper error handling for invalid input.
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_CustomerId_Is_Empty()
    {
        var sale = new Sale(Guid.Empty, Guid.NewGuid());
        var result = _validator.TestValidate(sale);
        result.ShouldHaveValidationErrorFor(s => s.CustomerId);
    }

    /// <summary>
    /// Validates that an error is generated when the BranchId is empty in a Sale object. Ensures proper error handling for invalid input.
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_BranchId_Is_Empty()
    {
        var sale = new Sale(Guid.NewGuid(), Guid.Empty);
        var result = _validator.TestValidate(sale);
        result.ShouldHaveValidationErrorFor(s => s.BranchId);
    }

    /// <summary>
    /// Validates that a sale cannot have a sale date set in the future. It checks for validation errors on the SaleDate property.
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_SaleDate_Is_In_The_Future()
    {
        var sale = new Sale(Guid.NewGuid(), Guid.NewGuid()) { SaleDate = DateTime.UtcNow.AddDays(1) };
        var result = _validator.TestValidate(sale);
        result.ShouldHaveValidationErrorFor(s => s.SaleDate);
    }

    /// <summary>
    /// Validates that a sale has an error when the total amount is negative. It checks the sale's total after adding a sale item with a negative price.
    /// </summary>
    [Fact]
    public void Should_Have_Error_When_TotalAmount_Is_Negative()
    {
        var sale = new Sale(Guid.NewGuid(), Guid.NewGuid());
        sale.SaleItems.Add(new SaleItem(Guid.NewGuid(), 1, -10)); 
        sale.RecalculateTotal(); 

        var result = _validator.TestValidate(sale);
        result.ShouldHaveValidationErrorFor(s => s.TotalAmount);
    }
}