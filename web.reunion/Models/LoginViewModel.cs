using System.ComponentModel.DataAnnotations;

namespace web.reunion.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "O utilizador é obrigatório!")]
        public string Username { get; set; } = null!;
        [Required(ErrorMessage = "A palavra-passe é obrigatória!")]
        public string Password { get; set; } = null!;
        public int LocalId { get; set; } 
        public List<Local> Locais { get; set; } = [];
        public bool RememberMe { get; set; }
    }
}
