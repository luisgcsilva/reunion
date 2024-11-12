using api.reunion.Models;

namespace api.reunion.Interfaces
{
    /// <summary>
    /// Marcações Interface
    /// </summary>
    public interface IMarcacaoRepository
    {
        ICollection<Marcacao> GetMarcacoes();
        Marcacao GetMarcacao(int id);
        ICollection<Marcacao> GetMarcacoesPorData(DateOnly dateOnly);
        ICollection<Marcacao> GetMarcacoesPorEstado(string estado);
        ICollection<Marcacao> GetMarcacoesPorUtilizador(string user);
        ICollection<Marcacao> GetMarcacoesPorAdmin(string modificadoPor);
        ICollection<Marcacao> GetMarcacoesPorSala(int salaId);
        ICollection<Marcacao> GetMarcacoesPorLocal(int localId);
        bool IsSalaBooked(int salaId, DateOnly dia, string horaInicio, string horaFim, string estado);
        bool MarcacaoExists(int id);
        bool CriarMarcacao(Marcacao marcacao);
        bool AtualizarMarcacao(Marcacao marcacao);
        bool ApagarMarcacao(Marcacao marcacao);
        bool Save();
    }
}
