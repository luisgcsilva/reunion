using System.ComponentModel.DataAnnotations;

namespace web.reunion.Models
{
    public class MarcacaoMaterial
    {
        public int MarcacaoMateriaisId { get; set; }

        public int MarcacaoId { get; set; }

        public int MaterialId { get; set; }
        [Required(ErrorMessage = "A Quantidade é obrigatória")]
        [Range(1, 1000, ErrorMessage = "A Quantidade tem de ser maior que 1")]
        public int Quantidade { get; set; }
    }
}
