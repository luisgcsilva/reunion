using api.reunion.Dto;
using api.reunion.Interfaces;
using api.reunion.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.reunion.Controllers
{
    [Route("api.reunion/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriasController(ICategoriaRepository categoriaRepository, IMapper mapper) : ControllerBase
    {
        private readonly ICategoriaRepository _categoriaRepository = categoriaRepository;
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Devolve todas as categorias
        /// </summary>
        /// <returns>Lista de categorias</returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Categoria>))]
        public IActionResult GetCategorias()
        {
            var categorias = _mapper.Map<List<CategoriaDto>>
                (_categoriaRepository.GetCategorias());

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(categorias);
        }

        /// <summary>
        /// Devolve uma categoria
        /// </summary>
        /// <param name="id">Id da categoria desejada</param>
        /// <returns>Categoria</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Categoria))]
        [ProducesResponseType(400)]
        public IActionResult GetCategoria(int id)
        {
            if (!_categoriaRepository.CategoriaExists(id))
                return NotFound();

            var categoria = _mapper.Map<CategoriaDto>
                (_categoriaRepository.GetCategoria(id));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(categoria);
        }

        /// <summary>
        /// Cria uma nova categoria
        /// </summary>
        /// <param name="categoria">Categoria a ser guardada</param>
        /// <returns>Id da categoria</returns>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CriarCategoria([FromBody] CategoriaDto categoria)
        {
            if (categoria == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();

            var categoriaMap = _mapper.Map<Categoria>(categoria);

            if (!_categoriaRepository.CriarCategoria(categoriaMap))
            {
                ModelState.AddModelError("", "Erro na criação!");
                return StatusCode(500, ModelState);
            }

            return Ok(categoriaMap.CategoriaId);
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
        public IActionResult PutCategoria([FromBody] CategoriaDto categoria)
        {
            if (categoria == null)
                return BadRequest(ModelState);

            if (!_categoriaRepository.CategoriaExists(categoria.CategoriaId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var categoriaMap = _mapper.Map<Categoria>(categoria);

            if (!_categoriaRepository.AtualizarCategoria(categoriaMap))
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
        public IActionResult DeleteCategoria(int id)
        {
            if (!_categoriaRepository.CategoriaExists(id))
                return NotFound();

            var categoriaToDelete = _categoriaRepository.GetCategoria(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_categoriaRepository.ApagarCategoria(categoriaToDelete))
            {
                ModelState.AddModelError("", "Erro ao Apagar");
            }

            return NoContent();
        }
    }
}
