using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using api.reunion.Dto;
using api.reunion.Interfaces;
using api.reunion.Models;

namespace api.reunion.Controllers
{
    [Route("api.reunion/[controller]")]
    [ApiController]
    [Authorize]
    public class MateriaisController(IMaterialRepository materialRepository, IMapper mapper) : ControllerBase
    {
        private readonly IMaterialRepository _materialRepository = materialRepository;
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Devolve todos os materiais
        /// </summary>
        /// <returns>Lista de materiais</returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Material>))]
        public IActionResult GetMateriais()
        {
            var materiais = _mapper.Map<List<MaterialDto>>
                (_materialRepository.GetMateriais());

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(materiais);
        }

        /// <summary>
        /// Devolve um material
        /// </summary>
        /// <param name="id">Id do material</param>
        /// <returns>Material</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Material))]
        [ProducesResponseType(400)]
        public IActionResult GetMaterial(int id)
        {
            if (!_materialRepository.MaterialExists(id))
                return NotFound();

            var material = _mapper.Map<MaterialDto>
                (_materialRepository.GetMaterial(id));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(material);
        }

        /*
            Cria um novo material

            @material -> MaterialDto  
        */
        /// <summary>
        /// Criar um material
        /// </summary>
        /// <param name="material">Material a criar</param>
        /// <returns>Id do material criado</returns>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CriarMaterial([FromBody] MaterialDto material)
        {
            if (material == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();

            var materialMap = _mapper.Map<Material>(material);

            if (!_materialRepository.CriarMaterial(materialMap))
            {
                ModelState.AddModelError("", "Erro na criação!");
                return StatusCode(500, ModelState);
            }

            return Ok(materialMap.MaterialId);
        }

        /// <summary>
        /// Atualiza um material
        /// </summary>
        /// <param name="materialAtualizado">Material a ser utilizado</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult PutMaterial([FromBody] MaterialDto materialAtualizado)
        {
            if (materialAtualizado == null)
                return BadRequest(ModelState);

            if (!_materialRepository.MaterialExists(materialAtualizado.MaterialId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var materialMap = _mapper.Map<Material>(materialAtualizado);

            if (!_materialRepository.AtualizarMaterial(materialMap))
            {
                ModelState.AddModelError("", "Erro na atualização do Material");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        /// <summary>
        /// Apaga um material
        /// </summary>
        /// <param name="id">Id do material a apagar</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteMaterial(int id)
        {
            if (!_materialRepository.MaterialExists(id))
                return NotFound();

            var materialToDelete = _materialRepository.GetMaterial(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_materialRepository.ApagarMaterial(materialToDelete))
            {
                ModelState.AddModelError("", "Erro ao Apagar");
            }

            return NoContent();
        }
    }
}
