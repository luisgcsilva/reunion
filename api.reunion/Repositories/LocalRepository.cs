using api.reunion.Data;
using api.reunion.Interfaces;
using api.reunion.Models;

namespace api.reunion.Repositories
{
    public class LocalRepository(AgendaformacaoContext context) : ILocalRepository
    {
        private readonly AgendaformacaoContext _context = context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="local"></param>
        /// <returns></returns>
        public bool ApagarLocal(Local local)
        {
            _context.Remove(local);
            return Save();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="local"></param>
        /// <returns></returns>
        public bool AtualizarLocal(Local local)
        {
            _context.Update(local);
            return Save();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="local"></param>
        /// <returns></returns>
        public bool CriarLocal(Local local)
        {
            _context.Add(local);
            return Save();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ICollection<Local> GetLocais()
        {
            return [.. _context.Locais];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Local GetLocal(int id)
        {
            return _context.Locais.Where(l => l.LocalId == id).First();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool LocalExists(int id)
        {
            return _context.Locais.Any(l => l.LocalId == id);
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
