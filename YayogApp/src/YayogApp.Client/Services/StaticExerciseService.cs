using System.Net.Http.Json;
using YayogApp.Shared.Models;
using YayogApp.Shared.Services;

namespace YayogApp.Client.Services;

public class StaticExerciseService : IExerciseService
{
    private readonly HttpClient _httpClient;
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

    private IEnumerable<Exercise>? _cachedExercises;

    public StaticExerciseService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<Exercise>> GetExercisesAsync(string? category = null, int? difficultyLevel = null)
    {
        if (_cachedExercises == null)
        {
            // GitHub Pages hosting: use the static JSON file
            _cachedExercises = await _httpClient.GetFromJsonAsync<IEnumerable<Exercise>>("data/yayog.json") ?? [];
        }

        var filtered = _cachedExercises;

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
