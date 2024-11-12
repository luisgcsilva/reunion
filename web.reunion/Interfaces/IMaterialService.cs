using web.reunion.Models;

namespace web.reunion.Interfaces
{
    /// <summary>
    /// Materiais Interface
    /// </summary>
    public interface IMaterialService
    {
        Task<List<Material>> GetMateriaisAsync();
        Task<Material> GetMaterialAsync(int materialId);
        Task PostMaterialAsync(Material materialDto);
        Task PutMaterialAsync(Material materialDto);
        Task DeleteMaterialAsync(int materialId);
    }
}
