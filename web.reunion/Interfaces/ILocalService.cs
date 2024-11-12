using web.reunion.Models;

namespace web.reunion.Interfaces
{
    /// <summary>
    /// Locais Interface
    /// </summary>
    public interface ILocalService
    {
        Task<List<Local>> GetLocaisAsync();
        Task<Local> GetLocalAsync(int localId);
        Task PostLocalAsync(Local local);
        Task PutLocalAsync(Local local);
        Task DeleteLocalAsync(int localId);
    }
}
