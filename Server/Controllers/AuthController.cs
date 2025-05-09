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
                // Cambiar la condición para usar el valor booleano de "Activo"
                var user = await _context.Usuario
                    .FirstOrDefaultAsync(u => u.Correo == loginDto.Correo &&
                                              u.Clave == loginDto.Clave &&
                                              u.Activo); // Verifica si el usuario está activo

                if (user == null)
                {
                    return Unauthorized("Usuario no encontrado o no activo.");
                }

                return Ok(user);  // Puedes devolver un token o datos adicionales si es necesario
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al realizar el login: {ex.Message}");
            }
        }
    }
}
