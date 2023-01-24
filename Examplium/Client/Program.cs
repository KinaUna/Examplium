using Examplium.Client;
using Examplium.Client.BFF;
using Examplium.Client.Services.Notes;
using Examplium.Client.Services.Settings;
using Examplium.Client.Services.UserInfos;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddTransient<AntiForgeryHandler>();

builder.Services.AddHttpClient("Examplium.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<AntiForgeryHandler>();

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Examplium.ServerAPI"));

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, BffAuthenticationStateProvider>();

builder.Services.AddApiAuthorization();

builder.Services.AddScoped<INotesService, NotesService>();
builder.Services.AddScoped<IUserInfosService, UserInfosService>();
builder.Services.AddScoped<ITimezonesService, TimezonesService>();

await builder.Build().RunAsync();
