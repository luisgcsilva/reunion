using Microsoft.IdentityModel.Tokens;
using MimeKit;
using System.Net.Http.Headers;
using System.Text.Json;
using web.reunion.Interfaces;
using web.reunion.Models;

namespace web.reunion.Services
{
    public class MarcacaoService(IHttpClientFactory clientFactory,
        IWebHostEnvironment env,
        IMaterialService materialService,
        IHttpContextAccessor httpContextAccessor,
        ISalaService salaService,
        ICategoriaService categoriaService,
        ILocalService localService,
        ISenderEmail emailSender) : IMarcacaoService
    {
        private readonly IHttpClientFactory _clientFactory = clientFactory;
        private readonly IHttpContextAccessor _contextAccessor = httpContextAccessor;
        private readonly IMaterialService _materialService = materialService;
        private readonly ICategoriaService _categoriaService = categoriaService;
        private readonly ISalaService _salaService = salaService;
        private readonly ILocalService _localService = localService;
        private readonly ISenderEmail _emailSender = emailSender;
        private readonly IWebHostEnvironment _env = env;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<Marcacao>> GetMarcacoesAsync()
        {
            var client = _clientFactory.CreateClient("apiClient");
            var token = _contextAccessor.HttpContext?.User.FindFirst("access_token")?.Value;

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync("Marcacoes");

                if (response.IsSuccessStatusCode)
                {
                    using var responseStream = await response
                        .Content.ReadAsStreamAsync();
                    var marcacoes = await JsonSerializer.DeserializeAsync<List<Marcacao>>
                        (responseStream, MyJsonOptions.Default);

                    return marcacoes!;
                }
                else
                {
                    throw new HttpRequestException($"Failed to retrieve Marcações. Status code: {response.StatusCode}");
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
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteMarcacaoMaterialsAsync(int id)
        {
            var client = _clientFactory.CreateClient("apiClient");
            var token = _contextAccessor.HttpContext?.User.FindFirst("access_token")?.Value;

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                await client.DeleteAsync($"MarcacaoMateriais/{id}");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Marcacao> GetMarcacaoAsync(int id)
        {
            var client = _clientFactory.CreateClient("apiClient");
            var token = _contextAccessor.HttpContext?.User.FindFirst("access_token")?.Value;

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync($"Marcacoes/marcacao/{id}");

                if (response.IsSuccessStatusCode)
                {
                    using var responseStream = await response.Content.ReadAsStreamAsync();
                    var marcacao = await JsonSerializer.DeserializeAsync
                        <Marcacao>(responseStream, MyJsonOptions.Default);
                    return marcacao!;
                }
                else
                {
                    throw new HttpRequestException($"Failed to retrieve Marcações. Status code: {response.StatusCode}");
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
        /// <param name="user"></param>
        /// <param name="local"></param>
        /// <param name="userRole"></param>
        /// <returns></returns>
        public async Task PostMarcacaoAsync(AdminDashboardViewModel viewModel, string user, int local, string userRole)
        {
            var marcacao = viewModel.Marcacao;

            if (userRole == "Client")
            {
                marcacao.Estado = "Pendente";
                marcacao.SalaId = 1;
            }
            else
            {
                marcacao.Estado = "Aprovado";
            }

            marcacao.Utilizador = user;
            marcacao.DataRegisto = DateTime.Now;
            
            if (userRole != "AdminCA")
            {
                marcacao.LocalId = local;
            }

            if (userRole == "AdminSecCA")
            {
                var salaDes = await _salaService.GetSalasAsync();
                
                marcacao.SalaId = salaDes.First(s => s.LocalId == local).SalaId;
            }

            var client = _clientFactory.CreateClient("apiClient");
            var token = _contextAccessor.HttpContext?.User.FindFirst("access_token")?.Value;

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.PostAsJsonAsync("Marcacoes/", marcacao);

                if (response.IsSuccessStatusCode)
                {
                    var contentStream = await response.Content.ReadAsStreamAsync();
                    var marcacaoCriada = await JsonSerializer.DeserializeAsync<int>(contentStream);

                    var selectedMateriais = viewModel.MaterialIds;
                    var materialQuantities = viewModel.MaterialQuantities;
                    var marcacaoMateriais = new List<MarcacaoMaterial>();

                    if (!selectedMateriais.IsNullOrEmpty())
                    {
                        foreach (var materialId in selectedMateriais)
                        {
                            var quantity = materialQuantities.FirstOrDefault(q => q.Key == materialId);

                            var MarcacaoMaterial = new MarcacaoMaterial
                            {
                                MarcacaoId = marcacaoCriada,
                                MaterialId = materialId,
                                Quantidade = quantity.Value
                            };

                            await PostMarcacaoMaterialAsync(MarcacaoMaterial);
                        }

                        marcacaoMateriais = await GetMarcacacoesMaterialAsync(marcacaoCriada);
                    }

                    var categoria = await _categoriaService.GetCategoriaAsync(marcacao.CategoriaId);

                    var localDes = await _localService.GetLocalAsync(marcacao.LocalId);

                    var salaDes = await _salaService.GetSalaAsync(marcacao.SalaId);

                    var materiaisList = await _materialService.GetMateriaisAsync();

                    if (userRole == "Client")
                    {
                        string relativePath = Path.Combine("wwwroot", "lib", "emailtemplates", "email-template-criar-client.cshtml");
                        string fullPath = Path.Combine(_env.ContentRootPath, relativePath);
                        var emailBodyTemplate = File.ReadAllText(fullPath);

                        EmailViewModel emailViewModel = new()
                        {
                            NumPessoas = marcacao.NumPessoas.ToString(),
                            Dia = marcacao.Dia.ToString("dd/MM/yyyy"),
                            Estado = marcacao.Estado,
                            Local = localDes.Descricao,
                            HoraInicio = marcacao.HoraInicio.ToString("HH:mm"),
                            HoraFim = marcacao.HoraFim.ToString("HH:mm"),
                            Categoria = categoria.Descricao,
                            Observacoes = marcacao.Observacoes!,
                            MarcacaoMateriais = marcacaoMateriais
                        };

                        string marcacaoMateriaisHtml = string.Empty;

                        foreach (var material in marcacaoMateriais!)
                        {
                            marcacaoMateriaisHtml += $"<li>{materiaisList?.Where(m => m.MaterialId == material.MaterialId).First().Descricao}: {material.Quantidade}</li>";
                        }

                        string emailBody = emailBodyTemplate
                            .Replace("{NumPessoas}", emailViewModel.NumPessoas)
                            .Replace("{Local}", emailViewModel.Local)
                            .Replace("{Dia}", emailViewModel.Dia)
                            .Replace("{Estado}", emailViewModel.Estado)
                            .Replace("{HoraInicio}", emailViewModel.HoraInicio)
                            .Replace("{HoraFim}", emailViewModel.HoraFim)
                            .Replace("{Categoria}", emailViewModel.Categoria)
                            .Replace("{Observacoes}", emailViewModel.Observacoes)
                            .Replace("{MarcacaoMateriais}", marcacaoMateriaisHtml);

                        var emailMessage = new BodyBuilder
                        {
                            HtmlBody = emailBody
                        };

                        var eMessage = emailMessage.ToMessageBody();

                        await _emailSender.SendEmailAsync(marcacao.Utilizador.ToString(), "Pedido de Marcação", eMessage);

                        await SendEmailsAdmin(marcacao, eMessage, "Criar");
                    } 
                    else
                    {
                        string relativePath = Path.Combine("wwwroot", "lib", "emailtemplates", "email-template-criar-admin.cshtml");
                        string fullPath = Path.Combine(_env.ContentRootPath, relativePath);
                        var emailBodyTemplate = File.ReadAllText(fullPath);

                        EmailViewModel emailViewModel = new()
                        {
                            Utilizador = marcacao.Utilizador,
                            NumPessoas = marcacao.NumPessoas.ToString(),
                            Dia = marcacao.Dia.ToString("dd/MM/yyyy"),
                            Local = localDes.Descricao,
                            Sala = salaDes.Descricao!,
                            HoraInicio = marcacao.HoraInicio.ToString("HH:mm"),
                            HoraFim = marcacao.HoraFim.ToString("HH:mm"),
                            Categoria = categoria.Descricao,
                            Observacoes = marcacao.Observacoes!,
                            MarcacaoMateriais = marcacaoMateriais
                        };

                        string marcacaoMateriaisHtml = string.Empty;

                        foreach (var material in marcacaoMateriais!)
                        {
                            marcacaoMateriaisHtml += $"<li>{materiaisList?.Where(m => m.MaterialId == material.MaterialId).First().Descricao}: {material.Quantidade}</li>";
                        }

                        string emailBody = emailBodyTemplate
                            .Replace("{Utilizador}", emailViewModel.Utilizador)
                            .Replace("{NumPessoas}", emailViewModel.NumPessoas)
                            .Replace("{Local}", emailViewModel.Local)
                            .Replace("{Sala}", emailViewModel.Sala)
                            .Replace("{Dia}", emailViewModel.Dia)
                            .Replace("{HoraInicio}", emailViewModel.HoraInicio)
                            .Replace("{HoraFim}", emailViewModel.HoraFim)
                            .Replace("{Categoria}", emailViewModel.Categoria)
                            .Replace("{Observacoes}", emailViewModel.Observacoes)
                            .Replace("{MarcacaoMateriais}", marcacaoMateriaisHtml);

                        var emailMessage = new BodyBuilder
                        {
                            HtmlBody = emailBody
                        };

                        var eMessage = emailMessage.ToMessageBody();

                        await _emailSender.SendEmailAsync(marcacao.Utilizador.ToString(), "Pedido de Marcação", eMessage);

                        await SendEmailsAdmin(marcacao, eMessage, "Criar");
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
        /// <param name="marcacao"></param>
        /// <param name="eMessage"></param>
        /// <param name="acao"></param>
        /// <returns></returns>
        public async Task SendEmailsAdmin(Marcacao marcacao, MimeEntity eMessage, string acao)
        {
            var client = _clientFactory.CreateClient("apiClient");
            var token = _contextAccessor.HttpContext?.User.FindFirst("access_token")?.Value;

            var local = await _localService.GetLocalAsync(marcacao.LocalId);

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var adminRoles = await client.GetAsync("AdminGroups");

                using var response = await adminRoles.Content.ReadAsStreamAsync();

                var roles = await JsonSerializer.DeserializeAsync<List<AdminGroup>>(
                    response, MyJsonOptions.Default);

                var role = "";
                var usersList = await client.GetAsync($"Authentication/users/{role}");

                foreach (var r in roles)
                {
                    if (local.AdminGroupId == r.AdminGroupId)
                    {
                        role = r.Grupo;
                        usersList = await client.GetAsync($"Authentication/users/{role}");
                        break;
                    }
                }

                var users = await usersList.Content.ReadAsStringAsync();

                var emails = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(users);

                var titulo = "";

                switch (acao)
                {
                    case "Editar":
                        titulo = "Alterações nos Detalhes de um Pedido de Marcação";
                        break;
                    case "Criar":
                        titulo = "Novo Pedido de Marcação";
                        break;
                }

                if (!emails.IsNullOrEmpty())
                {
                    foreach (var email in emails!)
                    {
                        await _emailSender.SendEmailAsync(email, titulo, eMessage);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="marcacaoMaterial"></param>
        /// <returns></returns>
        public async Task PostMarcacaoMaterialAsync(MarcacaoMaterial marcacaoMaterial)
        {
            var clientMarcacaoMaterial = _clientFactory.CreateClient("apiClient");

            var token = _contextAccessor.HttpContext?.User.FindFirst("access_token")?.Value;

            if (!string.IsNullOrEmpty(token))
            {
                clientMarcacaoMaterial.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var responseMarcacaoMaterial = await clientMarcacaoMaterial.PostAsJsonAsync("MarcacaoMateriais/", marcacaoMaterial);

                responseMarcacaoMaterial.EnsureSuccessStatusCode();
            }
            else
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="marcacaoId"></param>
        /// <returns></returns>
        public async Task<List<MarcacaoMaterial>> GetMarcacacoesMaterialAsync(int marcacaoId)
        {
            var client = _clientFactory.CreateClient("apiClient");

            var token = _contextAccessor.HttpContext?.User.FindFirst("access_token")?.Value;

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync($"MarcacaoMateriais/{marcacaoId}");

                if (response.IsSuccessStatusCode)
                {
                    using var responseStream = await response.Content.ReadAsStreamAsync();
                    var marcacaoMaterial = await JsonSerializer.DeserializeAsync
                        <List<MarcacaoMaterial>>(responseStream, MyJsonOptions.Default);
                    return marcacaoMaterial!;
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
        /// <param name="viewModel"></param>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <param name="userRole"></param>
        /// <param name="userLocal"></param>
        /// <returns></returns>
        public async Task PutMarcacaoAsync(AdminDashboardViewModel viewModel, int id, string user, string userRole, int userLocal)
        {
            viewModel.Marcacao.ModificadoPor = user;
            viewModel.Marcacao.ModificadoEm = DateTime.Now;

            var client = _clientFactory.CreateClient("apiClient");

            var token = _contextAccessor.HttpContext?.User.FindFirst("access_token")?.Value;

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var marcacao = viewModel.Marcacao;

                if (userRole == "Client")
                {
                    marcacao.SalaId = 1;
                    marcacao.LocalId = userLocal;
                }

                var response = await client.PutAsJsonAsync($"Marcacoes", marcacao);

                var selectedMateriais = viewModel.MaterialIds;
                var materialQuantities = viewModel.MaterialQuantities;
                var marcacaoMateriais = new List<MarcacaoMaterial>();

                if (!selectedMateriais.IsNullOrEmpty())
                {
                    await DeleteMarcacaoMaterialsAsync(id);

                    foreach (var materialId in selectedMateriais)
                    {
                        var quantity = materialQuantities.FirstOrDefault(q => q.Key == materialId);

                        var MarcacaoMaterial = new MarcacaoMaterial
                        {
                            MarcacaoId = id,
                            MaterialId = materialId,
                            Quantidade = quantity.Value
                        };

                        await PostMarcacaoMaterialAsync(MarcacaoMaterial);
                    }

                    marcacaoMateriais = await GetMarcacacoesMaterialAsync(id);
                }

                var materiaisList = await _materialService.GetMateriaisAsync();

                var salasList = await _salaService.GetSalasAsync();

                var categoriasList = await _categoriaService.GetCategoriasAsync();

                var locaisList = await _localService.GetLocaisAsync();

                if (response.IsSuccessStatusCode)
                {
                    if (userRole == "Client" || userRole == "AdminCA" || userRole == "AdminSecCA")
                    {
                        string relativePath = Path.Combine("wwwroot", "lib", "emailtemplates", "email-template-editar.cshtml");
                        string fullPath = Path.Combine(_env.ContentRootPath, relativePath);
                        var emailBodyTemplate = File.ReadAllText(fullPath);

                        EmailViewModel newViewModel = new()
                        {
                            NumPessoas = marcacao.NumPessoas.ToString(),
                            Local = locaisList.Where(l => l.LocalId == marcacao.LocalId).First().Descricao,
                            Dia = marcacao.Dia.ToString("dd/MM/yyyy"),
                            HoraInicio = marcacao.HoraInicio.ToString("HH:mm"),
                            HoraFim = marcacao.HoraFim.ToString("HH:mm"),
                            Categoria = categoriasList.Where(c => c.CategoriaId == marcacao.CategoriaId).First().Descricao,
                            Observacoes = marcacao.Observacoes!,
                            MarcacaoMateriais = marcacaoMateriais
                        };

                        string marcacaoMateriaisHtml = string.Empty;

                        foreach (var material in newViewModel.MarcacaoMateriais!)
                        {
                            marcacaoMateriaisHtml += $"<li>{materiaisList?.Where(m => m.MaterialId == material.MaterialId).First().Descricao}: {material.Quantidade}</li>";
                        }

                        string emailBody = emailBodyTemplate
                            .Replace("{NumPessoas}", newViewModel.NumPessoas)
                            .Replace("{Local}", newViewModel.Local)
                            .Replace("{Dia}", newViewModel.Dia)
                            .Replace("{HoraInicio}", newViewModel.HoraInicio)
                            .Replace("{HoraFim}", newViewModel.HoraFim)
                            .Replace("{Categoria}", newViewModel.Categoria)
                            .Replace("{Observacoes}", newViewModel.Observacoes)
                            .Replace("{MarcacaoMateriais}", marcacaoMateriaisHtml);

                        var emailMessage = new BodyBuilder
                        {
                            HtmlBody = emailBody
                        };

                        var eMessage = emailMessage.ToMessageBody();

                        await _emailSender.SendEmailAsync(marcacao.Utilizador.ToString(), "Alteração nos Detalhes da Sua Marcação", eMessage);

                        await SendEmailsAdmin(marcacao, eMessage, "Editar");
                    }
                    else
                    {
                        string sala = salasList!.FirstOrDefault(s => s.SalaId == marcacao.SalaId)!.Descricao!;

                        if (marcacao.Estado == "Aprovado")
                        {
                            string relativePath = Path.Combine("wwwroot", "lib", "emailtemplates", "email-template-aprovado.cshtml");
                            string fullPath = Path.Combine(_env.ContentRootPath, relativePath);
                            var emailBodyTemplate = File.ReadAllText(fullPath);

                            EmailViewModel newViewModel = new()
                            {
                                Estado = marcacao.Estado,
                                Local = locaisList.Where(l => l.LocalId == marcacao.LocalId).First().Descricao,
                                Sala = sala,
                                NumPessoas = marcacao.NumPessoas.ToString(),
                                Dia = marcacao.Dia.ToString("dd/MM/yyyy"),
                                HoraInicio = marcacao.HoraInicio.ToString("HH:mm"),
                                HoraFim = marcacao.HoraFim.ToString("HH:mm"),
                                Categoria = categoriasList.Where(c => c.CategoriaId == marcacao.CategoriaId).First().Descricao,
                                Observacoes = marcacao.Observacoes!,
                                MarcacaoMateriais = marcacaoMateriais
                            };

                            string marcacaoMateriaisHtml = string.Empty;

                            foreach (var material in newViewModel.MarcacaoMateriais!)
                            {
                                marcacaoMateriaisHtml += $"<li>{materiaisList?.Where(m => m.MaterialId == material.MaterialId).First().Descricao}: {material.Quantidade}</li>";
                            }

                            string emailBody = emailBodyTemplate
                                .Replace("{Estado}", newViewModel.Estado)
                                .Replace("{Local}", newViewModel.Local)
                                .Replace("{Sala}", newViewModel.Sala)
                                .Replace("{NumPessoas}", newViewModel.NumPessoas)
                                .Replace("{Dia}", newViewModel.Dia)
                                .Replace("{HoraInicio}", newViewModel.HoraInicio)
                                .Replace("{HoraFim}", newViewModel.HoraFim)
                                .Replace("{Categoria}", newViewModel.Categoria)
                                .Replace("{Observacoes}", newViewModel.Observacoes)
                                .Replace("{MarcacaoMateriais}", marcacaoMateriaisHtml);

                            var emailMessage = new BodyBuilder
                            {
                                HtmlBody = emailBody
                            };

                            var eMessage = emailMessage.ToMessageBody();

                            await _emailSender.SendEmailAsync(marcacao.Utilizador.ToString(), "Alteração no Estado da Sua Marcação", eMessage);
                        }
                        else
                        {
                            string relativePath = Path.Combine("wwwroot", "lib", "emailtemplates", "email-template-rejeitado.cshtml");
                            string fullPath = Path.Combine(_env.ContentRootPath, relativePath);
                            var emailBodyTemplate = File.ReadAllText(fullPath);

                            EmailViewModel newViewModel = new()
                            {
                                Estado = marcacao.Estado,
                                NumPessoas = marcacao.NumPessoas.ToString(),
                                Local = locaisList.Where(l => l.LocalId == marcacao.LocalId).First().Descricao,
                                Dia = marcacao.Dia.ToString("dd/MM/yyyy"),
                                HoraInicio = marcacao.HoraInicio.ToString("HH:mm"),
                                HoraFim = marcacao.HoraFim.ToString("HH:mm"),
                                Categoria = categoriasList.Where(c => c.CategoriaId == marcacao.CategoriaId).First().Descricao,
                                Observacoes = marcacao.Observacoes!,
                                Motivo = marcacao.Motivo!,
                                MarcacaoMateriais = marcacaoMateriais
                            };

                            string marcacaoMateriaisHtml = string.Empty;

                            foreach (var material in newViewModel.MarcacaoMateriais!)
                            {
                                marcacaoMateriaisHtml += $"<li>{materiaisList?.Where(m => m.MaterialId == material.MaterialId).First().Descricao}: {material.Quantidade}</li>";
                            }

                            string emailBody = emailBodyTemplate
                                .Replace("{Estado}", newViewModel.Estado)
                                .Replace("{Local}", newViewModel.Local)
                                .Replace("{Dia}", newViewModel.Dia)
                                .Replace("{NumPessoas}", newViewModel.NumPessoas)
                                .Replace("{HoraInicio}", newViewModel.HoraInicio)
                                .Replace("{HoraFim}", newViewModel.HoraFim)
                                .Replace("{Categoria}", newViewModel.Categoria)
                                .Replace("{Observacoes}", newViewModel.Observacoes)
                                .Replace("{Motivo}", newViewModel.Motivo)
                                .Replace("{MarcacaoMateriais}", marcacaoMateriaisHtml);

                            var emailMessage = new BodyBuilder
                            {
                                HtmlBody = emailBody
                            };

                            var eMessage = emailMessage.ToMessageBody();

                            await _emailSender.SendEmailAsync(marcacao.Utilizador.ToString(), "Alteração no Estado da Sua Marcação", eMessage);
                        }
                    }
                }
            }
            else
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }
        }
    }
}
