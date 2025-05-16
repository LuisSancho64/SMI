using Microsoft.EntityFrameworkCore;
using SMI.Shared.Models;
using SMI.Shared.Interfaces;
using SMI.Server.Data;
using SMI.Shared.DTOs;

namespace SMI.Server.Services
{
    public class PersonaRepository : IPersona
    {
        private readonly SGISDbContext _context;

        public PersonaRepository(SGISDbContext context)
        {
            _context = context;
        }

        public async Task<int> CrearPersonaAsync(Persona persona)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                _context.Personas.Add(persona);
                await _context.SaveChangesAsync();

                // Si la persona tiene dirección asociada, la guardamos
                if (persona.Direccion != null)
                {
                    persona.Direccion.id_Persona = persona.id;
                    _context.PersonaDirecciones.Add(persona.Direccion);
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                return persona.id;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<List<TipoDocumento>> ObtenerTiposDocumentoAsync()
        {
            return await _context.TipoDocumentos.ToListAsync();
        }

        public async Task<List<Persona>> ListarPersonasAsync()
        {
            return await _context.Personas
                .Include(p => p.Documentos)
                .ThenInclude(d => d.TipoDocumento)
                .Include(p => p.Direccion)
                .ToListAsync();
        }

        public async Task<Persona> ObtenerPersonaConDireccionAsync(int id)
        {
            return await _context.Personas
                .Include(p => p.Documentos)
                .ThenInclude(d => d.TipoDocumento)
                .Include(p => p.Direccion)
                .FirstOrDefaultAsync(p => p.id == id);
        }

        public async Task<bool> GuardarDireccionPersonaAsync(PersonaDireccion direccion)
        {
            _context.PersonaDirecciones.Add(direccion);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ActualizarDireccionPersonaAsync(PersonaDireccion direccion)
        {
            _context.PersonaDirecciones.Update(direccion);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<PersonaDireccion> ObtenerDireccionPorPersonaIdAsync(int personaId)
        {
            return await _context.PersonaDirecciones
                .FirstOrDefaultAsync(d => d.id_Persona == personaId);
        }

        // Nuevo método para implementar la interfaz
        public async Task<PersonaDireccionDto> ObtenerDireccionPersona(int personaId)
        {
            var direccion = await _context.PersonaDirecciones
                .FirstOrDefaultAsync(d => d.id_Persona == personaId);

            if (direccion == null)
                return null;

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
    }
}