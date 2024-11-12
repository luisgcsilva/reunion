using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using web.reunion.Interfaces;
using web.reunion.Models;
using System.Globalization;
using Microsoft.IdentityModel.Tokens;

namespace web.reunion.Controllers
{
    [Authorize]
    public class MarcacoesController(IHttpClientFactory clientFactory,
        ISenderEmail emailSender,
        ISalaService salaService,
        ILogger<MarcacoesController> logger,
        IHttpContextAccessor httpContextAccessor,
        IMaterialService materialService,
        ILocalService localService,
        ICategoriaService categoriaService,
        IMarcacaoService marcacaoService) : Controller
    {
        private readonly IHttpClientFactory _clientFactory = clientFactory;
        private readonly ISalaService _salaService = salaService;
        private readonly IMarcacaoService _marcacaoService = marcacaoService;
        private readonly IMaterialService _materialService = materialService;
        private readonly ILocalService _localService = localService;
        private readonly ICategoriaService _categoriaService = categoriaService;
        private readonly IHttpContextAccessor _contextAccessor = httpContextAccessor;
        private readonly ISenderEmail _emailSender = emailSender;
        private readonly ILogger<MarcacoesController> _logger = logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> GetMateriaisFromSala(int id)
        {
            var salaMateriais = await _salaService.GetSalaMateriaisAsync(id);

            return Json(salaMateriais);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="salaId"></param>
        /// <param name="estado"></param>
        /// <param name="utilizador"></param>
        /// <param name="local"></param>
        /// <returns></returns>
        public async Task<IActionResult> GetBookings(int? salaId, string estado, string utilizador, int? local)
        {
            var salasList = await _salaService.GetSalasAsync();

            var categoriasList = await _categoriaService.GetCategoriasAsync();

            var locaisList = await _localService.GetLocaisAsync();

            locaisList = locaisList.Where(l => l.IsActive == true).ToList();

            var marcacoes = await _marcacaoService.GetMarcacoesAsync();

            if (estado != null)
            {
                marcacoes = marcacoes?.Where(m => m.Estado == estado).ToList();
            }
            if (User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value == "Client")
            {
                marcacoes = marcacoes?.Where(m => m.Estado == "Aprovado").ToList();
            }
            if (utilizador != null)
            {
                var marcacoesUser = await _marcacaoService.GetMarcacoesAsync();
                marcacoes = marcacoesUser?.Where(m => m.Utilizador == utilizador).ToList();
            }
            if (local != null)
            {
                marcacoes = marcacoes?.Where(m => m.LocalId == local).ToList();
            }
            if (salaId != null)
            {
                marcacoes = marcacoes?.Where(m => m.SalaId == salaId).ToList();
            }

            var events = marcacoes?.Select(m => new
            {
                title = m.Utilizador,
                start = $"{m.Dia:yyyy-MM-dd}T{m.HoraInicio:HH:mm:ss}",
                end = $"{m.Dia:yyyy-MM-dd}T{m.HoraFim:HH:mm:ss}",
                extendedProps = new
                {
                    sala = salasList?.FirstOrDefault(s => s.SalaId == m.SalaId)?.Descricao,
                    color = salasList?.FirstOrDefault(s => s.SalaId == m.SalaId)?.Cor,
                    marcacaoId = m.MarcacaoId,
                    salaId = m.SalaId,
                    dia = m.Dia,
                    local = locaisList.FirstOrDefault(l => l.LocalId == m.LocalId)?.Descricao,
                    horaInicio = m.HoraInicio,
                    horaFim = m.HoraFim,
                    estado = m.Estado,
                    categoria = categoriasList.FirstOrDefault(c => c.CategoriaId == m.CategoriaId)?.Descricao,
                    observacoes = m.Observacoes,
                    dataRegisto = m.DataRegisto,
                    modificadoPor = m.ModificadoPor,
                    modificadoEm = m.ModificadoEm,
                    motivo = m.Motivo
                }
            }).ToList();

            return Json(events);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="day"></param>
        /// <param name="localId"></param>
        /// <returns></returns>
        public async Task<IActionResult> LoadCreateForm(string start, string end, string day, int localId)
        {
            var materiaisList = await _materialService.GetMateriaisAsync();

            var local = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Locality)?.Value!);

            var salasList = await _salaService.GetSalasAsync();

            var locaisList = await _localService.GetLocaisAsync();

            locaisList = locaisList.Where(l => l.IsActive == true).ToList();

            var categoriasList = await _categoriaService.GetCategoriasAsync();

            salasList = [.. salasList.Where(s => s.IsActive == true).Where(s => s.LocalId == local).OrderBy(s => s.Descricao)];

            var salaMateriaisList = new List<SalaMaterial>();

            if (!salasList.IsNullOrEmpty())
            {
                salaMateriaisList = await _salaService.GetSalaMateriaisAsync(salasList.First().SalaId);
            }

            var viewModel = new AdminDashboardViewModel();

            CultureInfo portugueseCulture = new("pt-PT");

            DateOnly data = DateOnly.ParseExact(day, "dd/MM/yyyy", portugueseCulture);

            viewModel = new AdminDashboardViewModel
            {
                Marcacao = new Marcacao
                {
                    HoraInicio = TimeOnly.Parse(start),
                    HoraFim = TimeOnly.Parse(end),
                    Dia = data,
                    LocalId = localId
                },

                Locais = locaisList!,
                Categorias = categoriasList!,
                Salas = salasList!,
                SalaMateriais = salaMateriaisList!,
                Materiais = materiaisList!,
            };

            ViewData["CategoriaId"] = new SelectList(categoriasList, "CategoriaId", "Descricao");
            ViewData["LocalId"] = new SelectList(locaisList, "LocalId", "Descricao");
            ViewData["SalaId"] = new SelectList(salasList, "SalaId", "Descricao");
            ViewBag.SalaCapacities = salasList.ToDictionary(s => s.SalaId, s => s.Capacidade);

            return PartialView("_CreateMarcacaoForm", viewModel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(AdminDashboardViewModel viewModel)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var local = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Locality)?.Value;
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            await _marcacaoService.PostMarcacaoAsync(viewModel, userEmail!, int.Parse(local!), userRole!);


            return userRole switch
            {
                "Client" => Redirect("/Home/Client"),
                "AdminCA" => Redirect("/Home/Admin"),
                "AdminSecCA" => Redirect("/Home/AdminSecretariadoCA"),
                "SuperAdmin" => Redirect("/Home/SuperAdmin"),
                _ => Redirect("/Home/AdminDashboard"),
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="salaId"></param>
        /// <param name="dia"></param>
        /// <param name="horaInicio"></param>
        /// <param name="horaFim"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> IsSalaBooked(int salaId, string dia, string horaInicio, string horaFim)
        {
            try
            {
                var client = _clientFactory.CreateClient("apiClient");
                var token = _contextAccessor.HttpContext?.User.FindFirst("access_token")?.Value;

                CultureInfo portugueseCulture = new("pt-PT");

                var data = DateOnly.ParseExact(dia, "yyyy-MM-dd");

                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    string estado = "Aprovado";

                    var response = await client.GetAsync($"Marcacoes/isSalaBooked/{salaId}/{dia}/{horaInicio}/{horaFim}/{estado}");

                    if (response.IsSuccessStatusCode)
                    {
                        var responseData = await response.Content.ReadAsStringAsync();
                        var isBooked = bool.Parse(responseData);
                        return Ok(isBooked);
                    }
                    else
                    {
                        return StatusCode((int)response.StatusCode, "");
                    }
                }
                else
                {
                    throw new UnauthorizedAccessException("User is not authenticated.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="marcacaoId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetMateriaisMarcacao(int marcacaoId)
        {
            var marcacaoMateriais = await _marcacaoService.GetMarcacacoesMaterialAsync(marcacaoId);

            return Ok(marcacaoMateriais);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("/Marcacoes/{id:int}/Edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var salasList = await _salaService.GetSalasAsync();

            var marcacaoEditar = await _marcacaoService.GetMarcacaoAsync(id);

            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (marcacaoEditar == null)
            {
                return userRole switch
                {
                    "Client" => Redirect("/Home/Client"),
                    "AdminCA" => Redirect("/Home/Admin"),
                    "AdminSecCA" => Redirect("/Home/AdminSecretariadoCA"),
                    "SuperAdmin" => Redirect("/Home/SuperAdmin"),
                    _ => Redirect("/Home/AdminDashboard"),
                };
            }

            var marcacaoMateriais = await _marcacaoService.GetMarcacacoesMaterialAsync(id);

            var materiaisList = await _materialService.GetMateriaisAsync();

            var salaMateriais = await _salaService.GetSalaMateriaisAsync(marcacaoEditar.SalaId);

            var locaisList = await _localService.GetLocaisAsync();

            locaisList = locaisList.Where(l => l.IsActive == true).ToList();

            var categoriasList = await _categoriaService.GetCategoriasAsync();

            var viewModel = new AdminDashboardViewModel
            {
                Locais = locaisList!,
                Categorias = categoriasList!,
                Marcacao = marcacaoEditar!,
                Materiais = materiaisList!,
                Salas = salasList!,
                SalaMateriais = salaMateriais!,
                MarcacaoMateriais = marcacaoMateriais!
            };

            var listEstados = new List<string>();

            foreach (var estado in Enum.GetValues(typeof(Estado)))
            {
                listEstados.Add(estado.ToString()!);
            }

            salasList = [.. salasList.Where(s => s.IsActive == true).OrderBy(s => s.SalaId)];

            var list = salasList.Select(s => new SelectListItem
            {
                Value = s.SalaId.ToString(),
                Text = s.Descricao
            }).ToList();

            ViewData["CategoriaId"] = new SelectList(categoriasList, "CategoriaId", "Descricao");
            ViewData["LocalId"] = new SelectList(locaisList, "LocalId", "Descricao");
            ViewData["Estados"] = new SelectList(listEstados, marcacaoEditar?.Estado);
            ViewData["SalaId"] = new SelectList(list, "Value", "Text");
            ViewBag.SalaCapacities = salasList.ToDictionary(s => s.SalaId, s => s.Capacidade);

            return PartialView("_EditMarcacaoForm", viewModel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("/Marcacoes/{id:int}/Details")]
        public async Task<IActionResult> Details(int id)
        {
            var salasList = await _salaService.GetSalasAsync();

            var marcacaoEditar = await _marcacaoService.GetMarcacaoAsync(id);

            DateTime d = (DateTime)marcacaoEditar.DataRegisto!;

            marcacaoEditar.DataRegisto = DateTime.SpecifyKind(d, DateTimeKind.Local);

            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (marcacaoEditar == null)
            {
                return userRole switch
                {
                    "Client" => Redirect("/Home/Client"),
                    "AdminCA" => Redirect("/Home/Admin"),
                    "AdminSecCA" => Redirect("/Home/AdminSecretariadoCA"),
                    "SuperAdmin" => Redirect("/Home/SuperAdmin"),
                    _ => Redirect("/Home/AdminDashboard"),
                };
            }

            var salaMateriais = await _salaService.GetSalaMateriaisAsync(marcacaoEditar.SalaId);

            var marcacaoMateriais = await _marcacaoService.GetMarcacacoesMaterialAsync(id);

            var materiaisList = await _materialService.GetMateriaisAsync();

            var locaisList = await _localService.GetLocaisAsync();

            locaisList = locaisList.Where(l => l.IsActive == true).ToList();

            var categoriasList = await _categoriaService.GetCategoriasAsync();

            var viewModel = new AdminDashboardViewModel
            {
                Locais = locaisList!,
                Categorias = categoriasList!,
                Salas = salasList!,
                Marcacao = marcacaoEditar!,
                Materiais = materiaisList!,
                SalaMateriais = salaMateriais!,
                MarcacaoMateriais = marcacaoMateriais!
            };

            return PartialView("_DetailsMarcacaoForm", viewModel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="novo"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/Marcacoes/{id:int}/Edit")]
        public async Task<IActionResult> Edit(AdminDashboardViewModel novo, int id)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            var userLocal = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Locality)?.Value;
            await _marcacaoService.PutMarcacaoAsync(novo, id, userEmail!, userRole!, int.Parse(userLocal!));

            return userRole switch
            {
                "Client" => Redirect("/Home/Client"),
                "AdminCA" => Redirect("/Home/Admin"),
                "AdminSecCA" => Redirect("/Home/AdminSecretariadoCA"),
                "SuperAdmin" => Redirect("/Home/SuperAdmin"),
                _ => Redirect("/Home/AdminDashboard"),
            };
        }
    }
}
