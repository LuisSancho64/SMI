using SMI.Shared.DTOs;
using SMI.Shared.Interfaces;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;

    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> Login(LoginDTO loginDto)
    {
        // Haces la solicitud POST al servidor para autenticar
        var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginDto);

        // Si la respuesta es exitosa, regresa true
        return response.IsSuccessStatusCode;
    }
}
