using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMI.Server.Data;
using SMI.Server.Services;
using SMI.Shared.DTOs;
using SMI.Shared.Interfaces;
using SMI.Shared.Models;

namespace SMI.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SGISDbContext _context;
        private readonly IPasswordService _passwordService;

        public AuthController(SGISDbContext context, IPasswordService passwordService)
        {
            _context = context;
            _passwordService = passwordService;

            // Intentar hacer una consulta simple para verificar la conexión
            try
            {
                var testConnection = _context.Usuario.Take(1).FirstOrDefault(); // Solo para probar la conexión
                if (testConnection == null)
                {
                    Console.WriteLine("No se pudo conectar a la base de datos.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al conectar a la base de datos: {ex.Message}");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            try
            {
                // Primero, buscar el usuario por correo (sin verificar la contraseña aún)
                var usuario = await _context.Usuario
                    .Include(u => u.Persona)
                    .Where(u => u.Persona.Correo == loginDto.Correo && u.Activo)
                    .FirstOrDefaultAsync();

                if (usuario == null)
                {
                    return Unauthorized("Usuario no encontrado o no activo.");
                }

                // Verificar la contraseña usando BCrypt
                if (!_passwordService.VerifyPassword(loginDto.Clave, usuario.Clave))
                {
                    return Unauthorized("Contraseña incorrecta.");
                }

                // Si llegamos aquí, el usuario y la contraseña son correctos
                var usuarioDto = new UsuarioDto
                {
                    Id = usuario.Id,
                    Activo = usuario.Activo,
                    // No incluir la contraseña en la respuesta
                    Persona = new PersonaDto
                    {
                        Id = usuario.Persona.id,
                        Correo = usuario.Persona.Correo,
                        Nombre = usuario.Persona.nombre,
                        Apellido = usuario.Persona.apellido,
                        FechaNacimiento = usuario.Persona.FechaNacimiento ?? DateTime.MinValue,
                        Id_Genero = usuario.Persona.id_Genero ?? 0,
                    }
                };

                return Ok(usuarioDto);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al realizar el login: {ex.Message}");
            }
        }

        // Método para migrar contraseñas existentes
        [HttpPost("migrar-contrasenas")]
        public async Task<IActionResult> MigrarContrasenas()
        {
            try
            {
                await MigrarContrasenasExistentes();
                return Ok("Contraseñas migradas correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al migrar contraseñas: {ex.Message}");
            }
        }

        // Método privado para realizar la migración
        private async Task MigrarContrasenasExistentes()
        {
            var usuarios = await _context.Usuario.ToListAsync();

            foreach (var usuario in usuarios)
            {
                // Asumiendo que las contraseñas actuales están en texto plano
                string contrasenaTextoPlano = usuario.Clave;

                // Hashear la contraseña
                usuario.Clave = _passwordService.HashPassword(contrasenaTextoPlano);

                // Marcar la entidad como modificada
                _context.Entry(usuario).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();
        }
    }
}