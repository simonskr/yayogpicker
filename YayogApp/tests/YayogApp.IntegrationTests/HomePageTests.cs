using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace YayogApp.IntegrationTests;

public class HomePageTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public HomePageTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Get_HomePageReturnsSuccess()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/");

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal("text/html; charset=utf-8", 
            response.Content.Headers.ContentType?.ToString());
    }

    [Fact]
    public async Task Get_HomePageContainsCssLink()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/");
        var content = await response.Content.ReadAsStringAsync();
        
        // Assert
        response.EnsureSuccessStatusCode();
        // .NET 9+ MapStaticAssets adds a hash like app.xxxx.css
        Assert.Matches(@"href=""app\.[a-z0-9]+\.css""", content);
        Assert.Contains("rel=\"stylesheet\"", content);
    }

    [Fact]
    public async Task Get_NonExistentPageReturnsNotFound()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/this-does-not-exist");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
