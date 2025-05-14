using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMI.Shared.DTOs;

namespace SMI.Shared.Interfaces
{
    // SMI.Shared/Interfaces/ICiudadService.cs
    public interface ICiudadService
    {
        Task<List<ProvinciaDto>> ObtenerProvincias();
        Task<List<CiudadDto>> ObtenerCiudadesPorProvincia(int idProvincia);
        Task<bool> GuardarLugaresResidencia(int idPersona, List<int> idCiudades);
        Task<List<CiudadDto>> ObtenerCiudadesPorPersona(int idPersona);
        Task<CiudadDto> ObtenerCiudad(int idCiudad);
    }
}
