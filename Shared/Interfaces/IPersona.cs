using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMI.Shared.DTOs;
using SMI.Shared.Models;

namespace SMI.Shared.Interfaces
{
    public interface IPersona
    {
        Task<int> CrearPersonaAsync(Persona persona);
        Task<List<TipoDocumento>> ObtenerTiposDocumentoAsync();
        Task<List<Persona>> ListarPersonasAsync();

        // Métodos para manejar direcciones
        Task<Persona> ObtenerPersonaConDireccionAsync(int id);
        Task<bool> GuardarDireccionPersonaAsync(PersonaDireccion direccion);
        Task<bool> ActualizarDireccionPersonaAsync(PersonaDireccion direccion);
        Task<PersonaDireccion> ObtenerDireccionPorPersonaIdAsync(int personaId);
        Task<PersonaDireccionDto> ObtenerDireccionPersona(int personaId);

    }
}
