namespace api.reunion.Models
{
    /// <summary>
    /// Objeto que faz a ligação entre o grupo de Administrador e o Grupo de Segurança na AD
    /// </summary>
    public partial class AdminGroup
    {
        public int AdminGroupId { get; set; }

        public string Grupo { get; set; } = null!;

        public string SecurityGroup { get; set; } = null!;

        public virtual ICollection<Local> Locais { get; set; } = [];
    }
}