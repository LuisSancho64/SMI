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
    public class UserController : ControllerBase
    {
        private readonly SGISDbContext _context;
        private readonly IPasswordService _passwordService;

        public UserController(SGISDbContext context, IPasswordService passwordService)
        {
            _context = context;
            _passwordService = passwordService;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioDto>>> GetUsuarios()
        {
            var usuarios = await _context.Usuario
                .Include(u => u.Persona)
                .Select(u => new UsuarioDto
                {
                    Id = u.Id,
                    Activo = u.Activo,
                    // No incluir la contraseña en la respuesta
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
                .ToListAsync();

            return Ok(usuarios);
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDto>> GetUsuario(int id)
        {
            var usuario = await _context.Usuario
                .Include(u => u.Persona)
                .Where(u => u.Id == id)
                .Select(u => new UsuarioDto
                {
                    Id = u.Id,
                    Activo = u.Activo,
                    // No incluir la contraseña en la respuesta
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

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        // POST: api/User
        [HttpPost]
        public async Task<ActionResult<UsuarioDto>> CreateUsuario(UsuarioCreateDto usuarioDto)
        {
            // Verificar si la persona existe
            var persona = await _context.Personas.FindAsync(usuarioDto.Id_Persona);
            if (persona == null)
            {
                return BadRequest("La persona especificada no existe");
            }

            // Verificar si ya existe un usuario para esta persona
            var usuarioExistente = await _context.Usuario.AnyAsync(u => u.Id_Persona == usuarioDto.Id_Persona);
            if (usuarioExistente)
            {
                return BadRequest("Ya existe un usuario para esta persona");
            }

            // Crear el nuevo usuario con la contraseña hasheada
            var usuario = new User
            {
                Id_Persona = usuarioDto.Id_Persona,
                Clave = _passwordService.HashPassword(usuarioDto.Clave),
                Activo = usuarioDto.Activo
            };

            _context.Usuario.Add(usuario);
            await _context.SaveChangesAsync();

            // Crear el DTO de respuesta
            var nuevoUsuarioDto = new UsuarioDto
            {
                Id = usuario.Id,
                Activo = usuario.Activo,
                Persona = new PersonaDto
                {
                    Id = persona.id,
                    Correo = persona.Correo,
                    Nombre = persona.nombre,
                    Apellido = persona.apellido,
                    FechaNacimiento = persona.FechaNacimiento ?? DateTime.MinValue,
                    Id_Genero = persona.id_Genero ?? 0,
                }
            };

            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, nuevoUsuarioDto);
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUsuario(int id, UsuarioUpdateDto usuarioDto)
        {
            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            // Actualizar solo los campos permitidos
            usuario.Activo = usuarioDto.Activo;

            // Solo actualizar la contraseña si se proporciona una nueva
            if (!string.IsNullOrEmpty(usuarioDto.Clave))
            {
                usuario.Clave = _passwordService.HashPassword(usuarioDto.Clave);
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            // En lugar de eliminar físicamente, puedes desactivar el usuario
            usuario.Activo = false;
            _context.Entry(usuario).State = EntityState.Modified;

            // O si prefieres eliminarlo físicamente:
            // _context.Usuario.Remove(usuario);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/User/cambiar-contrasena
        [HttpPost("cambiar-contrasena")]
        public async Task<IActionResult> CambiarContrasena(CambioContrasenaDto cambioDto)
        {
            var usuario = await _context.Usuario.FindAsync(cambioDto.UsuarioId);
            if (usuario == null)
            {
                return NotFound("Usuario no encontrado");
            }

            // Verificar la contraseña actual
            if (!_passwordService.VerifyPassword(cambioDto.ContrasenaActual, usuario.Clave))
            {
                return BadRequest("La contraseña actual es incorrecta");
            }

            // Actualizar a la nueva contraseña
            usuario.Clave = _passwordService.HashPassword(cambioDto.NuevaContrasena);
            _context.Entry(usuario).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok("Contraseña actualizada correctamente");
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuario.Any(e => e.Id == id);
        }
    }
}