using SMI.Shared.Models;
using SMI.Shared.DTOs;
using SMI.Shared.Interfaces;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMI.Client.Services
{
    public class TipoDocumentoService : ITipoDocumentoService
    {
        private readonly HttpClient _http;

        public TipoDocumentoService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<TipoDocumentoDto>> GetTiposDocumentoAsync()
        {
            var result = await _http.GetFromJsonAsync<List<TipoDocumentoDto>>("api/tipodocumento");
            return result ?? new List<TipoDocumentoDto>();
        }
    }
}