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
    public class SalaMateriaisController(
        ISalaMaterialRepository salaMaterialRepository, 
        ISalaRepository salaRepository, 
        IMapper mapper) : ControllerBase
    {
        private readonly ISalaMaterialRepository _salaMaterialRepository = salaMaterialRepository;
        private readonly ISalaRepository _salaRepository = salaRepository;
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Devolve os materiais de uma sala
        /// </summary>
        /// <param name="id">Id da sala</param>
        /// <returns>Lista de SalaMaterial</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<SalaMaterial>))]
        public IActionResult GetSalaMaterial(int id)
        {
            if (!_salaRepository.SalaExists(id))
                return NotFound();

            var salaMateriais = _mapper.Map<List<SalaMaterialDto>>
                (_salaMaterialRepository.GetSalaMateriais(id));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(salaMateriais);
        }

        /// <summary>
        /// Devolve todos os materiais de todas as salas
        /// </summary>
        /// <returns>Lista de SalaMateriais</returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<SalaMaterial>))]
        public IActionResult GetSalaMateriais()
        {
            var salaMateriais = _mapper.Map<List<SalaMaterialDto>>
                (_salaMaterialRepository.GetSalaMateriais());

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(salaMateriais);
        }

        /// <summary>
        /// Cria um SalaMaterial
        /// </summary>
        /// <param name="salaMaterial">SalaMaterial a ser criado</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CriarSalaMaterial([FromBody] SalaMaterialDto salaMaterial)
        {
            if (salaMaterial == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();

            var salaMaterialMap = _mapper.Map<SalaMaterial>(salaMaterial);

            if (!_salaMaterialRepository.CriarSalaMaterial(salaMaterialMap))
            {
                ModelState.AddModelError("", "Erro na criação!");
                return StatusCode(500, ModelState);
            }

            return Ok("Criado com sucesso!");
        }

        /// <summary>
        /// Apaga os SalaMaterial de uma sala
        /// </summary>
        /// <param name="id">Id da sala</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteSalaMaterialBySala(int id)
        {
            if (!_salaRepository.SalaExists(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_salaMaterialRepository.ApagarSalaMaterial(id))
            {
                ModelState.AddModelError("", "Erro ao Apagar");
            }

            return NoContent();
        }
    }
}
