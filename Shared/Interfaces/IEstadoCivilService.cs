using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMI.Shared.DTOs;

namespace SMI.Shared.Interfaces
{
    public interface IEstadoCivilService
    {
        Task<List<EstadoCivilDto>> ObtenerTodos();
        Task<bool> GuardarEstadoCivilPersona(int idPersona, int idEstadoCivil);
        Task<int?> ObtenerEstadoCivilDePersona(int idPersona);
    }
}
