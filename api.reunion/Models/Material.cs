namespace api.reunion.Models
{
    /// <summary>
    /// Objeto do tipo Material
    /// </summary>
    public partial class Material
    {
        public int MaterialId { get; set; }

        public string Descricao { get; set; } = null!;

        public virtual ICollection<MarcacaoMaterial> MarcacaoMateriais { get; set; } = [];

        public virtual ICollection<SalaMaterial> SalaMateriais { get; set; } = [];
    }
}