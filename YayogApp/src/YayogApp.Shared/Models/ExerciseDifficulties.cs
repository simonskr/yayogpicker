namespace YayogApp.Shared.Models;

public static class ExerciseDifficulties
{
    public static readonly IReadOnlyDictionary<int, string> Mapping = new Dictionary<int, string>
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
}
