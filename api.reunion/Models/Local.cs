namespace api.reunion.Models
{
    /// <summary>
    /// Objeto do tipo Local
    /// </summary>
    public partial class Local
    {
        public int LocalId { get; set; }

        public string Descricao { get; set; } = null!;

        public bool IsActive { get; set; }

        public int AdminGroupId { get; set; }

        public virtual AdminGroup AdminGroup { get; set; } = null!;

        public virtual ICollection<Marcacao> Marcacoes { get; set; } = [];

        public virtual ICollection<Sala> Salas { get; set; } = [];
    }
}