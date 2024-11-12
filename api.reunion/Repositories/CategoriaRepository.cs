using api.reunion.Data;
using api.reunion.Interfaces;
using api.reunion.Models;

namespace api.reunion.Repositories
{
    public class CategoriaRepository(AgendaformacaoContext context) : ICategoriaRepository
    {
        private readonly AgendaformacaoContext _context = context;

        /// <summary>
        /// Apagar uma categoria
        /// </summary>
        /// <param name="categoria">Categoria a apagar</param>
        /// <returns></returns>
        public bool ApagarCategoria(Categoria categoria)
        {
            _context.Remove(categoria);
            return Save();
        }

        /// <summary>
        /// Atualizar uma categoria
        /// </summary>
        /// <param name="categoria">Categoria atualizada</param>
        /// <returns></returns>
        public bool AtualizarCategoria(Categoria categoria)
        {
            _context.Update(categoria);
            return Save();
        }

        /// <summary>
        /// Verifica se a categoria existe
        /// </summary>
        /// <param name="id">Id da categoria</param>
        /// <returns>True ou false</returns>
        public bool CategoriaExists(int id)
        {
            return _context.Categorias.Any(c => c.CategoriaId == id);
        }

        /// <summary>
        /// Criar uma categoria
        /// </summary>
        /// <param name="categoria">Categoria a criar</param>
        /// <returns></returns>
        public bool CriarCategoria(Categoria categoria)
        {
            _context.Add(categoria);
            return Save();
        }

        /// <summary>
        /// Devolve uma categoria
        /// </summary>
        /// <param name="id">Id da categoria</param>
        /// <returns>Categoria</returns>
        public Categoria GetCategoria(int id)
        {
            return _context.Categorias.Where(c => c.CategoriaId == id).First();
        }

        /// <summary>
        /// Devolve todas as categorias
        /// </summary>
        /// <returns>Lista de categorias</returns>
        public ICollection<Categoria> GetCategorias()
        {
            return [.. _context.Categorias];
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
