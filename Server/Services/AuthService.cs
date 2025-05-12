using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SMI.Server.Data;
using SMI.Server.Settings;
using SMI.Shared.DTOs;
using SMI.Shared.Interfaces;
using SMI.Shared.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SMI.Server.Services
{
    public class AuthService : IAuthService
    {
        private readonly SGISDbContext _context;
        private readonly JwtSettings _jwtSettings;

        public AuthService(SGISDbContext context, IOptions<JwtSettings> jwtSettings)
        {
            _context = context;
            _jwtSettings = jwtSettings.Value;  // Configuración de JwtSettings inyectada
        }

        public async Task<LoginResponseDto> Login(LoginDTO loginDto)
        {
            var user = await _context.Usuario
                .AsNoTracking()
                .Include(u => u.Persona)
                .FirstOrDefaultAsync(u =>
                    u.Persona.Correo == loginDto.Correo &&
                    u.Clave == loginDto.Clave &&
                    u.Activo);

            if (user == null)
                return new LoginResponseDto { Token = "", Usuario = null };

            // Generar el JWT
            var token = GenerateJwtToken(user);

            return new LoginResponseDto
            {
                Token = token,
                Usuario = new UsuarioDto
                {
                    Id = user.Id,
                    Persona = new PersonaDto
                    {
                        Nombre = user.Persona?.nombre,
                        Apellido = user.Persona?.apellido,
                        Correo = user.Persona?.Correo
                    }
                }
            };
        }

        public Task Logout()
        {
            // Aquí puedes implementar la lógica para eliminar el token en memoria o cualquier otro mecanismo de cierre de sesión.
            return Task.CompletedTask;
        }

        public Task<UsuarioDto> GetCurrentUser()
        {
            // Aquí debes recuperar el usuario desde el contexto o usar el token JWT para identificarlo
            return Task.FromResult<UsuarioDto>(null); // Reemplaza con tu lógica real
        }

        public Task<string> GetToken()
        {
            // Retorna el token simulado o real
            return Task.FromResult("FAKE-TOKEN"); // Puedes actualizar esto si tienes un token válido
        }

        public Task<bool> IsAuthenticated()
        {
            // Lógica de autenticación con el token
            return Task.FromResult(true); // Ajusta según tu lógica
        }

        // Método para generar el token JWT
        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Persona.Correo),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.Persona.nombre),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                // Agrega más claims según sea necesario, como roles
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtSettings.ExpirationMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token); // Retorna el token JWT como string
        }
    }
}
