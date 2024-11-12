namespace web.reunion.Models
{
    public class LoginModel
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int LocalId { get; set; }
    }
}
