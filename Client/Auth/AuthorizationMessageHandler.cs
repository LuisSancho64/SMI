using SMI.Client.Services;
using System.Net.Http.Headers;

namespace SMI.Client.Auth
{
    public class AuthorizationMessageHandler : DelegatingHandler
    {
        private readonly LocalStorageService _localStorage;

        public AuthorizationMessageHandler(LocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}