using System.Net.Http.Headers;
using System.Text.Json;
using web.reunion.Interfaces;
using web.reunion.Models;

namespace web.reunion.Services
{
    public class MaterialService(IHttpClientFactory clientFactory,
        IHttpContextAccessor httpContextAccessor) : IMaterialService
    {
        private readonly IHttpClientFactory _clientFactory = clientFactory;
        private readonly IHttpContextAccessor _contextAccessor = httpContextAccessor;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<Material>> GetMateriaisAsync()
        {
            var client = _clientFactory.CreateClient("apiClient");
            var token = _contextAccessor.HttpContext?.User.FindFirst("access_token")?.Value;

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync("Materiais");

                if (response.IsSuccessStatusCode)
                {
                    using var clientResponseStream = await response.Content.ReadAsStreamAsync();
                    var materiais = await JsonSerializer.DeserializeAsync<List<Material>>(
                        clientResponseStream, MyJsonOptions.Default);

                    return materiais!;
                }
                else
                {
                    throw new HttpRequestException($"Failed to retrieve Materiais. Status code: {response.StatusCode}");
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
        /// <param name="materialId"></param>
        /// <returns></returns>
        public async Task<Material> GetMaterialAsync(int materialId)
        {
            var client = _clientFactory.CreateClient("apiClient");

            var token = _contextAccessor.HttpContext?.User.FindFirst("access_token")?.Value;

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync($"Materiais/{materialId}");

                if (response.IsSuccessStatusCode)
                {
                    using var responseStream = await response.Content.ReadAsStreamAsync();
                    var material = await JsonSerializer.DeserializeAsync
                        <Material>(responseStream, MyJsonOptions.Default);
                    return material!;
                }
                else
                {
                    throw new HttpRequestException($"Failed to retrieve Material. Status code: {response.StatusCode}");
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
        /// <param name="materialDto"></param>
        /// <returns></returns>
        public async Task PostMaterialAsync(Material materialDto)
        {
            var materiaisClient = _clientFactory.CreateClient("apiClient");

            var token = _contextAccessor.HttpContext?.User.FindFirst("access_token")?.Value;

            if (!string.IsNullOrEmpty(token))
            {
                materiaisClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                await materiaisClient.PostAsJsonAsync("Materiais/", materialDto);
            }
            else
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="materialDto"></param>
        /// <returns></returns>
        public async Task PutMaterialAsync(Material materialDto)
        {
            var client = _clientFactory.CreateClient("apiClient");

            var token = _contextAccessor.HttpContext?.User.FindFirst("access_token")?.Value;

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.PutAsJsonAsync($"Materiais", materialDto);

                response.EnsureSuccessStatusCode();
            }
            else
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="materialId"></param>
        /// <returns></returns>
        public async Task DeleteMaterialAsync(int materialId)
        {
            var client = _clientFactory.CreateClient("apiClient");

            var token = _contextAccessor.HttpContext?.User.FindFirst("access_token")?.Value;

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                await client.DeleteAsync("Materiais/" + materialId);
            }
            else
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }
        }
    }
}
