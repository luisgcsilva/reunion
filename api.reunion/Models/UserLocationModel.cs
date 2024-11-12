namespace api.reunion.Models
{
    /// <summary>
    /// Faz a ligação entre o Utilizador e o Local do inicio de sessão
    /// </summary>
    public class UserLocationModel
    {
        public string Username { get; set; } = null!;

        public int SelectedLocal { get; set; }
    }
}
