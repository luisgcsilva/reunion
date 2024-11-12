using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using web.reunion.Models;
using web.reunion.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using System.Data;

namespace web.reunion.Controllers
{
    [Authorize]
    public class SalasController(
        IMaterialService materialService,
        ILocalService localService,
        ISalaService salaService) : Controller
    {
        private readonly ISalaService _salaService = salaService;
        private readonly IMaterialService _materialService = materialService;
        private readonly ILocalService _localService = localService;

        /// <summary>
        /// Método para abrir a view para criar uma nova sala
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Create()
        {
            var materiaisList = await _materialService.GetMateriaisAsync();

            var locaisList = await _localService.GetLocaisAsync();
            
            locaisList = locaisList.Where(l => l.IsActive == true).ToList();

            var local = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Locality)?.Value;

            var viewModel = new EditSalaViewModel
            {
                Sala = new Sala
                {
                    LocalId = int.Parse(local!)
                },
                Materiais = materiaisList
            };

            ViewData["LocalId"] = new SelectList(locaisList, "LocalId", "Descricao", local);

            return PartialView("_CreateSalaForm", viewModel);
        }

        /// <summary>
        /// Método para receber as salas de um determinado local
        /// </summary>
        /// <param name="localId">Id do local</param>
        /// <param name="isActive">Se é um local para estar ativo ou não</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetSalasByLocal(int? localId, bool isActive)
        {
            var salas = await _salaService.GetSalasAsync();

            if (isActive == true)
            {
                if (localId == null)
                {
                    var salasList = salas.Where(s => s.IsActive == isActive).OrderBy(s => s.Descricao).Select(s => new
                    {
                        SalaId = s.SalaId,
                        Descricao = s.Descricao
                    }).ToList();

                    return Json(salasList);
                }
                else
                {
                    var salasList = salas.Where(s => s.LocalId == localId && s.IsActive == isActive).OrderBy(s => s.Descricao).Select(s => new
                    {
                        SalaId = s.SalaId,
                        Descricao = s.Descricao
                    }).ToList();

                    return Json(salasList);
                }
            } else
            {
                if (localId == null)
                {
                    var salasList = salas.OrderBy(s => s.Descricao).Select(s => new
                    {
                        SalaId = s.SalaId,
                        Descricao = s.Descricao
                    }).ToList();

                    return Json(salasList);
                }
                else
                {
                    var salasList = salas.Where(s => s.LocalId == localId).OrderBy(s => s.Descricao).Select(s => new
                    {
                        SalaId = s.SalaId,
                        Descricao = s.Descricao
                    }).ToList();

                    return Json(salasList);
                }
            }
        }

        /// <summary>
        /// Método para enviar o pedido para criar uma sala nova
        /// </summary>
        /// <param name="viewModel">Modelo com os dados da sala para criar</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(EditSalaViewModel viewModel)
        {
            await _salaService.PostSalaAsync(viewModel);
         
            var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            return role switch
            {
                "SuperAdmin" => RedirectToAction("SuperAdmin", "Home"),
                _ => RedirectToAction("AdminDashboard", "Home"),
            };
        }

        /// <summary>
        /// Método para abrir a view para editar uma sala
        /// </summary>
        /// <param name="id">Id da sala</param>
        /// <returns></returns>
        [Route("/Salas/{id:int}/Edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var salaEditar = await _salaService.GetSalaAsync(id);

            var salaMateriais = await _salaService.GetSalaMateriaisAsync(id);

            var materiais = await _materialService.GetMateriaisAsync();

            var locaisList = await _localService.GetLocaisAsync();

            locaisList = locaisList.Where(l => l.IsActive == true).ToList();

            var viewModel = new EditSalaViewModel
            {
                Locais = locaisList,
                Sala = salaEditar,
                SalaMateriais = salaMateriais,
                Materiais = materiais
            };

            ViewData["LocalId"] = new SelectList(locaisList, "LocalId", "Descricao", salaEditar?.LocalId);
            return PartialView("_EditSalaForm", viewModel);
        }

        /// <summary>
        /// Método para enviar o pedido para atualizar uma sala
        /// </summary>
        /// <param name="id">Id da sala</param>
        /// <param name="viewModel">Modelo com os dados da sala atualiazada</param>
        /// <returns></returns>
        [HttpPost]
        [Route("/Salas/{id:int}/Edit")]
        public async Task<IActionResult> Edit(int id, EditSalaViewModel viewModel)
        {
            await _salaService.PutSalaAsync(id, viewModel);

            var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            return role switch
            {
                "SuperAdmin" => RedirectToAction("SuperAdmin", "Home"),
                _ => RedirectToAction("AdminDashboard", "Home"),
            };
        }
    }
}
