using System.ComponentModel.DataAnnotations;

namespace web.reunion.Models
{
    public class Material
    {
        public int MaterialId { get; set; }

        [Display(Name = "Descrição:")]
        [Required(ErrorMessage = "A descrição/nome do Material é obrigatório!")]
        public string Descricao { get; set; } = null!;
    }
}
