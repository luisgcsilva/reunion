using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using web.reunion.Interfaces;
using web.reunion.Models;

namespace web.reunion.Controllers
{
    [Authorize]
    public class AdminGroupsController(
        IAdminGroupService adminGroupService) : Controller
    {
        private readonly IAdminGroupService _adminGroupService = adminGroupService;

        public IActionResult Create()
        {
            return PartialView("_CreateAdminGroupForm");
        }

        [HttpPost]
        public async Task<IActionResult> Create(AdminGroup adminGroup)
        {
            await _adminGroupService.PostAdminGroupAsync(adminGroup);

            return RedirectToAction("SuperAdmin", "Home");
        }

        [Route("/AdminGroups/{id:int}/Edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var adminGroupEditar = await _adminGroupService.GetAdminGroupAsync(id);

            return PartialView("_EditAdminGroupForm", adminGroupEditar);
        }

        [HttpPost]
        [Route("/AdminGroups/{id:int}/Edit")]
        public async Task<IActionResult> Edit(int id, AdminGroup adminGroup)
        {
            await _adminGroupService.PutAdminGroupAsync(adminGroup);

            return RedirectToAction("SuperAdmin", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _adminGroupService.DeleteAdminGroupAsync(id);

            return RedirectToAction("SuperAdmin", "Home");
        }
    }
}
