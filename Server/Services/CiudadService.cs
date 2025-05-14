// En SMI.SERVER/Services/CiudadService.cs
using Microsoft.EntityFrameworkCore;
using SMI.Server.Data;
using SMI.Shared.DTOs;
using SMI.Shared.Interfaces;
using SMI.Shared.Models;

// SMI.Server/Services/CiudadService.cs
public class CiudadService : ICiudadService
{
    private readonly SGISDbContext _context;

    public CiudadService(SGISDbContext context)
    {
        _context = context;
    }

    public async Task<List<ProvinciaDto>> ObtenerProvincias()
    {
        return await _context.Provincias
            .Select(p => new ProvinciaDto { Id = p.id, Nombre = p.nombre })
            .ToListAsync();
    }

    public async Task<List<CiudadDto>> ObtenerCiudadesPorProvincia(int idProvincia)
    {
        return await _context.Ciudades
            .Where(c => c.id_Provincia == idProvincia)
            .Select(c => new CiudadDto { Id = c.id, IdProvincia = (int)c.id_Provincia, Nombre = c.nombre })
            .ToListAsync();
    }

    public async Task<bool> GuardarLugaresResidencia(int idPersona, List<int> idCiudades)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            // Eliminar existentes
            var existentes = await _context.PersonasLugaresResidencia
                .Where(plr => plr.id_Persona == idPersona)
                .ToListAsync();

            _context.PersonasLugaresResidencia.RemoveRange(existentes);

            // Agregar nuevos
            foreach (var idCiudad in idCiudades)
            {
                _context.PersonasLugaresResidencia.Add(new PersonaLugarResidencia
                {
                    id_Persona = idPersona,
                    id_Ciudad = idCiudad
                });
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<List<CiudadDto>> ObtenerCiudadesPorPersona(int idPersona)
    {
        return await _context.PersonasLugaresResidencia
            .Where(plr => plr.id_Persona == idPersona)
            .Include(plr => plr.Ciudad)
            .Select(plr => new CiudadDto
            {
                Id = plr.Ciudad.id,
                IdProvincia = (int)plr.Ciudad.id_Provincia,
                Nombre = plr.Ciudad.nombre
            })
            .ToListAsync();
    }

    public async Task<CiudadDto> ObtenerCiudad(int idCiudad)
    {
        return await _context.Ciudades
            .Where(c => c.id == idCiudad)
            .Select(c => new CiudadDto
            {
                Id = c.id,
                IdProvincia = (int)c.id_Provincia,
                Nombre = c.nombre
            })
            .FirstOrDefaultAsync();
    }
}