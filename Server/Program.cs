using Microsoft.EntityFrameworkCore;
using SMI.Server.Data;
using SMI.Shared.Interfaces;
using SMI.Server.Services;


var builder = WebApplication.CreateBuilder(args);

// Añadir servicios
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddScoped<ITipoDocumentoService, TipoDocumentoService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();


// Configurar DbContext para SGISDbContext (para el login u otro contexto general)
builder.Services.AddDbContext<SGISDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // Cadena de conexión para el login u otro

//CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("https://localhost:7191") // o el puerto donde corre tu Blazor WASM
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});



// Crear el objeto de la aplicación
var app = builder.Build();

// Configuración del pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();
app.UseCors(); // Habilitar CORS

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
