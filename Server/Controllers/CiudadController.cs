// SMI.Server/Controllers/CiudadController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMI.Server.Data;
using SMI.Shared.DTOs;
using SMI.Shared.Interfaces;
using System.Net;

[ApiController]
[Route("api/[controller]")]
public class CiudadController : ControllerBase
{
    private readonly ICiudadService _ciudadService;
    private readonly SGISDbContext _context;

    public CiudadController(ICiudadService ciudadService, SGISDbContext context)
    {
        _ciudadService = ciudadService;
        _context = context;
    }

    /// <summary>
    /// Obtiene todas las provincias disponibles
    /// </summary>
    [HttpGet("provincias")]
    [ProducesResponseType(typeof(List<ProvinciaDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<List<ProvinciaDto>>> GetProvincias()
    {
        var provincias = await _ciudadService.ObtenerProvincias();
        if (provincias == null || !provincias.Any())
        {
            return NotFound("No se encontraron provincias");
        }
        return Ok(provincias);
    }

    /// <summary>
    /// Obtiene las ciudades de una provincia específica
    /// </summary>
    /// <param name="idProvincia">ID de la provincia</param>
    [HttpGet("provincia/{idProvincia}")]
    [ProducesResponseType(typeof(List<CiudadDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<List<CiudadDto>>> GetCiudadesPorProvincia(int idProvincia)
    {
        if (idProvincia <= 0)
        {
            return BadRequest("ID de provincia inválido");
        }

        var ciudades = await _ciudadService.ObtenerCiudadesPorProvincia(idProvincia);
        if (ciudades == null || !ciudades.Any())
        {
            return NotFound($"No se encontraron ciudades para la provincia con ID {idProvincia}");
        }

        return Ok(ciudades);
    }

    /// <summary>
    /// Obtiene una ciudad específica por su ID
    /// </summary>
    /// <param name="id">ID de la ciudad</param>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CiudadDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<CiudadDto>> GetCiudad(int id)
    {
        if (id <= 0)
        {
            return BadRequest("ID de ciudad inválido");
        }

        var ciudad = await _context.Ciudades
            .Include(c => c.Provincia)
            .Where(c => c.id == id)
            .Select(c => new CiudadDto
            {
                Id = c.id,
                IdProvincia = (int)c.id_Provincia,
                Nombre = c.nombre
            })
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (ciudad == null)
        {
            return NotFound($"No se encontró la ciudad con ID {id}");
        }

        return Ok(ciudad);
    }

    /// <summary>
    /// Obtiene todas las ciudades disponibles
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<CiudadDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<List<CiudadDto>>> GetAllCiudades()
    {
        var ciudades = await _context.Ciudades
            .Include(c => c.Provincia)
            .Select(c => new CiudadDto
            {
                Id = c.id,
                IdProvincia = (int)c.id_Provincia,
                Nombre = c.nombre
            })
            .AsNoTracking()
            .ToListAsync();

        if (ciudades == null || !ciudades.Any())
        {
            return NotFound("No se encontraron ciudades");
        }

        return Ok(ciudades);
    }
}