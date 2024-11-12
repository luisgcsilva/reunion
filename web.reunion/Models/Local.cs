using System.ComponentModel.DataAnnotations;

namespace web.reunion.Models
{
    public class Local
    {
        public int LocalId { get; set; }
        [Display(Name = "Local:")]
        [Required(ErrorMessage = "Por favor insira o nome do Local!")]
        public string Descricao { get; set; } = null!;
        [Display(Name = "Estado:")]
        public bool IsActive { get; set; }
        [Display(Name = "Grupo de Administrador:")]
        public int AdminGroupId { get; set; }
    }
}
