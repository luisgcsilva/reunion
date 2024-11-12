namespace api.reunion.Dto
{
    /// <summary>
    /// Objeto de transferência de dados para Material
    /// </summary>
    public class MaterialDto
    {
        public int MaterialId { get; set; }

        public string Descricao { get; set; } = null!;
    }
}
