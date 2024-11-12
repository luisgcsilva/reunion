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
    public class SalasController(ISalaRepository salaRepository, IMapper mapper) : ControllerBase
    {
        private readonly ISalaRepository _salaRepository = salaRepository;
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Devolve todas as salas
        /// </summary>
        /// <returns>Lista de salas</returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Sala>))]
        public IActionResult GetSalas()
        {
            var salas = _mapper.Map<List<SalaDto>>
                (_salaRepository.GetSalas());

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(salas);
        }

        /// <summary>
        /// Devolve uma sala
        /// </summary>
        /// <param name="id">Id da sala</param>
        /// <returns>Sala</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Sala))]
        [ProducesResponseType(400)]
        public IActionResult GetSala(int id)
        {
            if (!_salaRepository.SalaExists(id))
                return NotFound();

            var sala = _mapper.Map<SalaDto>
                (_salaRepository.GetSala(id));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(sala);
        }


        /// <summary>
        /// Devolve as salas de um local
        /// </summary>
        /// <param name="localId">Id do local</param>
        /// <returns>Lista de salas</returns>
        [HttpGet("local/{localId}")]
        [ProducesResponseType(200, Type = typeof(Sala))]
        [ProducesResponseType(400)]
        public IActionResult GetSalasPorLocal(int localId)
        {
            var salas = _mapper.Map<List<SalaDto>>
                (_salaRepository.GetSalasPorLocal(localId));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(salas);
        }

        /// <summary>
        /// Cria uma sala
        /// </summary>
        /// <param name="sala">Sala a criar</param>
        /// <returns>Id da sala criada</returns>
        [HttpPost]
        [ProducesResponseType(typeof(SalaDto), 200)]
        [ProducesResponseType(400)]
        public IActionResult CriarSala([FromBody] SalaDto sala)
        {
            if (sala == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();
 
            var salaMap = _mapper.Map<Sala>(sala);

            if (!_salaRepository.CriarSala(salaMap))
            {
                ModelState.AddModelError("", "Erro na Criação!");
                return StatusCode(500, ModelState);
            }

            return Ok(salaMap.SalaId);
        }

        /// <summary>
        /// Atualiza uma sala
        /// </summary>
        /// <param name="salaAtualizada">Sala a atualizar</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult PutSala([FromBody] SalaDto salaAtualizada)
        {
            if (salaAtualizada == null)
                return BadRequest(ModelState);

            if (!_salaRepository.SalaExists(salaAtualizada.SalaId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var salaMap = _mapper.Map<Sala>(salaAtualizada);

            if (!_salaRepository.AtualizarSala(salaMap))
            {
                ModelState.AddModelError("", "Erro na atualização da Sala");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        /// <summary>
        /// Apaga uma sala
        /// </summary>
        /// <param name="id">Id da sala a apagar</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteSala(int id)
        {
            if (!_salaRepository.SalaExists(id))
                return NotFound();

            var salaToDelete = _salaRepository.GetSala(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_salaRepository.ApagarSala(salaToDelete))
            {
                ModelState.AddModelError("", "Erro ao Apagar");
            }

            return NoContent();
        }
    }
}
