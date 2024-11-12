using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.Security.Claims;
using web.reunion.Interfaces;
using web.reunion.Models;
using Microsoft.IdentityModel.Tokens;
using System.Data;

namespace web.reunion.Controllers
{
    [Authorize]
    public class HomeController(IMarcacaoService marcacaoService,
        IMaterialService materialService,
        ILocalService localService,
        IAdminGroupService adminGroupService,
        ICategoriaService categoriaService,
        ISalaService salaService) : Controller
    {
        private readonly ILocalService _localService = localService;
        private readonly ISalaService _salaService = salaService;
        private readonly IAdminGroupService _adminGroupService = adminGroupService;
        private readonly ICategoriaService _categoriaService = categoriaService;
        private readonly IMaterialService _materialService = materialService;
        private readonly IMarcacaoService _marcacaoService = marcacaoService;

        /// <summary>
        /// Método para abrir a página Inicial
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            if (User.Identity!.IsAuthenticated)
            {
                var locaisList = await _localService.GetLocaisAsync();

                var localCA = locaisList.Where(l => l.Descricao == "CA").First();

                locaisList = locaisList.Where(l => l.IsActive == true).ToList();

                var user = User;

                var local = "";

                var localId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Locality)!.Value);

                if (!localCA.Descricao.IsNullOrEmpty())
                {
                    local = localCA.Descricao;
                }
                else
                {
                    local = locaisList.Where(s => s.LocalId == localId).First().Descricao;
                }

                var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                return role switch
                {
                    "Client" => RedirectToAction("Client"),
                    "AdminCA" => RedirectToAction("Admin"),
                    "AdminSecCA" => RedirectToAction("AdminSecretariadoCA"),
                    "SuperAdmin" => Redirect("/Home/SuperAdmin"),
                    _ => RedirectToAction("AdminDashboard"),
                };
            }
            return View();
        }

        /// <summary>
        /// Método para redirecionar utilizadores conforme a sua role
        /// </summary>
        /// <returns></returns>
        public IActionResult RedirecionarUser()
        {
            var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            return role switch
            {
                "Client" => RedirectToAction("Client"),
                "AdminCA" => RedirectToAction("Admin"),
                "AdminSecCA" => RedirectToAction("AdminSecretariadoCA"),
                "SuperAdmin" => Redirect("/Home/SuperAdmin"),
                _ => RedirectToAction("AdminDashboard"),
            };
        }

        /// <summary>
        /// Método para abrir a página de Cliente, qualquer utilizador que não seja administrador
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> Client()
        {
            if (User.Claims.Any())
            {
                var nome = User.FindFirst("FullName")?.Value;
            }

            var local = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Locality)?.Value!);

            var salasList = await _salaService.GetSalasAsync();

            var viewModel = new AdminDashboardViewModel
            {
                Salas = [.. salasList.Where(s => s.IsActive == true).Where(s => s.LocalId == local).OrderBy(s => s.Descricao)]
            };

            return View(viewModel);
        }

        /// <summary>
        /// Método para abrir a página de Administrador do Conselho de Administração
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "AdminCA")]
        public async Task<IActionResult> Admin()
        {
            var salasList = await _salaService.GetSalasAsync();

            var marcacoesList = await _marcacaoService.GetMarcacoesAsync();

            var locaisList = await _localService.GetLocaisAsync();

            locaisList = locaisList.Where(l => l.IsActive == true).ToList();

            var local = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Locality)?.Value!);

            var viewModel = new AdminDashboardViewModel
            {
                Locais = [.. locaisList.OrderBy(l => l.Descricao)],
                Salas = [.. salasList.OrderBy(s => s.Descricao)],
                Marcacoes = marcacoesList,
            };

            ViewData["LocalId"] = new SelectList(locaisList, "LocalId", "Descricao", local);

            return View(viewModel);
        }

        /// <summary>
        /// Método para abrir a página de Administrador do Conselho de Administração
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> SuperAdmin()
        {
            var salasList = await _salaService.GetSalasAsync();

            var materiaisList = await _materialService.GetMateriaisAsync();
            
            var adminGroupsList = await _adminGroupService.GetAdminGroupsAsync();

            adminGroupsList = adminGroupsList.Where(a => a.Grupo != "AdminCA").Where(a => a.Grupo != "AdminSecCA").Where(a => a.Grupo != "SuperAdmin").ToList();

            var marcacoesList = await _marcacaoService.GetMarcacoesAsync();

            var categoriasList = await _categoriaService.GetCategoriasAsync();

            var locaisList = await _localService.GetLocaisAsync();

            locaisList = locaisList.Where(l => l.IsActive == true).ToList();

            var local = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Locality)?.Value!);

            var viewModel = new AdminDashboardViewModel
            {
                Locais = [.. locaisList.OrderBy(l => l.Descricao)],
                Salas = [.. salasList.OrderBy(s => s.Descricao)],
                Materiais = materiaisList,
                AdminGroups = adminGroupsList,
                Categorias = categoriasList,
                Marcacoes = marcacoesList,
            };

            ViewData["LocalId"] = new SelectList(locaisList, "LocalId", "Descricao", local);

            return View(viewModel);
        }


        /// <summary>
        /// Método para abrir a página de Administrador do Secretariado do Conselhe de Administração
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "AdminSecCA")]
        public async Task<IActionResult> AdminSecretariadoCA()
        {
            var salasList = await _salaService.GetSalasAsync();

            var marcacoesList = await _marcacaoService.GetMarcacoesAsync();
            
            var locaisList = await _localService.GetLocaisAsync();

            locaisList = locaisList.Where(l => l.IsActive == false).ToList();

            var local = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Locality)?.Value!);

            var viewModel = new AdminDashboardViewModel
            {
                Locais = [.. locaisList.OrderBy(l => l.Descricao)],
                Salas = [.. salasList.OrderBy(s => s.Descricao)],
                Marcacoes = marcacoesList,
            };

            ViewData["LocalId"] = new SelectList(locaisList, "LocalId", "Descricao", local);

            return View(viewModel);
        }

        /// <summary>
        /// Método para mostrar a págind do Painel de Administrador
        /// </summary>
        /// <returns></returns>
        [Authorize(Policy = "Admins")]
        public async Task<IActionResult> AdminDashboard()
        {
            var salasList = await _salaService.GetSalasAsync();

            var materiaisList = await _materialService.GetMateriaisAsync();

            var marcacoesList = await _marcacaoService.GetMarcacoesAsync();

            var locaisList = await _localService.GetLocaisAsync();

            var local = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Locality)?.Value!);

            locaisList = locaisList.Where(l => l.IsActive == true).ToList();

            var viewModel = new AdminDashboardViewModel
            {
                Locais = locaisList,
                Salas = [.. salasList.Where(s => s.LocalId == local).OrderBy(s => s.Descricao)],
                Marcacoes = marcacoesList,
                Materiais = materiaisList,
            };

            return View(viewModel);
        }

        /// <summary>
        /// Método para mostrar a página de Privacidade
        /// </summary>
        /// <returns></returns>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Método para mostar a página de erro
        /// </summary>
        /// <returns></returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
