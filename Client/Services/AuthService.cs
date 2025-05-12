// SMI.Client/Services/AuthService.cs
using System.Net.Http.Json;
using SMI.Shared.DTOs;
using SMI.Shared.Interfaces;
using SMI.Client.Auth;

namespace SMI.Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly CustomAuthStateProvider _customAuthStateProvider;
        private readonly LocalStorageService _localStorage;

        public AuthService(HttpClient httpClient, CustomAuthStateProvider customAuthStateProvider, LocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _customAuthStateProvider = customAuthStateProvider;
        }

        public async Task<LoginResponseDto> Login(LoginDTO loginDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginDto);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();

                if (result != null)
                {
                    // Guardar token y datos del usuario en localStorage
                    await _localStorage.SetItemAsync("authToken", result.Token);
                    await _localStorage.SetItemAsync("userData", result.Usuario);

                    // Notificar al proveedor de autenticación para actualizar el estado
                    _customAuthStateProvider.NotifyUserAuthentication(result.Token);

                    return result;
                }
            }

            return null;
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            await _localStorage.RemoveItemAsync("userData");

            // Notificar al proveedor de autenticación para que el estado se actualice
            _customAuthStateProvider.NotifyUserLogout();
        }

        public async Task<UsuarioDto> GetCurrentUser()
        {
            return await _localStorage.GetItemAsync<UsuarioDto>("userData");
        }

        public async Task<string> GetToken()
        {
            return await _localStorage.GetItemAsync<string>("authToken");
        }

        public async Task<bool> IsAuthenticated()
        {
            var token = await GetToken();
            return !string.IsNullOrEmpty(token);
        }
    }
}
