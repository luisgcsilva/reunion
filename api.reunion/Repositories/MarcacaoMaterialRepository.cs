using api.reunion.Data;
using api.reunion.Interfaces;
using api.reunion.Models;

namespace api.reunion.Repositories
{
    public class MarcacaoMaterialRepository(AgendaformacaoContext context) : IMarcacaoMaterialRepository
    {
        private readonly AgendaformacaoContext _context = context;

        /// <summary>
        /// Atualiza uma MarcacaoMaterial
        /// </summary>
        /// <param name="marcacaoMaterial">MarcacaoMaterial atualizada</param>
        /// <returns></returns>
        public bool AtualizarMarcacaoMaterial(MarcacaoMaterial marcacaoMaterial)
        {
            _context.Update(marcacaoMaterial);
            return Save();
        }

        /// <summary>
        /// Cria uma MarcacaoMaterial
        /// </summary>
        /// <param name="marcacaoMaterial">MarcacaoMaterial a ser criada</param>
        /// <returns></returns>
        public bool CriarMarcacaoMaterial(MarcacaoMaterial marcacaoMaterial)
        {
            _context.Add(marcacaoMaterial);
            return Save();
        }

        /// <summary>
        /// Devolve todos Materiais de uma marcacao
        /// </summary>
        /// <param name="id">Id da Marcacao</param>
        /// <returns>Lista de MarcacaoMaterial</returns>
        public ICollection<MarcacaoMaterial> GetMarcacaoMateriais(int id)
        {
            return [.. _context.MarcacaoMateriais.Where(m => m.MarcacaoId == id)];
        }

        /// <summary>
        /// Verifica se existe algum Material para uma marcacao
        /// </summary>
        /// <param name="id">Id da Marcaocao</param>
        /// <returns>True ou false</returns>
        public bool MarcacaoMaterialExists(int id)
        {
            return _context.MarcacaoMateriais.Any(m => m.MarcacaoId == id);
        }

    	/// <summary>
         /// Apaga todas as MarcacaoMaterial de uma Marcacao
         /// </summary>
         /// <param name="id">Id da Marcacao</param>
         /// <returns></returns>
        public bool ApagarMarcacaoMaterial(int id)
        {
            var list = _context.MarcacaoMateriais.Where(m => m.MarcacaoId == id).ToList();
            _context.RemoveRange(list);
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
        /// Devolve todas as MarcacoesMateriais
        /// </summary>
        /// <returns>Lista de MarcacaoMaterial</returns>
        public ICollection<MarcacaoMaterial> GetMarcacoesMateriais()
        {
            return [.. _context.MarcacaoMateriais.OrderBy(m => m.MarcacaoId)];
        }

        /// <summary>
        /// Devolve uma MarcacaoMaterial
        /// </summary>
        /// <param name="id">Id da MarcacaoMaterial</param>
        /// <returns>MarcacaoMaterial</returns>
        public MarcacaoMaterial GetMarcacaoMaterial(int id)
        {
            return _context.MarcacaoMateriais.Where(m => m.MarcacaoMateriaisId == id).First();
        }
    }
}

