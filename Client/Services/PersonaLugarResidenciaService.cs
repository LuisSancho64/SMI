using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SMI.Shared.DTOs;

namespace SMI.Client.Services
{
    public class PersonaLugarResidenciaService
    {
        private readonly HttpClient _httpClient;

        public PersonaLugarResidenciaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PersonaLugarResidenciaDto> GetByPersonaIdAsync(int personaId)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<PersonaLugarResidenciaDto>($"api/personalugarresidencia/persona/{personaId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener lugar de residencia: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> SaveLugarResidenciaAsync(PersonaLugarResidenciaDto lugarResidencia)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/personalugarresidencia", lugarResidencia);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar lugar de residencia: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteLugarResidenciaAsync(int personaId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/personalugarresidencia/persona/{personaId}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar lugar de residencia: {ex.Message}");
                return false;
            }
        }
    }
}