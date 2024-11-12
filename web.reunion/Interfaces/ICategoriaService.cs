using web.reunion.Models;

namespace web.reunion.Interfaces
{
    /// <summary>
    /// Categorias interface
    /// </summary>
    public interface ICategoriaService
    {
        Task<List<Categoria>> GetCategoriasAsync();
        Task<Categoria> GetCategoriaAsync(int categoriaId);
        Task PostCategoriaAsync(Categoria categoria);
        Task PutCategoriaAsync(Categoria categoria);
        Task DeleteCategoriaAsync(int categoriaId);
    }
}
