using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using YayogApp.Client;
using YayogApp.Client.Services;
using YayogApp.Shared.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<Routes>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

if (builder.HostEnvironment.BaseAddress.Contains("github.io"))
{
    builder.Services.AddScoped<IExerciseService, StaticExerciseService>();
}
else
{
    builder.Services.AddScoped<IExerciseService, ApiExerciseService>();
}

await builder.Build().RunAsync();
