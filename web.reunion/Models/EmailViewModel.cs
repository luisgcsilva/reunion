namespace web.reunion.Models
{
    public class EmailViewModel
    {
        public string Utilizador { get; set; } = null!;
        public string Estado { get; set; } = null!;
        public string Local {  get; set; } = null!;
        public string NumPessoas { get; set; } = null!;
        public string Sala { get; set; } = null!;
        public string Dia { get; set; } = null!;
        public string HoraInicio { get; set; } = null!;
        public string HoraFim { get; set; } = null!;
        public string Categoria { get; set; } = null!;
        public string Observacoes { get; set; } = null!;
        public string Motivo { get; set; } = null!;
        public List<MarcacaoMaterial> MarcacaoMateriais { get; set; } = null!;

    }
}
