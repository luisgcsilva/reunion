using System.Net.Http.Headers;
using System.Text.Json;
using web.reunion.Interfaces;
using web.reunion.Models;

namespace web.reunion.Services
{
    public class LocalService(IHttpClientFactory clientFactory,
        IHttpContextAccessor httpContextAccessor) : ILocalService
    {
        private readonly IHttpClientFactory _clientFactory = clientFactory;
        private readonly IHttpContextAccessor _contextAccessor = httpContextAccessor;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localId"></param>
        /// <returns></returns>
        public async Task DeleteLocalAsync(int localId)
        {
            var client = _clientFactory.CreateClient("apiClient");
            var token = _contextAccessor.HttpContext?.User.FindFirst("access_token")?.Value;

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                await client.DeleteAsync("Locais/" + localId);
            }
            else
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<Local>> GetLocaisAsync()
        {
            var client = _clientFactory.CreateClient("apiClient");
            var token = _contextAccessor.HttpContext?.User.FindFirst("access_token")?.Value;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync("Locais");

                if (response.IsSuccessStatusCode)
                {
                    using var clientResponseStream = await response
                        .Content.ReadAsStreamAsync();
                    var locais = await JsonSerializer.DeserializeAsync<List<Local>>(
                        clientResponseStream, MyJsonOptions.Default);

                    return locais!;
                }
                else
                {
                    throw new HttpRequestException($"Failed to retrieve Locais. Status code: {response.StatusCode}");

                }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localId"></param>
        /// <returns></returns>
        public async Task<Local> GetLocalAsync(int localId)
        {
            var client = _clientFactory.CreateClient("apiClient");
            var token = _contextAccessor.HttpContext?.User.FindFirst("access_token")?.Value;

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync($"Locais/{localId}");

                if (response.IsSuccessStatusCode)
                {
                    using var clientResponseStream = await response
                        .Content.ReadAsStreamAsync();
                    var local = await JsonSerializer.DeserializeAsync
                        <Local>(clientResponseStream, MyJsonOptions.Default);

                    return local!;
                }
                else
                {
                    throw new HttpRequestException($"Failed to retrieve Local. Status code: {response.StatusCode}");
                }
            }
            else
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="local"></param>
        /// <returns></returns>
        public async Task PostLocalAsync(Local local)
        {
            var client = _clientFactory.CreateClient("apiClient");

            var token = _contextAccessor.HttpContext?.User.FindFirst("access_token")?.Value;

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                await client.PostAsJsonAsync("Locais/", local);
            }
            else
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="local"></param>
        /// <returns></returns>
        public async Task PutLocalAsync(Local local)
        {
            var client = _clientFactory.CreateClient("apiClient");

            var token = _contextAccessor.HttpContext?.User.FindFirst("access_token")?.Value;

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.PutAsJsonAsync($"Locais", local);

                response.EnsureSuccessStatusCode();
            }
            else
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }
        }
    }
}
