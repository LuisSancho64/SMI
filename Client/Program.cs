using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using SMI.Client;
using SMI.Client.Auth;
using SMI.Client.Services;
using SMI.Shared.Interfaces;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<AuthorizationMessageHandler>();

// Solo servicios que existen en el cliente
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITipoDocumentoService, TipoDocumentoService>();
builder.Services.AddScoped<PersonaService>();

builder.Services.AddScoped<LocalStorageService>();

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
    provider.GetRequiredService<CustomAuthStateProvider>()
);


builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();
