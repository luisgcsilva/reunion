namespace api.reunion.Models
{
    /// <summary>
    /// Objeto do tipo Sala 
    /// </summary>
    public partial class Sala
    {
        public int SalaId { get; set; }

        public int LocalId { get; set; }

        public string Descricao { get; set; } = null!;

        public int Capacidade { get; set; }

        public string Localizacao { get; set; } = null!;

        public bool IsActive { get; set; }

        public string? Cor { get; set; }

        public virtual Local Local { get; set; } = null!;

        public virtual ICollection<Marcacao> Marcacoes { get; set; } = [];

        public virtual ICollection<SalaMaterial> SalaMateriais { get; set; } = [];
    }
}