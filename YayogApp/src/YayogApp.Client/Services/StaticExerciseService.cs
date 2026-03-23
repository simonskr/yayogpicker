using System.Net.Http.Json;
using YayogApp.Shared.Models;
using YayogApp.Shared.Services;

namespace YayogApp.Client.Services;

public class StaticExerciseService : IExerciseService
{
    private readonly HttpClient _httpClient;
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

        if (difficultyLevel.HasValue && ExerciseDifficulties.Mapping.TryGetValue(difficultyLevel.Value, out var difficultyStr))
        {
            filtered = filtered.Where(e => e.Difficulty.Equals(difficultyStr, StringComparison.OrdinalIgnoreCase));
        }

        return filtered;
    }
}
