using api.reunion.Dto;
using api.reunion.Interfaces;
using api.reunion.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace api.reunion.Controllers
{
    [Route("api.reunion/[controller]")]
    [ApiController]
    public class LocaisController(ILocalRepository localRepository, IMapper mapper) : ControllerBase
    {
        private readonly ILocalRepository _localRepository = localRepository;
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Devolve todos os locais
        /// </summary>
        /// <returns>Lista de locais</returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Local>))]
        public IActionResult GetLocais()
        {
            var locais = _mapper.Map<List<LocalDto>>
                (_localRepository.GetLocais());

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(locais);
        }

        /// <summary>
        /// Devolve um local
        /// </summary>
        /// <param name="id">Id do local</param>
        /// <returns>Local</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Local))]
        [ProducesResponseType(400)]
        public IActionResult GetLocal(int id)
        {
            if (!_localRepository.LocalExists(id))
                return NotFound();

            var local = _mapper.Map<LocalDto>
                (_localRepository.GetLocal(id));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(local);
        }

        /// <summary>
        /// Cria um local
        /// </summary>
        /// <param name="local">Local para criar</param>
        /// <returns>Id do local criado</returns>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CriarLocal([FromBody] LocalDto local)
        {
            if (local == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();

            var localMap = _mapper.Map<Local>(local);

            if (!_localRepository.CriarLocal(localMap))
            {
                ModelState.AddModelError("", "Erro na criação");
                return StatusCode(500, ModelState);
            }

            return Ok(localMap.LocalId);
        }

        /// <summary>
        /// Atualiza um local
        /// </summary>
        /// <param name="local">Local a atualizar</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult PutLocal([FromBody] LocalDto local)
        {
            if (local == null)
                return BadRequest(ModelState);

            if (_localRepository.LocalExists(local.LocalId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var localMap = _mapper.Map<Local>(local);

            if (!_localRepository.AtualizarLocal(localMap))
            {
                ModelState.AddModelError("", "Erro na atualização do Local");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        /// <summary>
        /// Apaga um local
        /// </summary>
        /// <param name="id">Id do local a apagar</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteLocal(int id)
        {
            if (!_localRepository.LocalExists(id))
                return NotFound();

            var localToDelete = _localRepository.GetLocal(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_localRepository.ApagarLocal(localToDelete))
            {
                ModelState.AddModelError("", "Erro ao Apagar");
            }

            return NoContent();
        }
    }
}
