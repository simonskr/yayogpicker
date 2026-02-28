using System.Net.Http.Json;
using YayogApp.Shared.Models;
using YayogApp.Shared.Services;

namespace YayogApp.Client.Services;

public class ApiExerciseService : IExerciseService
{
    private readonly HttpClient _httpClient;

    public ApiExerciseService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<Exercise>> GetExercisesAsync(string? category = null, int? difficultyLevel = null)
    {
        var url = $"/api/exercises?category={category}&difficulty={difficultyLevel}";
        var result = await _httpClient.GetFromJsonAsync<IEnumerable<Exercise>>(url);
        return result ?? [];
    }
}
