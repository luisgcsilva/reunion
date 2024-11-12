namespace api.reunion.Dto
{

    /// <summary>
    /// Objeto de transferência de dados para Categoria
    /// </summary>
    public class CategoriaDto
    {
        public int CategoriaId { get; set; }

        public string Descricao { get; set; } = null!;
    }
}
