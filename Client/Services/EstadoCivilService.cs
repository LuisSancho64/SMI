using SMI.Shared.DTOs;
using SMI.Shared.Interfaces;
using System.Net;
using System.Net.Http.Json;

namespace SMI.Client.Services
{
    public class EstadoCivilService : IEstadoCivilService
    {
        private readonly HttpClient _httpClient;

        public EstadoCivilService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<EstadoCivilDto>> ObtenerTodos()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<EstadoCivilDto>>("api/estadocivil");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error al obtener estados civiles: {ex.Message}");
                return new List<EstadoCivilDto>();
            }
        }

        public async Task<bool> GuardarEstadoCivilPersona(int idPersona, int idEstadoCivil)
        {
            var response = await _httpClient.PostAsJsonAsync(
                $"api/personaestadocivil/{idPersona}",
                idEstadoCivil);
            return response.IsSuccessStatusCode;
        }

        public async Task<int?> ObtenerEstadoCivilDePersona(int idPersona)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<int?>($"api/personaestadocivil/{idPersona}");
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener estado civil de persona: {ex.Message}");
                return null;
            }
        }
    }
}
