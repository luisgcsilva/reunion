namespace api.reunion.Dto
{
    /// <summary>
    /// Objeto de transferência de dados para Local
    /// </summary>
    public class LocalDto
    {
        public int LocalId { get; set; }

        public string Descricao { get; set; } = null!;

        public bool IsActive { get; set; }

        public int AdminGroupId { get; set; }

    }
}
