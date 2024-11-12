using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using web.reunion.Interfaces;
using web.reunion.Models;

namespace web.reunion.Controllers
{
    [Authorize]
    public class CategoriasController(
        ICategoriaService categoriaService) : Controller
    {
        private readonly ICategoriaService _categoriaService = categoriaService;
        
        public IActionResult Create()
        {
            return PartialView("_CreateCategoriaForm");
        }

        [HttpPost]
        public async Task<IActionResult> Create(Categoria categoria)
        {
            await _categoriaService.PostCategoriaAsync(categoria);

            return RedirectToAction("SuperAdmin", "Home");
        }

        [Route("/Categorias/{id:int}/Edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var categoriaEditar = await _categoriaService.GetCategoriaAsync(id);

            return PartialView("_EditCategoriaForm", categoriaEditar);
        }

        [HttpPost]
        [Route("/Categorias/{id:int}/Edit")]
        public async Task<IActionResult> Edit(Categoria categoria)
        {
            await _categoriaService.PutCategoriaAsync(categoria);

            return RedirectToAction("SuperAdmin", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoriaService.DeleteCategoriaAsync(id);

            return RedirectToAction("SuperAdmin", "Home");
        }
    }
}
