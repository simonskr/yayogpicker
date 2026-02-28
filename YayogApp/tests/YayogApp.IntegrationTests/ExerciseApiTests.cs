using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using YayogApp.Shared.Models;
using YayogApp.Client.Services;

namespace YayogApp.IntegrationTests;

public class ExerciseApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public ExerciseApiTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetExercises_WithPushAndModerate_ReturnsExercises()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        // Difficulty 4 should map to "Moderate"
        var exercises = await client.GetFromJsonAsync<IEnumerable<Exercise>>("/api/exercises?category=Push&difficulty=4");

        // Assert
        Assert.NotNull(exercises);
        Assert.NotEmpty(exercises);
        foreach (var exercise in exercises)
        {
            Assert.Equal("Push", exercise.Category);
            Assert.Equal("Moderate", exercise.Difficulty);
        }
    }

    [Fact]
    public async Task GetExercises_WithInvalidDifficulty_ReturnsAllForCategory()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        // Difficulty 99 is invalid, should return all for category or empty depending on implementation
        var exercises = await client.GetFromJsonAsync<IEnumerable<Exercise>>("/api/exercises?category=Push&difficulty=99");

        // Assert
        Assert.NotNull(exercises);
        Assert.NotEmpty(exercises);
        Assert.All(exercises, e => Assert.Equal("Push", e.Category));
    }

    [Fact]
    public async Task GetExercises_WithMissingCsv_ReturnsEmpty()
    {
        // Arrange
        // Using a dummy content root where Data/yayog.csv doesn't exist
        var emptyDataDir = Path.Combine(Directory.GetCurrentDirectory(), "EmptyData");
        Directory.CreateDirectory(emptyDataDir);
        
        var factory = _factory.WithWebHostBuilder(builder =>
        {
            builder.UseContentRoot(emptyDataDir);
        });
        var client = factory.CreateClient();

        // Act
        var exercises = await client.GetFromJsonAsync<IEnumerable<Exercise>>("/api/exercises");

        // Assert
        Assert.NotNull(exercises);
        Assert.Empty(exercises);
    }

    [Fact]
    public async Task ApiExerciseService_ReturnsExercises()
    {
        // Arrange
        var client = _factory.CreateClient();
        var apiService = new ApiExerciseService(client);

        // Act
        var exercises = await apiService.GetExercisesAsync("Push", 4);

        // Assert
        Assert.NotNull(exercises);
        Assert.NotEmpty(exercises);
        Assert.All(exercises, e => {
            Assert.Equal("Push", e.Category);
            Assert.Equal("Moderate", e.Difficulty);
        });
    }
}
