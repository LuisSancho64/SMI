using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using SMI.Server.Settings;
using SMI.Server.Data;
using SMI.Shared.Interfaces;
using SMI.Server.Services;


var builder = WebApplication.CreateBuilder(args);

// Añadir servicios
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddScoped<ITipoDocumentoService, TipoDocumentoService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPersona, PersonaRepository>();
builder.Services.AddScoped<PersonaService>();

//Configuracion JWT
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
var key = Encoding.ASCII.GetBytes(jwtSettings.Key);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})

    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

// Configurar DbContext para SGISDbContext (para el login u otro contexto general)
builder.Services.AddDbContext<SGISDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // Cadena de conexión para el login u otro

builder.Services.AddScoped<ICiudadService, CiudadService>();
builder.Services.AddScoped<IEstadoCivilService, EstadoCivilService>();

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
app.UseRouting();              // Primero el enrutamiento
app.UseAuthentication();       // Luego la autenticación
app.UseAuthorization();        // Luego la autorización
app.UseCors();                 

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
