namespace web.reunion.Models
{
    public class AdminDashboardViewModel
    {
        public List<Sala> Salas { get; set; } = null!;
        public List<Marcacao> Marcacoes { get; set; } = null!;
        public List<Material> Materiais { get; set; } = null!;
        public List<SalaMaterial> SalaMateriais { get; set; } = null!;
        public List<MarcacaoMaterial> MarcacaoMateriais { get; set; } = null!;
        public List<Local> Locais { get; set; } = null!;
        public List<Categoria> Categorias { get; set; } = null!;
        public List<AdminGroup> AdminGroups { get; set; } = null!;
        public Material Material { get; set; } = null!;
        public Marcacao Marcacao { get; set; } = null!;
        public List<int> MaterialIds { get; set; } = null!;
        public MarcacaoMaterial MarcacaoMaterial { get; set; } = null!;
        public string HoraInicio { get; set; } = null!;
        public string HoraFim { get; set; } = null!;
        public Dictionary<int, int> MaterialQuantities { get; set; } = null!;
    }
}
