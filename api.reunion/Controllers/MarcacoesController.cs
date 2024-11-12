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
    public class MarcacoesController(
        IMarcacaoRepository marcacaoRepository, 
        IMapper mapper) : ControllerBase
    {
        private readonly IMarcacaoRepository _marcacaoRepository = marcacaoRepository;
        private readonly IMapper _mapper = mapper;

        /*
            Devolve todas as Marcações
        */
        /// <summary>
        /// Devolve todas as marcações
        /// </summary>
        /// <returns>Lista de marcações</returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Marcacao>))]
        public IActionResult GetMarcacoes()
        {
            var marcacoes = _mapper.Map<List<MarcacaoDto>>
                (_marcacaoRepository.GetMarcacoes());

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(marcacoes);
        }

        /// <summary>
        /// Devolve uma marcação
        /// </summary>
        /// <param name="id">Id da marcação</param>
        /// <returns>Marcação</returns>
        [HttpGet("marcacao/{id}")]
        [ProducesResponseType(200, Type = typeof(Marcacao))]
        [ProducesResponseType(400)]
        public IActionResult GetMarcacao(int id)
        {
            if (!_marcacaoRepository.MarcacaoExists(id))
                return NotFound();

            var marcacao = _mapper.Map<MarcacaoDto>
                (_marcacaoRepository.GetMarcacao(id));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(marcacao);
        }

        /// <summary>
        /// Devolve as marcações de um dia
        /// </summary>
        /// <param name="date">Dia</param>
        /// <returns>Lista de marcações do dia</returns>
        [HttpGet("data/{date}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Marcacao>))]
        [ProducesResponseType(400)]
        public IActionResult GetMarcacoesPorData(DateOnly date)
        {
            var marcacoes = _mapper.Map<List<MarcacaoDto>>
                (_marcacaoRepository.GetMarcacoesPorData(date));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(marcacoes);
        }

        /// <summary>
        /// Devolve as marcações com um determinado estado
        /// </summary>
        /// <param name="estado">Estado</param>
        /// <returns>Lista de marcações do estado</returns>
        [HttpGet("estado/{estado}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Marcacao>))]
        [ProducesResponseType(400)]
        public IActionResult GetMarcacoesPorEstado(string estado)
        {
            var marcacoes = _mapper.Map<List<MarcacaoDto>>
                (_marcacaoRepository.GetMarcacoesPorEstado(estado));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(marcacoes);
        }

        /// <summary>
        /// Devolve as marcações de uma sala
        /// </summary>
        /// <param name="salaId">Id da sala</param>
        /// <returns>Lista de marcações da sala</returns>
        [HttpGet("sala/{salaId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Marcacao>))]
        [ProducesResponseType(400)]
        public IActionResult GetMarcacoesPorSala(int salaId)
        {
            var marcacoes = _mapper.Map<List<MarcacaoDto>>
                (_marcacaoRepository.GetMarcacoesPorSala(salaId));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(marcacoes);
        }

        /// <summary>
        /// Devolve as marcações de um local
        /// </summary>
        /// <param name="localId">Id do local</param>
        /// <returns>Lista de marcações do local</returns>
        [HttpGet("local/{localId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Marcacao>))]
        [ProducesResponseType(400)]
        public IActionResult GetMarcacoesPorLocal(int localId)
        {
            var marcacoes = _mapper.Map<List<MarcacaoDto>>
                (_marcacaoRepository.GetMarcacoesPorLocal(localId));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(marcacoes);
        }

        /// <summary>
        /// Verifica se a sala está ocupada
        /// </summary>
        /// <param name="salaId">Id da sala</param>
        /// <param name="dia">Dia</param>
        /// <param name="horaInicio">Hora de Inicio</param>
        /// <param name="horaFim">Hora de fim</param>
        /// <param name="estado">Estado</param>
        /// <returns>True ou False</returns>
        [HttpGet("isSalaBooked/{salaId}/{dia}/{horaInicio}/{horaFim}/{estado}")]
        public IActionResult IsSalaBooked(int salaId, string dia, string horaInicio, string horaFim, string estado)
        {
            DateOnly date = DateOnly.ParseExact(dia, "yyyy-MM-dd");
            bool isBooked = _marcacaoRepository.IsSalaBooked(salaId, date, horaInicio, horaFim, estado);
            return Ok(isBooked);
        }

        /// <summary>
        /// Devolve as marcações de um utilizador
        /// </summary>
        /// <param name="utilizador">Email do utilizador</param>
        /// <returns>Lista de marcações do utilizador</returns>
        [HttpGet("utilizador/{utilizador}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Marcacao>))]
        [ProducesResponseType(400)]
        public IActionResult GetMarcacoesPorUtilizador(string utilizador)
        {
            var marcacoes = _mapper.Map<List<MarcacaoDto>>
                (_marcacaoRepository.GetMarcacoesPorUtilizador(utilizador));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(marcacoes);
        }

        /// <summary>
        /// Devolve as marcações modificadas por um administrador
        /// </summary>
        /// <param name="modificadoPor">Email do admin</param>
        /// <returns>Lista de Marcações modifcadas por admin</returns>
        [HttpGet("modificarPor/{modificadoPor}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Marcacao>))]
        [ProducesResponseType(400)]
        public IActionResult GetMarcacoesPorAdmin(string modificadoPor)
        {
            var marcacoes = _mapper.Map<List<MarcacaoDto>>
                (_marcacaoRepository.GetMarcacoesPorAdmin(modificadoPor));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(marcacoes);
        }

        /// <summary>
        /// Cria uma nova marcação
        /// </summary>
        /// <param name="marcacao">Marcação a ser criada</param>
        /// <returns>Id da nova marcação</returns>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CriarMarcacao([FromBody] MarcacaoDto marcacao)
        {
            if (marcacao == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();

            var marcacaoMap = _mapper.Map<Marcacao>(marcacao);

            if (!_marcacaoRepository.CriarMarcacao(marcacaoMap))
            {
                ModelState.AddModelError("", "Erro na Criação!");
                return StatusCode(500, ModelState);
            }

            return Ok(marcacaoMap.MarcacaoId);
        }

        /// <summary>
        /// Atualiza uma marcação
        /// </summary>
        /// <param name="marcacaoAtualizada">Marcação para ser atualizada</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult PutMarcacao([FromBody] MarcacaoDto marcacaoAtualizada)
        {
            if (marcacaoAtualizada == null)
                return BadRequest(ModelState);

            if (!_marcacaoRepository.MarcacaoExists(marcacaoAtualizada.MarcacaoId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var marcacaoMap = _mapper.Map<Marcacao>(marcacaoAtualizada);

            if (!_marcacaoRepository.AtualizarMarcacao(marcacaoMap))
            {
                ModelState.AddModelError("", "Erro na atualização da Marcação!");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        /// <summary>
        /// Apaga uma marcação
        /// </summary>
        /// <param name="id">Id da marcação a apagar</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteMarcacao(int id)
        {
            if (!_marcacaoRepository.MarcacaoExists(id))
                return NotFound();

            var marcacaoToDelete = _marcacaoRepository.GetMarcacao(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_marcacaoRepository.ApagarMarcacao(marcacaoToDelete))
            {
                ModelState.AddModelError("", "Erro ao Apagar");
            }

            return NoContent();
        }
    }
}
