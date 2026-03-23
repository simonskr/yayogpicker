using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;

namespace YayogApp.E2ETests;

public class ExercisePickerTests : PageTest
{
    private const string BaseUrl = "http://localhost:5009";

    [Fact]
    public async Task ExercisePicker_CanPickExercise()
    {
        await Page.GotoAsync($"{BaseUrl}/exercise-picker");

        // Wait for the page to load
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Exercise Picker" })).ToBeVisibleAsync();

        // Wait for Blazor WASM to initialize (approximate with network idle)
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await Task.Delay(1000); // Give WASM a moment to hook up event listeners

        // Select a category
        await Page.GetByLabel("Category").SelectOptionAsync(new[] { "Push" });

        // Set difficulty
        await Page.GetByLabel("Difficulty (1-9)").FillAsync("4");

        // Click the pick button
        await Page.GetByRole(AriaRole.Button, new() { Name = "Pick Something!" }).ClickAsync();

        // Check if an exercise is displayed
        // We expect the exercise category to appear
        await Expect(Page.Locator(".bg-blue-500\\/20").GetByText("Push")).ToBeVisibleAsync();
        
        // The difficulty text should be visible and correct
        await Expect(Page.GetByText("Difficulty: Moderate")).ToBeVisibleAsync();
    }
}
