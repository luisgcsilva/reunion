using api.reunion.Dto;
using api.reunion.Interfaces;
using api.reunion.Models;
using api.reunion.Repositories;
using api.reunion.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace api.reunion.Controllers
{
    [Route("api.reunion/[controller]")]
    [ApiController]
    public class AdminGroupsController(IAdminGroupRepository adminGroupRepository,
        IAdminGroupService adminGroupService,
        IMapper mapper) : ControllerBase
    {
        private readonly IAdminGroupRepository _adminGroupRepository = adminGroupRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IAdminGroupService _adminGroupService = adminGroupService;

        /// <summary>
        /// Get Admin Groups from the database
        /// </summary>
        /// <returns>List of AdminGroups</returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<AdminGroup>))]
        public IActionResult GetAdminGroups()
        {
            var adminGroups = _mapper.Map<List<AdminGroup>>
                        (_adminGroupRepository.GetAdminGroups());

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(adminGroups);
        }

        /// <summary>
        /// Devolve uma categoria
        /// </summary>
        /// <param name="id">Id da categoria desejada</param>
        /// <returns>Categoria</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(AdminGroup))]
        [ProducesResponseType(400)]
        public IActionResult GetAdminGroup(int id)
        {
            if (!_adminGroupRepository.AdminGroupExists(id))
                return NotFound();

            var adminGroup = _mapper.Map<AdminGroupDto>
                (_adminGroupRepository.GetAdminGroup(id));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(adminGroup);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CriarAdminGroup([FromBody] AdminGroupDto adminGroupDto)
        {
            if (adminGroupDto == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();

            var adminGroupMap = _mapper.Map<AdminGroup>(adminGroupDto);

            if (!_adminGroupRepository.CriarAdminGroup(adminGroupMap))
            {
                ModelState.AddModelError("", "Erro na criação!");
                return StatusCode(500, ModelState);
            }

            return Ok(adminGroupMap.AdminGroupId);
        }

        /// <summary>
        /// Atualiza uma categoria
        /// </summary>
        /// <param name="categoria">Categoria a ser editada</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult PutAdminGroup([FromBody] AdminGroupDto adminGroupDto)
        {
            if (adminGroupDto == null)
                return BadRequest(ModelState);

            if (!_adminGroupRepository.AdminGroupExists(adminGroupDto.AdminGroupId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var adminGroupMap = _mapper.Map<AdminGroup>(adminGroupDto);

            if (!_adminGroupRepository.AtualizarAdminGroup(adminGroupMap))
            {
                ModelState.AddModelError("", "Erro na atualização do Categoria");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        /// <summary>
        /// Apaga uma categoria
        /// </summary>
        /// <param name="id">Id da categoria a apagar</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteAdminGroup(int id)
        {
            if (!_adminGroupRepository.AdminGroupExists(id))
                return NotFound();

            var adminGroupToDelete = _adminGroupRepository.GetAdminGroup(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_adminGroupRepository.ApagarAdminGroup(adminGroupToDelete))
            {
                ModelState.AddModelError("", "Erro ao Apagar");
            }

            return NoContent();
        }

        /// <summary>
        /// Verify the User Role 
        /// </summary>
        /// <param name="model">UserLocationModel contains Username and the Selected Local</param>
        /// <returns></returns>
        [HttpPost("verify-role")]
        public IActionResult VerifyUserRole([FromBody] UserLocationModel model)
        {
            try
            {
                var role = _adminGroupService.VerifyUserRole(model.Username, model.SelectedLocal);
                return Ok(new { Username = model.Username, Role = role });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
