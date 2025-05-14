using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMI.Server.Data;
using SMI.Shared.DTOs;
using SMI.Shared.Models;

[Route("api/[controller]")]
[ApiController]
public class PersonasController : ControllerBase
{
    private readonly SGISDbContext _context;

    public PersonasController(SGISDbContext context)
    {
        _context = context;
    }

    // GET: api/Personas
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PersonaDto>>> GetPersonas()
    {
        var personas = await _context.Personas
            .Include(p => p.Documentos)
                .ThenInclude(pd => pd.TipoDocumento)
            .ToListAsync();

        var personasDto = personas.Select(p => new PersonaDto
        {
            Id = p.id,
            Nombre = p.nombre,
            Apellido = p.apellido,
            Id_Genero = p.id_Genero,
            FechaNacimiento = p.FechaNacimiento,
            Correo = p.Correo,
            Documentos = p.Documentos.Select(pd => new PersonaDocumentoDto
            {
                TipoDocumentoId = pd.id_TipoDocumento,
                NumeroDocumento = pd.numeroDocumento,
                TipoDocumentoNombre = pd.TipoDocumento?.nombre  // Aquí se asigna correctamente el nombre del tipo de documento
            }).ToList()
        }).ToList();

        return Ok(personasDto);
    }

    // GET: api/Personas/5
    [HttpGet("{id}")]
    public async Task<ActionResult<PersonaDto>> GetPersona(int id)
    {
        var persona = await _context.Personas
            .Include(p => p.Documentos)
                .ThenInclude(pd => pd.TipoDocumento)
            .FirstOrDefaultAsync(p => p.id == id);

        if (persona == null)
            return NotFound();

        var personaDto = new PersonaDto
        {
            Id = persona.id,
            Nombre = persona.nombre,
            Apellido = persona.apellido,
            Id_Genero = persona.id_Genero,
            FechaNacimiento = persona.FechaNacimiento,
            Correo = persona.Correo,
            Documentos = persona.Documentos.Select(pd => new PersonaDocumentoDto
            {
                TipoDocumentoId = pd.id_TipoDocumento,
                NumeroDocumento = pd.numeroDocumento,
                TipoDocumentoNombre = pd.TipoDocumento?.nombre  // Asignación de nombre correctamente
            }).ToList()
        };

        return personaDto;
    }
    // PUT: api/Personas/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPersona(int id, PersonaDto personaDto)
    {
        if (id != personaDto.Id)
            return BadRequest();

        var persona = await _context.Personas
            .Include(p => p.Documentos)
            .FirstOrDefaultAsync(p => p.id == id);

        if (persona == null)
            return NotFound();

        persona.nombre = personaDto.Nombre;
        persona.apellido = personaDto.Apellido;
        persona.id_Genero = personaDto.Id_Genero;
        persona.FechaNacimiento = personaDto.FechaNacimiento;
        persona.Correo = personaDto.Correo;

        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            _context.Entry(persona).State = EntityState.Modified;

            // Eliminar documentos actuales
            _context.PersonaDocumentos.RemoveRange(persona.Documentos);

            // Agregar los nuevos
            if (personaDto.Documentos != null)
            {
                foreach (var docDto in personaDto.Documentos)
                {
                    _context.PersonaDocumentos.Add(new PersonaDocumento
                    {
                        id_Persona = id,
                        id_TipoDocumento = docDto.TipoDocumentoId,
                        numeroDocumento = docDto.NumeroDocumento
                    });
                }
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }

        return NoContent();
    }


    // POST: api/Personas
    [HttpPost]
    public async Task<ActionResult<PersonaDto>> PostPersona(PersonaDto personaDto)
    {
        var persona = new Persona
        {
            nombre = personaDto.Nombre,
            apellido = personaDto.Apellido,
            id_Genero = personaDto.Id_Genero,
            FechaNacimiento = personaDto.FechaNacimiento,
            Correo = personaDto.Correo
        };

        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            _context.Personas.Add(persona);
            await _context.SaveChangesAsync();

            if (personaDto.Documentos != null)
            {
                foreach (var docDto in personaDto.Documentos)
                {
                    _context.PersonaDocumentos.Add(new PersonaDocumento
                    {
                        id_Persona = persona.id,
                        id_TipoDocumento = docDto.TipoDocumentoId,
                        numeroDocumento = docDto.NumeroDocumento
                    });
                }

                await _context.SaveChangesAsync();
            }

            await transaction.CommitAsync();
            personaDto.Id = persona.id;

            return CreatedAtAction("GetPersona", new { id = persona.id }, personaDto);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    // DELETE: api/Personas/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePersona(int id)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var documentos = await _context.PersonaDocumentos
                .Where(pd => pd.id_Persona == id)
                .ToListAsync();

            _context.PersonaDocumentos.RemoveRange(documentos);

            var persona = await _context.Personas.FindAsync(id);
            if (persona == null)
                return NotFound();

            _context.Personas.Remove(persona);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return NoContent();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    private bool PersonaExists(int id)
    {
        return _context.Personas.Any(e => e.id == id);
    }
}
