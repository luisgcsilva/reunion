using api.reunion.Models;

namespace api.reunion.Interfaces
{
    /// <summary>
    /// Categorias Inferface
    /// </summary>
    public interface ICategoriaRepository
    {
        ICollection<Categoria> GetCategorias();
        Categoria GetCategoria(int id);
        bool CategoriaExists(int id);
        bool CriarCategoria(Categoria categoria);
        bool AtualizarCategoria(Categoria categoria);
        bool ApagarCategoria(Categoria categoria);
        bool Save();
    }
}
