using System.ComponentModel.DataAnnotations;

namespace web.reunion.Models
{
    public class AdminGroup
    {
        public int AdminGroupId { get; set; }
        [Display(Name = "Grupo de Administrador na aplicação:")]
        public string Grupo { get; set; } = null!;
        [Display(Name = "Grupo de Segurança na AD:")]
        public string SecurityGroup { get; set; } = null!;
    }
}
