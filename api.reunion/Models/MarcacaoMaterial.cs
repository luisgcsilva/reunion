namespace api.reunion.Models
{
    /// <summary>
    /// Objeto do tipo Marcação Material
    /// Faz a ligação entre os Materiais e uma Marcação
    /// </summary>
    public partial class MarcacaoMaterial
    {
        public int MarcacaoMateriaisId { get; set; }

        public int MarcacaoId { get; set; }

        public int MaterialId { get; set; }

        public int Quantidade { get; set; }

        public virtual Marcacao Marcacao { get; set; } = null!;

        public virtual Material Material { get; set; } = null!;
    }
}