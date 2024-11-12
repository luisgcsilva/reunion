using api.reunion.Models;

namespace api.reunion.Interfaces
{
    /// <summary>
    /// SalaMateriais Interface
    /// </summary>
    public interface ISalaMaterialRepository
    {
        ICollection<SalaMaterial> GetSalaMateriais(int id);
        ICollection<SalaMaterial> GetSalaMateriais();
        SalaMaterial GetSalaMaterial(int id);
        bool SalaMaterialExists(int id);
        bool CriarSalaMaterial(SalaMaterial salaMaterial);
        bool AtualizarSalaMaterial(SalaMaterial salaMaterial);
        bool ApagarSalaMaterial(int id);
        bool Save();
    }
}
