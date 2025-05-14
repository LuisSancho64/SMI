using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMI.Server.Data;
using SMI.Shared.Interfaces;
using SMI.Shared.Models;

namespace SMI.Server.Controllers
{
    // SMI.Server/Controllers/PersonaEstadoCivilController.cs
    [ApiController]
    [Route("api/[controller]")]
    public class PersonaEstadoCivilController : ControllerBase
    {
        private readonly SGISDbContext _context;

        public PersonaEstadoCivilController(SGISDbContext context)
        {
            _context = context;
        }

        [HttpGet("{idPersona}")]
        public async Task<ActionResult<int?>> Get(int idPersona)
        {
            try
            {
                var estadoCivil = await _context.PersonasEstadosCiviles
                    .Where(pec => pec.id_Persona == idPersona)
                    .Select(pec => pec.id_EstadoCivil)
                    .FirstOrDefaultAsync();

                return Ok(estadoCivil);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en PersonaEstadoCivilController.Get: {ex}");
                return StatusCode(500, "Error interno al obtener estado civil");
            }
        }

        [HttpPost("{idPersona}")]
        public async Task<IActionResult> Save(int idPersona, [FromBody] int idEstadoCivil)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Eliminar existente si existe
                var existente = await _context.PersonasEstadosCiviles
                    .FirstOrDefaultAsync(pec => pec.id_Persona == idPersona);

                if (existente != null)
                {
                    _context.PersonasEstadosCiviles.Remove(existente);
                }

                // Crear nueva relación
                var nuevaRelacion = new PersonaEstadoCivil
                {
                    id_Persona = idPersona,
                    id_EstadoCivil = idEstadoCivil
                };

                _context.PersonasEstadosCiviles.Add(nuevaRelacion);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"Error en PersonaEstadoCivilController.Save: {ex}");
                return StatusCode(500, "Error interno al guardar estado civil");
            }
        }
    }
}
