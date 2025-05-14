using Microsoft.EntityFrameworkCore;
using SMI.Server.Data;
using SMI.Shared.DTOs;
using SMI.Shared.Interfaces;
using SMI.Shared.Models;

namespace SMI.Server.Services
{
    public class EstadoCivilService : IEstadoCivilService
    {
        private readonly SGISDbContext _context;

        public EstadoCivilService(SGISDbContext context)
        {
            _context = context;
        }

        public async Task<List<EstadoCivilDto>> ObtenerTodos()
        {
            return await _context.EstadosCiviles
                .Select(ec => new EstadoCivilDto
                {
                    id = ec.id,
                    nombre = ec.nombre
                })
                .ToListAsync();
        }

        public async Task<bool> GuardarEstadoCivilPersona(int idPersona, int idEstadoCivil)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Eliminar relación existente si existe
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

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<int?> ObtenerEstadoCivilDePersona(int idPersona)
        {
            var relacion = await _context.PersonasEstadosCiviles
                .FirstOrDefaultAsync(pec => pec.id_Persona == idPersona);

            return relacion?.id_EstadoCivil;
        }
    }
}
