using System.Collections.Generic;
using System.Threading.Tasks;
using SMI.Shared.Models;
using SMI.Shared.DTOs;

namespace SMI.Shared.Interfaces
{
    public interface ITipoDocumentoService
    {
        Task<List<TipoDocumentoDto>> GetTiposDocumentoAsync();
    }

}