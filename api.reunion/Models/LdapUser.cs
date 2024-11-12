namespace api.reunion.Models
{
    /// <summary>
    /// Objeto para um LdapUser
    /// </summary>
    public class LdapUser
    {
        public string? Username { get; set; }

        public string? FullName { get; set; }

        public string? Email { get; set; }

        public IEnumerable<string>? Groups { get; set; }
    }
}
