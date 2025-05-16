using SMI.Shared.DTOs;
using SMI.Shared.Interfaces;
using System.Net.Http.Json;

namespace SMI.Client.Services
{
    public class PersonaDireccionService : IPersonaDireccionService
    {
        private readonly HttpClient _httpClient;

        public PersonaDireccionService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:5095/"); // Asegúrate que coincida con tu backend
        }

        public async Task<PersonaDireccionDto> ObtenerPorPersona(int idPersona)
        {
            return await _httpClient.GetFromJsonAsync<PersonaDireccionDto>($"api/personadireccion/{idPersona}/direccion");
        }


        public async Task<bool> GuardarDireccion(int idPersona, PersonaDireccionDto direccion)
        {
            try
            {
                // Asegurar que el IdPersona esté correctamente asignado
                direccion.IdPersona = idPersona;

                var response = await _httpClient.PostAsJsonAsync($"api/personadireccion/{idPersona}/direccion", direccion);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error al guardar dirección: {errorContent}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GuardarDireccion: {ex.ToString()}");
                return false;
            }
        }
    }
}
