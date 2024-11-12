namespace web.reunion.Models
{
    public class EditSalaViewModel
    {
        public Sala Sala { get; set; } = null!;
        public Material Material { get; set; } = null!;
        public List<Material> Materiais { get; set; } = null!;
        public List<SalaMaterial> SalaMateriais { get; set; } = null!;
        public List<Local> Locais { get; set; } = null!;
        public List<Categoria> Categorias { get; set; } = null!;
        public List<int> MaterialIds { get; set; } = null!;
        public Dictionary<int, int> MaterialQuantities { get; set; } = null!;
    }
}
