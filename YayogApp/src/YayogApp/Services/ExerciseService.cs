using System.Text.Json;
using YayogApp.Shared.Models;
using YayogApp.Shared.Services;

namespace YayogApp.Services;

public class ExerciseService : IExerciseService
{
    private readonly string _jsonPath;
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

        if (difficultyLevel.HasValue && ExerciseDifficulties.Mapping.TryGetValue(difficultyLevel.Value, out var difficultyStr))
        {
            filtered = filtered.Where(e => e.Difficulty.Equals(difficultyStr, StringComparison.OrdinalIgnoreCase));
        }

        return filtered;
    }
}
