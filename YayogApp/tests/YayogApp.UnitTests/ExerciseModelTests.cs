using YayogApp.Shared.Models;
using Xunit;

namespace YayogApp.UnitTests;

public class ExerciseModelTests
{
    [Fact]
    public void Exercise_CanBeCreated()
    {
        // Arrange
        var name = "Push-up";
        var variation = "Standard";
        var category = "Push";
        var difficulty = "Easy";

        // Act
        var exercise = new Exercise(name, variation, category, difficulty);

        // Assert
        Assert.Equal(name, exercise.Name);
        Assert.Equal(variation, exercise.Variation);
        Assert.Equal(category, exercise.Category);
        Assert.Equal(difficulty, exercise.Difficulty);
    }

    [Fact]
    public void Exercise_EqualityTest()
    {
        // Arrange
        var e1 = new Exercise("A", "B", "C", "D");
        var e2 = new Exercise("A", "B", "C", "D");
        var e3 = new Exercise("X", "B", "C", "D");

        // Assert
        Assert.Equal(e1, e2);
        Assert.NotEqual(e1, e3);
    }
}
