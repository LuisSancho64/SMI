using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMI.Server.Data;
using SMI.Shared.DTOs;
using SMI.Shared.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMI.Server.Controllers
{
    // SMI.Server/Controllers/PersonaLugarResidenciaController.cs
    [Route("api/[controller]")]
    [ApiController]
    public class PersonaLugarResidenciaController : ControllerBase
    {
        private readonly SGISDbContext _context;

        public PersonaLugarResidenciaController(SGISDbContext context)
        {
            _context = context;
        }

        [HttpGet("persona/{personaId}")]
        public async Task<ActionResult<List<CiudadDto>>> GetByPersonaId(int personaId)
        {
            var ciudades = await _context.PersonasLugaresResidencia
                .Where(plr => plr.id_Persona == personaId)
                .Include(plr => plr.Ciudad)
                .ThenInclude(c => c.Provincia)
                .Select(plr => new CiudadDto
                {
                    Id = plr.Ciudad.id,
                    Nombre = plr.Ciudad.nombre,
                    IdProvincia = (int)plr.Ciudad.id_Provincia,
                    NombreProvincia = plr.Ciudad.Provincia.nombre
                })
                .ToListAsync();

            return Ok(ciudades);
        }

        [HttpPost("persona/{personaId}")]
        public async Task<IActionResult> SaveForPerson(int personaId, [FromBody] List<int> ciudadIds)
        {
            try
            {
                // Eliminar existentes
                var existentes = await _context.PersonasLugaresResidencia
                    .Where(plr => plr.id_Persona == personaId)
                    .ToListAsync();

                if (existentes.Any())
                {
                    _context.PersonasLugaresResidencia.RemoveRange(existentes);
                }

                // Agregar nuevos
                foreach (var ciudadId in ciudadIds)
                {
                    _context.PersonasLugaresResidencia.Add(new PersonaLugarResidencia
                    {
                        id_Persona = personaId,
                        id_Ciudad = ciudadId
                    });
                }

                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al guardar: {ex.Message}");
            }
        }
    }
}