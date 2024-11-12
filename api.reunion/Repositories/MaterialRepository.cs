using api.reunion.Data;
using api.reunion.Interfaces;
using api.reunion.Models;

namespace api.reunion.Repositories
{
    public class MaterialRepository(AgendaformacaoContext context) : IMaterialRepository
    {
        private readonly AgendaformacaoContext _context = context;

        /// <summary>
        /// Atualiza um material
        /// </summary>
        /// <param name="material">Material atualizado</param>
        /// <returns></returns>
        public bool AtualizarMaterial(Material material)
        {
            _context.Update(material);
            return Save();
        }

        /// <summary>
        /// Cria um material
        /// </summary>
        /// <param name="material">Material a ser criado</param>
        /// <returns></returns>
        public bool CriarMaterial(Material material)
        {
            _context.Add(material);
            return Save();
        }

        /// <summary>
        /// Devolve todos os materiais
        /// </summary>
        /// <returns>Lista de materiais</returns>
        public ICollection<Material> GetMateriais()
        {
            return [.. _context.Materiais.OrderBy(m => m.Descricao)];
        }

        /// <summary>
        /// Devolve um material
        /// </summary>
        /// <param name="id">Id do material</param>
        /// <returns>Id do material</returns>
        public Material GetMaterial(int id)
        {
            return _context.Materiais.Where(m => m.MaterialId == id).First();
        }

        /// <summary>
        /// Apaga um material
        /// </summary>
        /// <param name="material">Material a ser apagado</param>
        /// <returns></returns>
        public bool ApagarMaterial(Material material)
        {
            _context.Remove(material);
            return Save();
        }

        /// <summary>
        /// Verifica se um material existe
        /// </summary>
        /// <param name="id">Id do material</param>
        /// <returns></returns>
        public bool MaterialExists(int id)
        {
            return _context.Materiais.Any(m => m.MaterialId == id);
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
