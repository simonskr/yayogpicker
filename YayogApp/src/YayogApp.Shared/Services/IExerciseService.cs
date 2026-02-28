using YayogApp.Shared.Models;

namespace YayogApp.Shared.Services;

public interface IExerciseService
{
    Task<IEnumerable<Exercise>> GetExercisesAsync(string? category = null, int? difficultyLevel = null);
}
