using web.reunion.Models;

namespace web.reunion.Interfaces
{
    /// <summary>
    /// Marcações Interface
    /// </summary>
    public interface IMarcacaoService
    {
        Task<List<Marcacao>> GetMarcacoesAsync();
        Task<Marcacao> GetMarcacaoAsync(int id);
        Task PostMarcacaoAsync(AdminDashboardViewModel viewModel, string user, int local, string userRole);
        Task PutMarcacaoAsync(AdminDashboardViewModel viewModel, int id, string user, string userRole, int userLocal);
        Task<List<MarcacaoMaterial>> GetMarcacacoesMaterialAsync(int marcacaoId);
    }
}
