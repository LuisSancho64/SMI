using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using SMI.Shared.DTOs;
using SMI.Shared.Interfaces;
using System.Net;

namespace SMI.Client.Services
{
    public class CiudadService : ICiudadService
    {
        private readonly HttpClient _httpClient;

        public CiudadService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ProvinciaDto>> ObtenerProvincias()
        {
            return await _httpClient.GetFromJsonAsync<List<ProvinciaDto>>("api/ciudad/provincias");
        }

        public async Task<List<CiudadDto>> ObtenerCiudadesPorProvincia(int idProvincia)
        {
            return await _httpClient.GetFromJsonAsync<List<CiudadDto>>($"api/ciudad/provincia/{idProvincia}");
        }

        public async Task<bool> GuardarLugaresResidencia(int idPersona, List<int> idCiudades)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/personalugarresidencia/persona/{idPersona}", idCiudades);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<CiudadDto>> ObtenerCiudadesPorPersona(int idPersona)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<CiudadDto>>($"api/personalugarresidencia/persona/{idPersona}");
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                // Si no encuentra registros, devolver lista vacía en lugar de error
                return new List<CiudadDto>();
            }
        }

        public async Task<CiudadDto> ObtenerCiudad(int idCiudad)
        {
            if (idCiudad <= 0)
            {
                return null;
            }

            try
            {
                return await _httpClient.GetFromJsonAsync<CiudadDto>($"api/ciudad/{idCiudad}");
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }
    }
}