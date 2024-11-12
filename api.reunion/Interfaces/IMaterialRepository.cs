using api.reunion.Models;

namespace api.reunion.Interfaces
{
    /// <summary>
    /// Materiais Interface
    /// </summary>
    public interface IMaterialRepository
    {
        ICollection<Material> GetMateriais();
        Material GetMaterial(int id);
        bool ApagarMaterial(Material material);
        bool MaterialExists(int id);
        bool CriarMaterial(Material material);
        bool AtualizarMaterial(Material material);
        bool Save();
    }
}
