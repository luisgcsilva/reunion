using api.reunion.Data;
using api.reunion.Interfaces;
using api.reunion.Models;

namespace api.reunion.Repositories
{
    public class SalaMaterialRepository(AgendaformacaoContext context) : ISalaMaterialRepository
    {
        private readonly AgendaformacaoContext _context = context;

        /// <summary>
        /// Atualiza uma SalaMaterial
        /// </summary>
        /// <param name="salaMaterial">SalaMaterial atualizada</param>
        /// <returns></returns>
        public bool AtualizarSalaMaterial(SalaMaterial salaMaterial)
        {
            _context.Update(salaMaterial);
            return Save();
        }

        /// <summary>
        /// Cria uma SalaMaterial
        /// </summary>
        /// <param name="salaMaterial">SalaMaterial a ser criada</param>
        /// <returns></returns>
        public bool CriarSalaMaterial(SalaMaterial salaMaterial)
        {
            _context.Add(salaMaterial);
            return Save();
        }

        /// <summary>
        /// Devolve todas as SalaMateriais
        /// </summary>
        /// <returns>Lista de SalaMaterias</returns>
        public ICollection<SalaMaterial> GetSalaMateriais()
        {
            return [.. _context.SalaMateriais.OrderBy(s => s.SalaId)];
        }

        /// <summary>
        /// Devolve uma SalaMaterial
        /// </summary>
        /// <param name="id">Id da SalaMaterial</param>
        /// <returns>SalaMaterial</returns>
        public SalaMaterial GetSalaMaterial(int id)
        {
            return _context.SalaMateriais.Where(s => s.SalaMateriaisId == id).First();
        }

        /// <summary>
        /// Devolve todos os SalaMateriais de uma sala
        /// </summary>
        /// <param name="id">Id da sala</param>
        /// <returns>Lista de SalaMateriais</returns>
        public ICollection<SalaMaterial> GetSalaMateriais(int id)
        {
            return [.. _context.SalaMateriais.Where(s => s.SalaId == id)];
        }

        /// <summary>
        /// Verifica se existe algum SalaMaterial de uma sala
        /// </summary>
        /// <param name="id">Id da Sala</param>
        /// <returns>True ou false</returns>
        public bool SalaMaterialExists(int id)
        {
            return _context.SalaMateriais.Any(s => s.SalaId == id);
        }

        /// <summary>
        /// Apaga todos os SalaMateriais de uma sala
        /// </summary>
        /// <param name="id">Id da sala</param>
        /// <returns></returns>
        public bool ApagarSalaMaterial(int id)
        {
            var list = _context.SalaMateriais.Where(s => s.SalaId == id).ToList();
            _context.RemoveRange(list);
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
