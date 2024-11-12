namespace api.reunion.Models
{
    /// <summary>
    /// Objeto do tipo Categoria
    /// </summary>
    public partial class Categoria
    {
        public int CategoriaId { get; set; }

        public string Descricao { get; set; } = null!;

        public virtual ICollection<Marcacao> Marcacoes { get; set; } = [];
    }
}