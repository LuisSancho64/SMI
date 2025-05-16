using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMI.Server.Data;
using SMI.Server.Services;
using SMI.Shared.DTOs;
using SMI.Shared.Models;

[Route("api/[controller]")]
[ApiController]
public class PersonasController : ControllerBase
{
    private readonly PersonaService _personaService;
    private readonly SGISDbContext _context;

    public PersonasController(PersonaService personaService, SGISDbContext context)
    {
        _personaService = personaService;
        _context = context;
    }

    // GET: api/Personas
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PersonaDto>>> GetPersonas()
    {
        var personas = await _context.Personas
            .Include(p => p.Documentos)
                .ThenInclude(pd => pd.TipoDocumento)
            .Include(p => p.EstadosCiviles)
                .ThenInclude(pe => pe.EstadoCivil)
            .Include(p => p.Direccion) // Incluir dirección
            .ToListAsync();

        var personasDto = personas.Select(p => new PersonaDto
        {
            Id = p.id,
            Nombre = p.nombre,
            Apellido = p.apellido,
            Id_Genero = p.id_Genero,
            FechaNacimiento = p.FechaNacimiento,
            Correo = p.Correo,
            EstadoCivilNombre = p.EstadosCiviles.FirstOrDefault()?.EstadoCivil?.nombre,
            Documentos = p.Documentos.Select(pd => new PersonaDocumentoDto
            {
                TipoDocumentoId = pd.id_TipoDocumento,
                NumeroDocumento = pd.numeroDocumento,
                TipoDocumentoNombre = pd.TipoDocumento?.nombre
            }).ToList(),
            Direccion = p.Direccion != null ? new PersonaDireccionDto
            {
                IdPersona = p.Direccion.id_Persona,
                CallePrincipal = p.Direccion.callePrincipal,
                CalleSecundaria1 = p.Direccion.calleSecundaria1,
                CalleSecundaria2 = p.Direccion.calleSecundaria2,
                NumeroCasa = p.Direccion.numeroCasa,
                Referencia = p.Direccion.referencia
            } : null
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
            .Include(p => p.EstadosCiviles)
                .ThenInclude(pe => pe.EstadoCivil)
            .Include(p => p.Direccion) // Incluir dirección
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
            EstadoCivilNombre = persona.EstadosCiviles.FirstOrDefault()?.EstadoCivil?.nombre,
            Documentos = persona.Documentos.Select(pd => new PersonaDocumentoDto
            {
                TipoDocumentoId = pd.id_TipoDocumento,
                NumeroDocumento = pd.numeroDocumento,
                TipoDocumentoNombre = pd.TipoDocumento?.nombre
            }).ToList(),
            Direccion = persona.Direccion != null ? new PersonaDireccionDto
            {
                IdPersona = persona.Direccion.id_Persona,
                CallePrincipal = persona.Direccion.callePrincipal,
                CalleSecundaria1 = persona.Direccion.calleSecundaria1,
                CalleSecundaria2 = persona.Direccion.calleSecundaria2,
                NumeroCasa = persona.Direccion.numeroCasa,
                Referencia = persona.Direccion.referencia
            } : null
        };

        return personaDto;
    }

    // PUT: api/Personas/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPersona(int id, PersonaDto personaDto)
    {
        if (id != personaDto.Id)
            return BadRequest();

        try
        {
            var personaActualizada = await _personaService.ActualizarPersonaConDireccion(personaDto);
            return Ok(personaActualizada);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error al actualizar persona: {ex.Message}");
        }
    }

    // POST: api/Personas
    [HttpPost]
    public async Task<ActionResult<PersonaDto>> PostPersona(PersonaDto personaDto)
    {
        try
        {
            var personaCreada = await _personaService.CrearPersonaConDireccion(personaDto);
            return CreatedAtAction("GetPersona", new { id = personaCreada.Id }, personaCreada);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error al crear persona: {ex.Message}");
        }
    }

    // DELETE: api/Personas/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePersona(int id)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            // Eliminar documentos
            var documentos = await _context.PersonaDocumentos
                .Where(pd => pd.id_Persona == id)
                .ToListAsync();
            _context.PersonaDocumentos.RemoveRange(documentos);

            // Eliminar dirección
            var direccion = await _context.PersonaDirecciones
                .FirstOrDefaultAsync(d => d.id_Persona == id);
            if (direccion != null)
            {
                _context.PersonaDirecciones.Remove(direccion);
            }

            // Eliminar persona
            var persona = await _context.Personas.FindAsync(id);
            if (persona == null)
                return NotFound();

            _context.Personas.Remove(persona);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return StatusCode(500, $"Error al eliminar persona: {ex.Message}");
        }
    }

    [HttpGet("{id}/direccion")]
    public async Task<ActionResult<PersonaDireccionDto>> GetDireccionPersona(int id)
    {
        var direccion = await _context.PersonaDirecciones
            .FirstOrDefaultAsync(d => d.id_Persona == id);

        if (direccion == null)
        {
            return Ok(new PersonaDireccionDto()); // Retorna DTO vacío en lugar de 404
        }

        return new PersonaDireccionDto
        {
            IdPersona = direccion.id_Persona,
            CallePrincipal = direccion.callePrincipal,
            CalleSecundaria1 = direccion.calleSecundaria1,
            CalleSecundaria2 = direccion.calleSecundaria2,
            NumeroCasa = direccion.numeroCasa,
            Referencia = direccion.referencia
        };
    }

    [HttpPost("direccion")]
    public async Task<IActionResult> GuardarDireccion([FromBody] PersonaDireccionDto direccionDto)
    {
        if (direccionDto.IdPersona == 0)
            return BadRequest("Id de persona inválido");

        var direccion = await _context.PersonaDirecciones
            .FirstOrDefaultAsync(d => d.id_Persona == direccionDto.IdPersona);

        if (direccion == null)
        {
            // Crear nueva dirección
            direccion = new PersonaDireccion
            {
                id_Persona = direccionDto.IdPersona,
                callePrincipal = direccionDto.CallePrincipal,
                calleSecundaria1 = direccionDto.CalleSecundaria1,
                calleSecundaria2 = direccionDto.CalleSecundaria2,
                numeroCasa = direccionDto.NumeroCasa,
                referencia = direccionDto.Referencia
            };
            _context.PersonaDirecciones.Add(direccion);
        }
        else
        {
            // Actualizar dirección existente
            direccion.callePrincipal = direccionDto.CallePrincipal;
            direccion.calleSecundaria1 = direccionDto.CalleSecundaria1;
            direccion.calleSecundaria2 = direccionDto.CalleSecundaria2;
            direccion.numeroCasa = direccionDto.NumeroCasa;
            direccion.referencia = direccionDto.Referencia;
            _context.PersonaDirecciones.Update(direccion);
        }

        await _context.SaveChangesAsync();
        return Ok(true);
    }

    private bool PersonaExists(int id)
    {
        return _context.Personas.Any(e => e.id == id);
    }
}