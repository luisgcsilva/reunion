namespace api.reunion.Dto
{
    /// <summary>
    /// Objeto de transferência de dados para Sala
    /// </summary>
    public class SalaDto
    {
        public int SalaId { get; set; }

        public int LocalId { get; set; }

        public string? Descricao { get; set; } = null!;

        public int? Capacidade { get; set; }

        public string? Localizacao { get; set; } = null!;

        public bool? IsActive { get; set; }

        public string? Cor { get; set; } = null!;
    }
}
