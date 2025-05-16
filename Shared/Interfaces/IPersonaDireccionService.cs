using SMI.Shared.DTOs;

namespace SMI.Shared.Interfaces
{
    public interface IPersonaDireccionService
    {
        Task<PersonaDireccionDto> ObtenerPorPersona(int idPersona);
        Task<bool> GuardarDireccion(int idPersona, PersonaDireccionDto direccion);
    }
}
