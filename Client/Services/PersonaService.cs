using SMI.Shared.Models;
using System.Net.Http.Json;
using SMI.Shared.DTOs;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SMI.Client.Services
{
    public class PersonaService
    {
        private readonly HttpClient _httpClient;

        public PersonaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // ------------------ CRUD de Personas ------------------

        public async Task<List<PersonaDto>> GetUsuariosAsync()
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<List<PersonaDto>>("api/personas");
                return result ?? new List<PersonaDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener usuarios: {ex.Message}");
                return new List<PersonaDto>();
            }
        }

        public async Task<PersonaDto> GetUsuarioAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<PersonaDto>($"api/personas/{id}") ?? new PersonaDto();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener usuario: {ex.Message}");
                return new PersonaDto();
            }
        }

        public async Task<PersonaDto> CreateUsuarioAsync(PersonaDto persona)
        {
            try
            {
                // Asegurarse de que la lista de documentos no sea null
                if (persona.Documentos == null)
                {
                    persona.Documentos = new List<PersonaDocumentoDto>();
                }

                var response = await _httpClient.PostAsJsonAsync("api/personas", persona);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<PersonaDto>();
                }

                // Para depuración
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error al crear persona: {errorContent}");
                Console.WriteLine($"Datos enviados: {System.Text.Json.JsonSerializer.Serialize(persona)}");

                response.EnsureSuccessStatusCode();
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción al crear persona: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateUsuarioAsync(int id, PersonaDto usuario)
        {
            try
            {
                // Asegurarse de que la lista de documentos no sea null
                if (usuario.Documentos == null)
                {
                    usuario.Documentos = new List<PersonaDocumentoDto>();
                }

                var response = await _httpClient.PutAsJsonAsync($"api/personas/{id}", usuario);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error al actualizar persona: {errorContent}");
                }

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar el usuario: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteUsuarioAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/personas/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar el usuario: {ex.Message}");
                return false;
            }
        }

        // ------------------ Tipos de Documento ------------------

        public async Task<List<TipoDocumentoDto>> GetTiposDocumentoAsync()
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<List<TipoDocumentoDto>>("api/tipodocumento");
                return result ?? new List<TipoDocumentoDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los tipos de documento: {ex.Message}");
                return new List<TipoDocumentoDto>();
            }
        }
    }
}