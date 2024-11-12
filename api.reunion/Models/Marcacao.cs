namespace api.reunion.Models
{
    /// <summary>
    /// Objeto do tipo Marcação
    /// </summary>
    public partial class Marcacao
    {
        public int MarcacaoId { get; set; }

        public int? SalaId { get; set; }

        public int NumPessoas { get; set; }

        public int LocalId { get; set; }

        public int CategoriaId { get; set; }

        public string Utilizador { get; set; } = null!;

        public DateOnly Dia { get; set; }

        public TimeOnly HoraInicio { get; set; }

        public TimeOnly HoraFim { get; set; }

        public string Estado { get; set; } = null!;

        public string? Observacoes { get; set; }

        public DateTime DataRegisto { get; set; }

        public string? ModificadoPor { get; set; }

        public DateTime? ModificadoEm { get; set; }

        public string? Motivo { get; set; }

        public virtual Categoria Categoria { get; set; } = null!;

        public virtual Local Local { get; set; } = null!;

        public virtual ICollection<MarcacaoMaterial> MarcacaoMateriais { get; set; } = [];

        public virtual Sala Sala { get; set; } = null!;
    }
}