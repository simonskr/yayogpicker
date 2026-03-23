using System.Text.Json;
using YayogApp.Shared.Models;
using YayogApp.Shared.Services;

namespace YayogApp.Services;

public class ExerciseService : IExerciseService
{
    private readonly string _jsonPath;
    private static readonly Dictionary<int, string> DifficultyMapping = new()
    {
        { 1, "Easier" },
        { 2, "Easy" },
        { 3, "Semi-Easy" },
        { 4, "Moderate" },
        { 5, "Semi-Hard" },
        { 6, "Hard" },
        { 7, "Harder" },
        { 8, "Very Hard" },
        { 9, "Hardest" }
    };

    public ExerciseService(IWebHostEnvironment env)
    {
        _jsonPath = Path.Combine(env.ContentRootPath, "Data", "yayog.json");
    }

    public async Task<IEnumerable<Exercise>> GetExercisesAsync(string? category = null, int? difficultyLevel = null)
    {
        if (!File.Exists(_jsonPath))
        {
            return [];
        }

        using var stream = File.OpenRead(_jsonPath);
        var exercises = await JsonSerializer.DeserializeAsync<List<Exercise>>(stream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();

        var filtered = exercises.AsEnumerable();

        if (!string.IsNullOrEmpty(category))
        {
            filtered = filtered.Where(e => e.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
        }

        if (difficultyLevel.HasValue && DifficultyMapping.TryGetValue(difficultyLevel.Value, out var difficultyStr))
        {
            filtered = filtered.Where(e => e.Difficulty.Equals(difficultyStr, StringComparison.OrdinalIgnoreCase));
        }

        return filtered;
    }
}
