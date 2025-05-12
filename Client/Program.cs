// SMI.Client/Program.cs

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SMI.Client;
using SMI.Client.Services;
using SMI.Shared.Interfaces;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});

// Solo servicios que existen en el cliente
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITipoDocumentoService, TipoDocumentoService>();
builder.Services.AddScoped<PersonaService>();

await builder.Build().RunAsync();
