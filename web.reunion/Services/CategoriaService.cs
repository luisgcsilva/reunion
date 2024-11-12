using System.Net.Http.Headers;
using System.Text.Json;
using web.reunion.Interfaces;
using web.reunion.Models;

namespace web.reunion.Services
{
    public class CategoriaService(IHttpClientFactory clientFactory,
        IHttpContextAccessor httpContextAccessor) : ICategoriaService
    {
        private readonly IHttpClientFactory _clientFactory = clientFactory;
        private readonly IHttpContextAccessor _contextAccessor = httpContextAccessor;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoriaId"></param>
        /// <returns></returns>
        public async Task DeleteCategoriaAsync(int categoriaId)
        {
            var client = _clientFactory.CreateClient("apiClient");
            var token = _contextAccessor.HttpContext?.User.FindFirst("access_token")?.Value;

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
                await client.DeleteAsync("Categorias/" + categoriaId);
            }
            else
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoriaId"></param>
        /// <returns></returns>
        public async Task<Categoria> GetCategoriaAsync(int categoriaId)
        {
            var client = _clientFactory.CreateClient("apiClient");
            var token = _contextAccessor.HttpContext?.User.FindFirst("access_token")?.Value;

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync($"Categorias/{categoriaId}");

                if (response.IsSuccessStatusCode)
                {
                    using var clientResponseStream = await response
                        .Content.ReadAsStreamAsync();
                    var categoria = await JsonSerializer.DeserializeAsync<Categoria>(
                        clientResponseStream, MyJsonOptions.Default);

                    return categoria!;
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<Categoria>> GetCategoriasAsync()
        {
            var client = _clientFactory.CreateClient("apiClient");
            var token = _contextAccessor.HttpContext?.User.FindFirst("access_token")?.Value;

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync("Categorias");

                if (response.IsSuccessStatusCode)
                {
                    using var clientResponseStream = await response
                        .Content.ReadAsStreamAsync();
                    var categorias = await JsonSerializer.DeserializeAsync<List<Categoria>>(
                        clientResponseStream, MyJsonOptions.Default);

                    return categorias!;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoria"></param>
        /// <returns></returns>
        public async Task PostCategoriaAsync(Categoria categoria)
        {
            var client = _clientFactory.CreateClient("apiClient");

            var token = _contextAccessor.HttpContext?.User.FindFirst("access_token")?.Value;

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                await client.PostAsJsonAsync("Categorias/", categoria);
            }
            else
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoria"></param>
        /// <returns></returns>
        public async Task PutCategoriaAsync(Categoria categoria)
        {
            var client = _clientFactory.CreateClient("apiClient");

            var token = _contextAccessor.HttpContext?.User.FindFirst("access_token")?.Value;

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.PutAsJsonAsync($"Categorias", categoria);

                response.EnsureSuccessStatusCode();
            }
            else
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }
        }
    }
}
