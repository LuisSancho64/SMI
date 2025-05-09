using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SMI.Shared.Models;

namespace SMI.Client.Services
{
    public class PersonaService
    {
        private readonly HttpClient _httpClient;

        public PersonaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Obtener todos los usuarios
        public async Task<List<Persona>> GetUsuariosAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Persona>>("api/Personas");
        }

        // Crear un nuevo usuario
        public async Task CreateUsuarioAsync(Persona persona)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Personas", persona);
            response.EnsureSuccessStatusCode();
        }

        // Actualizar usuario
        public async Task UpdateUsuarioAsync(Persona persona)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Personas/{persona.id}", persona);
            response.EnsureSuccessStatusCode();
        }

        // Eliminar un usuario
        public async Task DeleteUsuarioAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Personas/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
