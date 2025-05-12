using Microsoft.EntityFrameworkCore;
using SMI.Shared.Models;
using SMI.Shared.Interfaces;
using SMI.Server.Data;

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
            _context.Personas.Add(persona);
            await _context.SaveChangesAsync(); // Aquí se genera el id por la base de datos
            return persona.id;
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
                .ToListAsync();
        }
    }
}
