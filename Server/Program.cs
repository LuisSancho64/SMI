using Microsoft.EntityFrameworkCore;
using SMI.Server.Data;
using SMI.Shared.Data;  // Asegúrate de importar el espacio de nombres de tu DbContext de Persona

var builder = WebApplication.CreateBuilder(args);

// Añadir servicios
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Configurar DbContext para SGISDbContext (para el login u otro contexto general)
builder.Services.AddDbContext<SGISDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // Cadena de conexión para el login u otro

// Configurar DbContext para SGISPersonDbContext (para la tabla Persona)
builder.Services.AddDbContext<SGISPersonDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // Cadena de conexión para la base de datos que usa Persona

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

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
