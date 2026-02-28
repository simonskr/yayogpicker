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
            var csvData = await _httpClient.GetStringAsync("data/yayog.csv");
            _cachedExercises = ParseCsv(csvData);
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

    private IEnumerable<Exercise> ParseCsv(string csvData)
    {
        var exercises = new List<Exercise>();
        var lines = csvData.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

        // Skip header
        foreach (var line in lines.Skip(1))
        {
            var parts = ParseCsvLine(line);
            if (parts.Length < 4) continue;

            exercises.Add(new Exercise(parts[0], parts[1], parts[2], parts[3]));
        }

        return exercises;
    }

    private string[] ParseCsvLine(string line)
    {
        var result = new List<string>();
        var current = "";
        var inQuotes = false;

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];
            if (c == '"')
            {
                inQuotes = !inQuotes;
            }
            else if (c == ',' && !inQuotes)
            {
                result.Add(current.Trim('"'));
                current = "";
            }
            else
            {
                current += c;
            }
        }
        result.Add(current.Trim('"'));

        return result.ToArray();
    }
}
