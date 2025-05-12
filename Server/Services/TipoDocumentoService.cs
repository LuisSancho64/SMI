using Microsoft.EntityFrameworkCore;
using SMI.Shared.Models;
using SMI.Shared.Interfaces;
using SMI.Server.Data;
using SMI.Shared.DTOs;

namespace SMI.Server.Services
{
    public class TipoDocumentoService : ITipoDocumentoService
    {
        private readonly SGISDbContext _context;

        public TipoDocumentoService(SGISDbContext context)
        {
            _context = context;
        }

        public async Task<List<TipoDocumentoDto>> GetTiposDocumentoAsync()
        {
            // Obtienes los tipos de documentos desde la base de datos
            var tipos = await _context.TipoDocumentos.ToListAsync();

            // Mapeas los resultados a los DTOs
            var tiposDto = tipos.Select(t => new TipoDocumentoDto
            {
                Id = t.id,
                Nombre = t.nombre
            }).ToList();

            return tiposDto;
        }
    }
}
