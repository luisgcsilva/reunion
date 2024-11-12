using System.ComponentModel.DataAnnotations;

namespace web.reunion.Models
{
    public class Categoria
    {
        public int CategoriaId { get; set; }

        [Display(Name = "Categoria:")]
        [Required(ErrorMessage = "Por favor insira a categoria!")]
        public string Descricao { get; set; } = null!;

    }
}
