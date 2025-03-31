namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

/// <summary>
/// Represents the response from a cancel sale operation. Contains a success flag and an optional message.
/// </summary>
public class CancelSaleResponse
{
	/// <summary>
	/// Indicates whether an operation was successful. It is a boolean property that can be true or false.
	/// </summary>
	public bool Success { get; set; }

	/// <summary>
	/// Represents a message as a string. It can be accessed and modified through its getter and setter.
	/// </summary>
	public string Message { get; set; }
}
