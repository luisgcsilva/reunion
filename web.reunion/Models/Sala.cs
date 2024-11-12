using System.ComponentModel.DataAnnotations;

namespace web.reunion.Models
{
    public class Sala
    {
        public int SalaId { get; set; }

        [Display(Name = "Local da Sala:")]
        public int LocalId { get; set; }

        [Display(Name = "Descrição:")]
        [Required(ErrorMessage = "A descrição/nome da sala é obrigatório!")]
        public string? Descricao { get; set; } = null!;

        [Display(Name = "Lotação:")]
        [Required(ErrorMessage = "A lotação da sala é obrigatória!")]
        [Range(1, int.MaxValue, ErrorMessage = "A lotação da sala tem de ser maior que 1!")]
        public int? Capacidade { get; set; }

        [Display(Name = "Localização:")]
        [Required(ErrorMessage = "A localização da sala é obrigatório!")]
        public string? Localizacao { get; set; } = null!;

        [Display(Name = "Estado da Sala:")]
        [Required(ErrorMessage = "O estado da sala é obrigatório!")]
        public bool? IsActive { get; set; }

        [Display(Name = "Cor:")]
        [Required(ErrorMessage = "A cor da sala é obrigatória!")]
        public string? Cor { get; set; } = null!;
    }
}
