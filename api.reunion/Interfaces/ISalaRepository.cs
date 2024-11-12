using api.reunion.Models;

namespace api.reunion.Interfaces
{
    /// <summary>
    /// Salas Interface
    /// </summary>
    public interface ISalaRepository
    {
        ICollection<Sala> GetSalas();
        ICollection<Sala> GetSalasPorLocal(int localId);
        Sala GetSala(int id);
        bool SalaExists(int id);
        bool CriarSala(Sala sala);
        bool AtualizarSala(Sala sala);
        bool ApagarSala(Sala sala);
        bool Save();
    }
}
