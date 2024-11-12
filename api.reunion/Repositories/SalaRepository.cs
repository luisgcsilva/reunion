using api.reunion.Data;
using api.reunion.Interfaces;
using api.reunion.Models;

namespace api.reunion.Repositories
{
    public class SalaRepository(AgendaformacaoContext context) : ISalaRepository
    {
        private readonly AgendaformacaoContext _context = context;

        /// <summary>
        /// Devolve todas as Salas
        /// </summary>
        /// <returns>Lista de Salas</returns>
        public ICollection<Sala> GetSalas()
        {
            return [.. _context.Salas.Where(s => s.Descricao != "Pendente").OrderBy(s => s.SalaId)];
        }

        /// <summary>
        /// Devolve as salas de um local
        /// </summary>
        /// <param name="localId">Id do local</param>
        /// <returns>Lista de salas</returns>
        public ICollection<Sala> GetSalasPorLocal(int localId)
        {
            return [.. _context.Salas.Where(s => s.Descricao != "Pendente").Where(s => s.LocalId == localId)];
        }

        /// <summary>
        /// Devolve uma sala
        /// </summary>
        /// <param name="id">Id da sala</param>
        /// <returns>Sala</returns>
        public Sala GetSala(int id)
        {
            return _context.Salas.Where(s => s.SalaId == id).First();
        }

        /// <summary>
        /// Verifica se uma sala existe
        /// </summary>
        /// <param name="id">Id da sala</param>
        /// <returns></returns>
        public bool SalaExists(int id)
        {
            return _context.Salas.Any(s => s.SalaId == id);
        }

        /// <summary>
        /// Cria uma sala
        /// </summary>
        /// <param name="sala">Sala a ser criada</param>
        /// <returns></returns>
        public bool CriarSala(Sala sala)
        {
            _context.Add(sala);
            return Save();
        }

        /// <summary>
        /// Atualiza uma sala
        /// </summary>
        /// <param name="sala">Sala atualizada</param>
        /// <returns></returns>
        public bool AtualizarSala(Sala sala)
        {
            _context.Update(sala);
            return Save();
        }

        /// <summary>
        /// Apaga uma sala
        /// </summary>
        /// <param name="sala">Sala a ser apagada</param>
        /// <returns></returns>
        public bool ApagarSala(Sala sala)
        {
            _context.Remove(sala);
            return Save();
        }

        /// <summary>
        /// Guarda e verifica se as alterações foram bem sucedidas
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }
    }
}
