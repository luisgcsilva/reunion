using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using web.reunion.Models;
using web.reunion.Interfaces;
using System.Security.Claims;

namespace web.reunion.Controllers
{
    [Authorize]
    public class MateriaisController(
        IMaterialService materialService) : Controller
    {
        private readonly IMaterialService _materialService = materialService;

        /// <summary>
        /// Método para abrir a view para cria um material novo
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            return PartialView("_CreateMaterialForm");
        }

        /// <summary>
        /// Método para enviar o pedido para criar um material novo
        /// </summary>
        /// <param name="material"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(Material material)
        {
            await _materialService.PostMaterialAsync(material);

            var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            return role switch
            {
                "SuperAdmin" => RedirectToAction("SuperAdmin", "Home"),
                _ => RedirectToAction("AdminDashboard", "Home"),
            };
        }

        /// <summary>
        /// Método para abrir a view de editar um material
        /// </summary>
        /// <param name="id">Id do material a editar</param>
        /// <returns></returns>
        [Route("/Materiais/{id:int}/Edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var materialEditar = await _materialService.GetMaterialAsync(id);

            return PartialView("_EditMaterialForm", materialEditar);
        }

        /// <summary>
        /// Método para enviar o pedido para atualizar um material
        /// </summary>
        /// <param name="materialDto">Material Atualizado</param>
        /// <returns></returns>
        [HttpPost]
        [Route("/Materiais/{id:int}/Edit")]
        public async Task<IActionResult> Edit(Material materialDto)
        {
            await _materialService.PutMaterialAsync(materialDto);

            var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            return role switch
            {
                "SuperAdmin" => RedirectToAction("SuperAdmin", "Home"),
                _ => RedirectToAction("AdminDashboard", "Home"),
            };
        }

        /// <summary>
        /// Método para enviar o pedido para apagar um material
        /// </summary>
        /// <param name="id">Id do Material</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _materialService.DeleteMaterialAsync(id);

            var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            return role switch
            {
                "SuperAdmin" => RedirectToAction("SuperAdmin", "Home"),
                _ => RedirectToAction("AdminDashboard", "Home"),
            };
        }
    }
}
