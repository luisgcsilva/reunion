namespace api.reunion.Models
{
    /// <summary>
    /// Objeto para o Login
    /// </summary>
    public class LoginModel
    {
        public string? Username { get; set; }

        public string? Password { get; set; }

        public int LocalId { get; set; }
    }

}
