using api.reunion.Models;

namespace api.reunion.Interfaces
{
    /// <summary>
    /// Locais Interface
    /// </summary>
    public interface ILocalRepository
    {
        ICollection<Local> GetLocais();
        Local GetLocal(int id);
        bool LocalExists(int id);
        bool CriarLocal(Local local);
        bool AtualizarLocal(Local local);
        bool ApagarLocal(Local local);
        bool Save();
    }
}
