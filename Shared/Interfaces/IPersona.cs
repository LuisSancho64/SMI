using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMI.Shared.Models;

namespace SMI.Shared.Interfaces
{
    public interface IPersona
    {
        Task<int> CrearPersonaAsync(Persona persona);
        Task<List<TipoDocumento>> ObtenerTiposDocumentoAsync();
        Task<List<Persona>> ListarPersonasAsync();
    }
}
