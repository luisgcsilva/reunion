using System.Net.Http.Headers;
using System.Text.Json;
using web.reunion.Interfaces;
using web.reunion.Models;

namespace web.reunion.Services
{
    public class AdminGroupService(IHttpClientFactory clientFactory,
        IHttpContextAccessor httpContextAccessor) : IAdminGroupService
    {
        private readonly IHttpContextAccessor _contextAccessor = httpContextAccessor;
        private readonly IHttpClientFactory _clientFactory = clientFactory;

        public async Task DeleteAdminGroupAsync(int id)
        {
            var client = _clientFactory.CreateClient("apiClient");
            var token = _contextAccessor.HttpContext?.User.FindFirst("access_token")?.Value;

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                await client.DeleteAsync("AdminGroups/" + id);
            }
            else
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }
        }

        public async Task<AdminGroup> GetAdminGroupAsync(int id)
        {
            var client = _clientFactory.CreateClient("apiClient");
            var token = _contextAccessor.HttpContext?.User.FindFirst("access_token")?.Value;

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync($"AdminGroups/{id}");

                if (response.IsSuccessStatusCode)
                {
                    using var clientResponseStream = await response
                        .Content.ReadAsStreamAsync();
                    var adminGroup = await JsonSerializer.DeserializeAsync<AdminGroup>(
                        clientResponseStream, MyJsonOptions.Default);

                    return adminGroup!;
                }
                else
                {
                    throw new HttpRequestException($"Failed to retrieve Locais. Status code: {response.StatusCode}");

                }
            }
            else
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }
        }

        public async Task<List<AdminGroup>> GetAdminGroupsAsync()
        {
            var client = _clientFactory.CreateClient("apiClient");
            var token = _contextAccessor.HttpContext?.User.FindFirst("access_token")?.Value;

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync("AdminGroups");

                if (response.IsSuccessStatusCode)
                {
                    using var clientResponseStream = await response
                        .Content.ReadAsStreamAsync();
                    var adminGroups = await JsonSerializer.DeserializeAsync<List<AdminGroup>>(
                        clientResponseStream, MyJsonOptions.Default);

                    return adminGroups!;
                }
                else
                {
                    throw new HttpRequestException($"Failed to retrieve Locais. Status code: {response.StatusCode}");

                }
            }
            else
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }
        }

        public async Task PostAdminGroupAsync(AdminGroup adminGroup)
        {
            var client = _clientFactory.CreateClient("apiClient");

            var token = _contextAccessor.HttpContext?.User.FindFirst("access_token")?.Value;

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                await client.PostAsJsonAsync("AdminGroups/", adminGroup);
            }
            else
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }
        }

        public async Task PutAdminGroupAsync(AdminGroup adminGroup)
        {
            var client = _clientFactory.CreateClient("apiClient");

            var token = _contextAccessor.HttpContext?.User.FindFirst("access_token")?.Value;

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.PutAsJsonAsync($"AdminGroups", adminGroup);

                response.EnsureSuccessStatusCode();
            }
            else
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }
        }
    }
}
