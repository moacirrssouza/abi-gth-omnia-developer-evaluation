using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.WebApi;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using System.Net;
using Xunit;

namespace Ambev.DeveloperEvaluation.Functional;

/// <summary>
/// Functional tests for the sales endpoints.
/// </summary>
public class SalesEndpointsTests
{
    /// <summary>
    /// Tests if the sale creation endpoint returns a 201 Created response for a valid request.
    /// </summary>
    [Fact(DisplayName = "Create sale endpoint returns 201 for valid request")]
    public async Task CreateSale_ReturnsCreated()
    {
        await using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        // Prepare the request payload for creating a sale
        var request = new
        {
            date = DateTime.UtcNow,
            customer = "Customer",
            branch = "Branch",
            items = new[]
            {
                new { product = "Product", quantity = 5, unitPrice = 10m }
            }
        };

        // Send POST request to the /api/sales endpoint
        var response = await client.PostAsJsonAsync("/api/sales", request);

        // Assert that the response status code is 201 Created
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }
}

/// <summary>
/// Factory class for creating a customized WebApplicationFactory for functional tests.
/// </summary>
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    /// <summary>
    /// Configures the web host to use an in-memory database for functional tests.
    /// </summary>
    /// <param name="builder">The web host builder used to configure the services.</param>
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove existing DbContextOptions configuration
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<DefaultContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Add an in-memory database for testing purposes
            services.AddDbContext<DefaultContext>(options =>
            {
                options.UseInMemoryDatabase("FunctionalTests");
            });
        });
    }
}