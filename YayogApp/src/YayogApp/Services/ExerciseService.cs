using YayogApp.Shared.Models;
using YayogApp.Shared.Services;

namespace YayogApp.Services;

public class ExerciseService : IExerciseService
{
    private readonly string _csvPath;
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
        _csvPath = Path.Combine(env.ContentRootPath, "Data", "yayog.csv");
    }

    public async Task<IEnumerable<Exercise>> GetExercisesAsync(string? category = null, int? difficultyLevel = null)
    {
        if (!File.Exists(_csvPath))
        {
            return [];
        }

        var lines = await File.ReadAllLinesAsync(_csvPath);
        var exercises = new List<Exercise>();

        // Skip header
        foreach (var line in lines.Skip(1))
        {
            var parts = ParseCsvLine(line);
            if (parts.Length < 4) continue;

            exercises.Add(new Exercise(parts[0], parts[1], parts[2], parts[3]));
        }

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

    private string[] ParseCsvLine(string line)
    {
        // Basic CSV parser that handles quotes
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
