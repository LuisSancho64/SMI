using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SMI.Client;
using SMI.Client.Services;
using SMI.Shared.Interfaces;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Aqu� debes a�adir servicios que el cliente necesita, no servicios del servidor directamente
builder.Services.AddScoped<IAuthService, AuthService>(); // Esto es incorrecto si AuthService est� en el servidor

//servicio de la persona
builder.Services.AddScoped<PersonaService>();

await builder.Build().RunAsync();
