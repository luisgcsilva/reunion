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
    public class MarcacaoMateriaisController(
        IMarcacaoMaterialRepository marcacaoMaterialRepository,
        IMarcacaoRepository marcacaoRepository,
        IMapper mapper) : ControllerBase
    {
        private readonly IMarcacaoMaterialRepository _marcacaoMaterialRepository = marcacaoMaterialRepository;
        private readonly IMarcacaoRepository _marcacaoRepository = marcacaoRepository;
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Devolve os materiais de uma marcacao 
        /// <param name="id">Id da marcacao</param>
        /// </summary>
        /// <returns>Lista de MarcacaoMateirais</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<MarcacaoMaterial>))]
        public IActionResult GetMarcacaoMateriais(int id)
        {
            if (!_marcacaoRepository.MarcacaoExists(id))
                return NotFound();

            var marcacaoMateriais = _mapper.Map<List<MarcacaoMaterialDto>>
                (_marcacaoMaterialRepository.GetMarcacaoMateriais(id));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(marcacaoMateriais);
        }

        /// <summary>
        /// Devolve todos os materiais de todas as marcações
        /// </summary>
        /// <returns>Lista de MarcacaoMateriais</returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<MarcacaoMaterial>))]
        public IActionResult GetMarcacoesMateriais()
        {
            var marcacaoMateriais = _mapper.Map<List<MarcacaoMaterialDto>>
                (_marcacaoMaterialRepository.GetMarcacoesMateriais());

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(marcacaoMateriais);
        }

        /// <summary>
        /// Cria uma nova Marcacao Material
        /// </summary>
        /// <param name="marcacaoMaterial">MarcacaoMaterial a ser criado</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CriarMarcacaoMaterial([FromBody] MarcacaoMaterialDto marcacaoMaterial)
        {
            if (marcacaoMaterial == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();

            var marcacaoMaterialMap = _mapper.Map<MarcacaoMaterial>(marcacaoMaterial);

            if (!_marcacaoMaterialRepository.CriarMarcacaoMaterial(marcacaoMaterialMap))
            {
                ModelState.AddModelError("", "Erro na criação!");
                return StatusCode(500, ModelState);
            }

            return Ok("Criado com sucesso!");
        }


        /// <summary>
        /// Apaga uma MarcacaoMaterial
        /// </summary>
        /// <param name="id">Id da MarcacaoMaterial a apahar</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteMarcacaoMaterialBySala(int id)
        {
            if (!_marcacaoMaterialRepository.MarcacaoMaterialExists(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_marcacaoMaterialRepository.ApagarMarcacaoMaterial(id))
            {
                ModelState.AddModelError("", "Erro ao Apagar");
            }

            return NoContent();
        }
    }
}
