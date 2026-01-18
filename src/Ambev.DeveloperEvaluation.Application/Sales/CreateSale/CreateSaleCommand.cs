using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Represents a command to create a new sale, including sale details, customer information, and associated items.
/// </summary>
/// <remarks>Use this command to initiate the creation of a sale transaction. The command includes the sale date,
/// customer and branch information, a collection of sale items, the total amount, and a flag indicating whether the
/// sale is cancelled. This type is typically used in a request/response pattern to encapsulate all data required to
/// create a sale.</remarks>
public class CreateSaleCommand : IRequest<CreateSaleCommandResult>
{
    public DateTime Date { get; set; }
    public string? Customer { get; set; }
    public string? Branch { get; set; }
    public List<CreateSaleItemCommand>? Items { get; set; }
    public decimal TotalAmount { get; set; }
    public bool IsCancelled { get; set; }
}

/// <summary>
/// Represents a command to create a new sale item with specified product details, quantity, pricing, and discount
/// information.
/// </summary>
/// <remarks>This class is typically used to encapsulate the data required when adding a sale item to an order or
/// transaction. All properties should be set before processing the command. The command does not perform validation;
/// ensure that property values meet business requirements before use.</remarks>
public class CreateSaleItemCommand
{
    public string? Product { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Discount { get; set; }
    public bool IsCancelled { get; set; }
}