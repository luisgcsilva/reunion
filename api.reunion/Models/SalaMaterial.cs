namespace api.reunion.Models
{
    /// <summary>
    /// Objeto do tipo Sala Material
    /// Faz a ligação entre as Salas e os Materiais
    /// </summary>
    public partial class SalaMaterial
    {
        public int SalaMateriaisId { get; set; }

        public int SalaId { get; set; }

        public int MaterialId { get; set; }

        public int Quantidade { get; set; }

        public virtual Material Material { get; set; } = null!;

        public virtual Sala Sala { get; set; } = null!;
    }
}