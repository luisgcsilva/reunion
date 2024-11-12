using api.reunion.Data;
using api.reunion.Interfaces;
using api.reunion.Models;

namespace api.reunion.Repositories
{
    public class MarcacaoRepository(AgendaformacaoContext context) : IMarcacaoRepository
    {
        private readonly AgendaformacaoContext _context = context;

        /// <summary>
        /// Devolve todas as marcações
        /// </summary>
        /// <returns>Lista de Marcações</returns>
        public ICollection<Marcacao> GetMarcacoes()
        {
            return [.. _context.Marcacoes.OrderBy(m => m.MarcacaoId)];
        }

        /// <summary>
        /// Verifica se uma sala está ocupada
        /// </summary>
        /// <param name="salaId">Id da sala</param>
        /// <param name="dia">Dia</param>
        /// <param name="horaInicio">Hora de inicio</param>
        /// <param name="horaFim">Hora de fim</param>
        /// <param name="estado">Estado</param>
        /// <returns>True ou false</returns>
        public bool IsSalaBooked(int salaId, DateOnly dia, string horaInicio, string horaFim, string estado)
        {
            var horaI = TimeOnly.Parse(horaInicio);
            var horaF = TimeOnly.Parse(horaFim);

            return _context.Marcacoes.Any(m => m.SalaId == salaId &&
                                               (m.Estado == "Aprovado" || m.Estado == "Pendente") &&
                                               m.Dia == dia &&
                                               ((m.HoraInicio <= horaI && m.HoraFim > horaI) ||
                                                (m.HoraInicio < horaF && m.HoraFim >= horaF) ||
                                                (m.HoraInicio >= horaI && m.HoraFim <= horaF)));
        }

        /// <summary>
        /// Devolve uma marcação
        /// </summary>
        /// <param name="id">Id da marcação</param>
        /// <returns>Marcação</returns>
        public Marcacao GetMarcacao(int id)
        {
            return _context.Marcacoes.Where(m => m.MarcacaoId == id).First();
        }

        /// <summary>
        /// Devolve as marcações de um dia
        /// </summary>
        /// <param name="dateOnly">Dia</param>
        /// <returns>Lista de marcações</returns>
        public ICollection<Marcacao> GetMarcacoesPorData(DateOnly dateOnly)
        {
            return [.. _context.Marcacoes.Where(m => m.Dia == dateOnly)];
        }

        /// <summary>
        /// Devolve as marcações de um estado
        /// </summary>
        /// <param name="estado">Estado</param>
        /// <returns>Lista de marcações</returns>
        public ICollection<Marcacao> GetMarcacoesPorEstado(string estado)
        {
            return [.. _context.Marcacoes.Where(m => m.Estado == estado)];
        }

        /// <summary>
        /// Devolve as marcações de um utilizador
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ICollection<Marcacao> GetMarcacoesPorUtilizador(string user)
        {
            return [.. _context.Marcacoes.Where(m => m.Utilizador == user)];
        }

        /// <summary>
        /// Devolve as marcações de uma sala
        /// </summary>
        /// <param name="salaId">Id da sala</param>
        /// <returns>Lista de Marcacoes</returns>
        public ICollection<Marcacao> GetMarcacoesPorSala(int salaId)
        {
            return [.. _context.Marcacoes.Where(m => m.SalaId == salaId)];
        }

        /// <summary>
        /// Devolve as marcações de um local
        /// </summary>
        /// <param name="localId">Id do local</param>
        /// <returns>Lista de Marcações</returns>
        public ICollection<Marcacao> GetMarcacoesPorLocal(int localId)
        {
            return [.. _context.Marcacoes.Where(m => m.LocalId == localId)];
        }

        /// <summary>
        /// Devolve as marcações modificadas por algum admin
        /// </summary>
        /// <param name="modificadoPor">Email do admin</param>
        /// <returns>Lista de Marcações</returns>
        public ICollection<Marcacao> GetMarcacoesPorAdmin(string modificadoPor)
        {
            return [.. _context.Marcacoes.Where(m => m.ModificadoPor == modificadoPor)];
        }

        /// <summary>
        /// Verifica se uma marcação existe
        /// </summary>
        /// <param name="id">Id da marcação</param>
        /// <returns></returns>
        public bool MarcacaoExists(int id)
        {
            return _context.Marcacoes.Any(m => m.MarcacaoId == id);
        }

        /// <summary>
        /// Cria uma marcação
        /// </summary>
        /// <param name="marcacao">Marcacao a ser criada</param>
        /// <returns></returns>
        public bool CriarMarcacao(Marcacao marcacao)
        {
            _context.Add(marcacao);
            return Save();
        }

        /// <summary>
        /// Atualiza uma marcação
        /// </summary>
        /// <param name="marcacao">Marcação atualizada</param>
        /// <returns></returns>
        public bool AtualizarMarcacao(Marcacao marcacao)
        {
            _context.Update(marcacao);
            return Save();
        }

        /// <summary>
        /// Guarda e verifica se as alterações foram bem sucedidas
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        /// <summary>
        /// Apaga uma marcação
        /// </summary>
        /// <param name="marcacao">Marcacao a ser apagada</param>
        /// <returns></returns>
        public bool ApagarMarcacao(Marcacao marcacao)
        {
            _context.Remove(marcacao);
            return Save();
        }
    }
}
