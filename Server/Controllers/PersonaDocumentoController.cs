using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMI.Server.Data;
using SMI.Shared.DTOs;
using SMI.Shared.Models;

namespace SMI.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonaDocumentoController : ControllerBase
    {
        private readonly SGISDbContext _context;

        public PersonaDocumentoController(SGISDbContext context)
        {
            _context = context;
        }

        [HttpPost("{personaId}")]
        public async Task<IActionResult> SaveDocumento(int personaId, [FromBody] PersonaDocumentoDto documentoDto)
        {
            if (documentoDto == null || string.IsNullOrWhiteSpace(documentoDto.NumeroDocumento))
            {
                return BadRequest("Documento inválido.");
            }

            var persona = await _context.Personas
                .Include(p => p.Documentos)
                .FirstOrDefaultAsync(p => p.id == personaId);

            if (persona == null)
            {
                return NotFound("Persona no encontrada.");
            }

            var documentoExistente = persona.Documentos
                .FirstOrDefault(d => d.id_TipoDocumento == documentoDto.TipoDocumentoId);

            if (documentoExistente != null)
            {
                documentoExistente.numeroDocumento = documentoDto.NumeroDocumento;
            }
            else
            {
                persona.Documentos.Add(new PersonaDocumento
                {
                    id_Persona = personaId,
                    id_TipoDocumento = documentoDto.TipoDocumentoId,
                    numeroDocumento = documentoDto.NumeroDocumento
                });
            }

            await _context.SaveChangesAsync();
            return Ok("Documento guardado correctamente.");
        }
    }
}
