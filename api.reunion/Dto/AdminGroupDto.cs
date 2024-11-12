namespace api.reunion.Dto
{
    /// <summary>
    /// Objeto de transferência de dados para AdminGroup
    /// </summary>
    public class AdminGroupDto
    {
        public int AdminGroupId { get; set; }

        public string Grupo { get; set; } = null!;

        public string SecurityGroup { get; set; } = null!;
    }
}
