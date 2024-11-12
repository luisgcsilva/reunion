using api.reunion.Data;
using api.reunion.Interfaces;
using api.reunion.Models;

namespace api.reunion.Repositories
{
    public class AdminGroupRepository(
        AgendaformacaoContext context) : IAdminGroupRepository
    {
        public readonly AgendaformacaoContext _context = context;

        /// <summary>
        /// Devolve todos os AdminGroups
        /// </summary>
        /// <returns>Lista de AdminGroups</returns>
        public ICollection<AdminGroup> GetAdminGroups()
        {
            return [.. _context.AdminGroups];
        }

        public bool CriarAdminGroup(AdminGroup adminGroup)
        {
            _context.Add(adminGroup);
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

        /// <summary>
        /// Devolve uma categoria
        /// </summary>
        /// <param name="id">Id da categoria</param>
        /// <returns>Categoria</returns>
        public AdminGroup GetAdminGroup(int id)
        {
            return _context.AdminGroups.Where(c => c.AdminGroupId == id).First();
        }

        /// <summary>
        /// Atualizar uma categoria
        /// </summary>
        /// <param name="categoria">Categoria atualizada</param>
        /// <returns></returns>
        public bool AtualizarAdminGroup(AdminGroup adminGroup)
        {
            _context.Update(adminGroup);
            return Save();
        }

        /// <summary>
        /// Verifica se a categoria existe
        /// </summary>
        /// <param name="id">Id da categoria</param>
        /// <returns>True ou false</returns>
        public bool AdminGroupExists(int id)
        {
            return _context.AdminGroups.Any(c => c.AdminGroupId == id);
        }

        /// <summary>
        /// Apagar uma categoria
        /// </summary>
        /// <param name="categoria">Categoria a apagar</param>
        /// <returns></returns>
        public bool ApagarAdminGroup(AdminGroup adminGroup)
        {
            _context.Remove(adminGroup);
            return Save();
        }

    }
}
