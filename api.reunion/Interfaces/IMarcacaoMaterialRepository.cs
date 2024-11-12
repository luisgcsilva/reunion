using api.reunion.Models;

namespace api.reunion.Interfaces
{
    /// <summary>
    /// MarcacaoMateriais Interface
    /// </summary>
    public interface IMarcacaoMaterialRepository
    {
        ICollection<MarcacaoMaterial> GetMarcacaoMateriais(int id);
        ICollection<MarcacaoMaterial> GetMarcacoesMateriais();
        MarcacaoMaterial GetMarcacaoMaterial(int id);
        bool MarcacaoMaterialExists(int id);
        bool CriarMarcacaoMaterial(MarcacaoMaterial marcacaoMaterial);
        bool AtualizarMarcacaoMaterial(MarcacaoMaterial marcacaoMaterial);
        bool ApagarMarcacaoMaterial(int id);
        bool Save();
    }
}
