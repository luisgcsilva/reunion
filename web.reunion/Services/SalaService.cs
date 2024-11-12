using Microsoft.IdentityModel.Tokens;
using System.Net.Http.Headers;
using System.Text.Json;
using web.reunion.Interfaces;
using web.reunion.Models;

namespace web.reunion.Services
{
    public class SalaService(IHttpClientFactory clienctFactory,
        IHttpContextAccessor httpContextAccessor) : ISalaService
    {
        private readonly IHttpClientFactory _clientFactory = clienctFactory;
        private readonly IHttpContextAccessor _contextAccessor = httpContextAccessor;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<Sala>> GetSalasAsync()
        {
            var client = _clientFactory.CreateClient("apiClient");
            var token = _contextAccessor.HttpContext?.User.FindFirst("access_token")?.Value;

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync("Salas");

                if (response.IsSuccessStatusCode)
                {
                    using var clientResponseStream = await response
                        .Content.ReadAsStreamAsync();
                    var salas = await JsonSerializer.DeserializeAsync<List<Sala>>(
                        clientResponseStream, MyJsonOptions.Default);

                    return salas!;
                }
                else
                {
                    throw new HttpRequestException($"Failed to retrieve Salas. Status code: {response.StatusCode}");
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
        /// <param name="salaId"></param>
        /// <returns></returns>
        public async Task<Sala> GetSalaAsync(int salaId)
        {
            var client = _clientFactory.CreateClient("apiClient");

            var token = _contextAccessor.HttpContext?.User.FindFirst("access_token")?.Value;

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync($"Salas/{salaId}");

                if (response.IsSuccessStatusCode)
                {
                    using var clientResponseStream = await response.Content.ReadAsStreamAsync();
                    var sala = await JsonSerializer.DeserializeAsync
                        <Sala>(clientResponseStream, MyJsonOptions.Default);
                    return sala!;
                }
                else
                {
                    throw new HttpRequestException($"Failed to retrieve Sala. Status code: {response.StatusCode}");
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
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public async Task PostSalaAsync(EditSalaViewModel viewModel)
        {
            var newSala = new Sala
            {
                LocalId = viewModel.Sala!.LocalId,
                Descricao = viewModel.Sala!.Descricao,
                Capacidade = viewModel.Sala!.Capacidade,
                Localizacao = viewModel.Sala!.Localizacao,
                Cor = viewModel.Sala!.Cor,
                IsActive = true
            };

            var client = _clientFactory.CreateClient("apiClient");

            var token = _contextAccessor.HttpContext?.User.FindFirst("access_token")?.Value;

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var responsePostSala = await client.PostAsJsonAsync("Salas", newSala);

                if (responsePostSala.IsSuccessStatusCode)
                {
                    var contentStream = await responsePostSala.Content.ReadAsStreamAsync();
                    var salaCriada = await JsonSerializer.DeserializeAsync<int>(contentStream);

                    var selectedMateriais = viewModel.MaterialIds;
                    var materialQuantities = viewModel.MaterialQuantities;

                    if (!selectedMateriais.IsNullOrEmpty())
                    {
                        foreach (var materialId in selectedMateriais!)
                        {
                            var quantity = materialQuantities!.FirstOrDefault(q => q.Key == materialId);

                            var SalaMaterial = new SalaMaterial
                            {
                                SalaId = salaCriada,
                                MaterialId = materialId,
                                Quantidade = quantity.Value
                            };

                            await PostSalaMaterialAsync(SalaMaterial);
                        }
                    }
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
        /// <param name="salaId"></param>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public async Task PutSalaAsync(int salaId, EditSalaViewModel viewModel)
        {
            var client = _clientFactory.CreateClient("apiClient");

            var token = _contextAccessor.HttpContext?.User.FindFirst("access_token")?.Value;

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var requestPutSala = await client.PutAsJsonAsync($"Salas", viewModel.Sala);

                if (requestPutSala.IsSuccessStatusCode)
                {
                    var deleteClient = _clientFactory.CreateClient("apiClient");
                    deleteClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var requestDeleteSM = await deleteClient.DeleteAsync($"SalaMateriais/{salaId}");

                    if (requestDeleteSM.IsSuccessStatusCode)
                    {
                        var selectedMateriais = viewModel.MaterialIds;
                        var materialQuantities = viewModel.MaterialQuantities;

                        if (!selectedMateriais.IsNullOrEmpty())
                        {
                            foreach (var materialId in selectedMateriais!)
                            {
                                if (materialQuantities!.TryGetValue(materialId, out int quantity))
                                {
                                    var salaMaterial = new SalaMaterial
                                    {
                                        SalaId = salaId,
                                        MaterialId = materialId,
                                        Quantidade = quantity
                                    };

                                    await PostSalaMaterialAsync(salaMaterial);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="salaMaterialDto"></param>
        /// <returns></returns>
        public async Task PostSalaMaterialAsync(SalaMaterial salaMaterialDto)
        {
            var client = _clientFactory.CreateClient("apiClient");

            var token = _contextAccessor.HttpContext?.User.FindFirst("access_token")?.Value;

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var responseSalaMaterial = await client.PostAsJsonAsync(
                 "SalaMateriais/", salaMaterialDto);

                responseSalaMaterial.EnsureSuccessStatusCode();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<SalaMaterial>> GetSalaMateriaisAsync(int id)
        {
            var client = _clientFactory.CreateClient("apiClient");

            var token = _contextAccessor.HttpContext?.User.FindFirst("access_token")?.Value;

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var responseSalaMateriais = await client.GetAsync($"SalaMateriais/{id}");

                var salaMateriais = new List<SalaMaterial>();

                if (responseSalaMateriais.IsSuccessStatusCode)
                {
                    using var responseStream = await responseSalaMateriais.Content.ReadAsStreamAsync();
                    var salaMaterial = await JsonSerializer.DeserializeAsync
                        <List<SalaMaterial>>(responseStream, MyJsonOptions.Default);
                    if (!salaMaterial.IsNullOrEmpty())
                    {
                        return salaMaterial!;
                    }

                    return salaMateriais;
                }
                else
                {
                    throw new HttpRequestException($"Failed to retrieve Sala. Status code: {responseSalaMateriais.StatusCode}");
                }
            }
            else
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }
        }
    }
}
