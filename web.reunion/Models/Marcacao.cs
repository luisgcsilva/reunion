using System.ComponentModel.DataAnnotations;

namespace web.reunion.Models
{
    public class Marcacao
    {
        public int MarcacaoId { get; set; }

        [Display(Name = "Sala:")]
        public int SalaId { get; set; }

        [Display(Name = "Nº de Participantes:")]
        [Range(1, 10000, ErrorMessage = "O Número tem de ser maior que 1!")]
        [Required(ErrorMessage = "O Número de Participantes é obrigatório!")]
        public int NumPessoas { get; set; }

        [Display(Name = "Local:")]
        public int LocalId { get; set; }

        [Display(Name = "Categoria:")]
        public int CategoriaId { get; set; }

        [Display(Name = "Utilizador:")]
        public string Utilizador { get; set; } = null!;

        [Display(Name = "Dia:")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "O Dia é obrigatório!")]
        public DateOnly Dia { get; set; }

        [Display(Name = "Hora de Início:")]
        public TimeOnly HoraInicio { get; set; }

        [Display(Name = "Hora de Fim:")]
        public TimeOnly HoraFim { get; set; }

        [Display(Name = "Estado:")]
        public string Estado { get; set; } = null!;

        [Display(Name = "Observações:")]
        public string? Observacoes { get; set; } = null!;

        [Display(Name = "Data de registo:")]
        public DateTime? DataRegisto { get; set; } = null!;

        [Display(Name = "Modificado Por:")]
        public string? ModificadoPor { get; set; } = null!;

        [Display(Name = "Modificado Em:")]
        public DateTime? ModificadoEm { get; set; } = null!;

        [Display(Name = "Motivo da Recusa:")]
        public string? Motivo { get; set; } = null!;
    }
}
