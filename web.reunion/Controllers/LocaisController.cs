using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using web.reunion.Interfaces;
using web.reunion.Models;

namespace web.reunion.Controllers
{
    public class LocaisController(
        ILocalService localService,
        IAdminGroupService adminGroupService) : Controller
    {
        private readonly ILocalService _localService = localService;
        private readonly IAdminGroupService _adminGroupService = adminGroupService;

        public async Task<IActionResult> Create()
        {
            var adminGroupsList = await _adminGroupService.GetAdminGroupsAsync();

            adminGroupsList = adminGroupsList.Where(a => a.Grupo != "AdminCA").Where(a => a.Grupo != "AdminSecCA").Where(a => a.Grupo != "SuperAdmin").ToList();

            ViewData["AdminGroupId"] = new SelectList(adminGroupsList, "AdminGroupId", "Grupo");

            return PartialView("_CreateLocalForm");
        }

        [HttpPost]
        public async Task<IActionResult> Create(Local local)
        {
            local.IsActive = true;

            await _localService.PostLocalAsync(local);

            return RedirectToAction("SuperAdmin", "Home");
        }

        [Route("/Locais/{id:int}/Edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var localEditar = await _localService.GetLocalAsync(id);
            
            var adminGroupsList = await _adminGroupService.GetAdminGroupsAsync();

            adminGroupsList = adminGroupsList.Where(a => a.Grupo != "AdminCA").Where(a => a.Grupo != "AdminSecCA").Where(a => a.Grupo != "SuperAdmin").ToList();

            ViewData["AdminGroupId"] = new SelectList(adminGroupsList, "AdminGroupId", "Grupo", localEditar?.AdminGroupId);

            return PartialView("_EditLocalForm", localEditar);
        }

        [HttpPost]
        [Route("/Locais/{id:int}/Edit")]
        public async Task<IActionResult> Edit(int id, Local local)
        {
                await _localService.PutLocalAsync(local);

            return RedirectToAction("SuperAdmin", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _localService.DeleteLocalAsync(id);

            return RedirectToAction("SuperAdmin", "Home");
        }
    }
}
