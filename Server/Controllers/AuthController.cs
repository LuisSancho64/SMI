using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMI.Server.Data;
using SMI.Shared.DTOs;
using SMI.Shared.Models;

namespace SMI.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SGISDbContext _context;

        public AuthController(SGISDbContext context)
        {
            _context = context;

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
                var usuarioDto = await _context.Usuario
                    .AsNoTracking()
                    .Where(u => u.Persona.Correo == loginDto.Correo &&
                                u.Clave == loginDto.Clave &&
                                u.Activo)
                    .Select(u => new UsuarioDto
                    {
                        Id = u.Id,
                        Activo = u.Activo,
                        Clave = u.Clave,
                        Persona = new PersonaDto
                        {
                            Id = u.Persona.id,
                            Correo = u.Persona.Correo,
                            Nombre = u.Persona.nombre,
                            Apellido = u.Persona.apellido,
                            FechaNacimiento = u.Persona.FechaNacimiento ?? DateTime.MinValue,
                            Id_Genero = u.Persona.id_Genero ?? 0,
                        }
                    })
                    .FirstOrDefaultAsync();

                if (usuarioDto == null)
                {
                    return Unauthorized("Usuario no encontrado o no activo.");
                }

                return Ok(usuarioDto);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al realizar el login: {ex.Message}");
            }
        }

    }
}
