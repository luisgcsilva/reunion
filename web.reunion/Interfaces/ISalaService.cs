using web.reunion.Models;

namespace web.reunion.Interfaces
{
    /// <summary>
    /// Salas Interface
    /// </summary>
    public interface ISalaService
    {
        Task<List<Sala>> GetSalasAsync();
        Task<Sala> GetSalaAsync(int salaId);
        Task PostSalaAsync(EditSalaViewModel viewModel);
        Task PutSalaAsync(int salaId, EditSalaViewModel viewModel);
        Task<List<SalaMaterial>> GetSalaMateriaisAsync(int salaId);
        Task PostSalaMaterialAsync(SalaMaterial salaMaterialDto);
    }
}
