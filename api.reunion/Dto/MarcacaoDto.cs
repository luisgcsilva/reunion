namespace api.reunion.Dto
{
    /// <summary>
    /// Objeto de transferência de dados para Marcação
    /// </summary>
    public class MarcacaoDto
    {
        public int MarcacaoId { get; set; }

        public int SalaId { get; set; }

        public int NumPessoas { get; set; }

        public int LocalId { get; set; }
        
        public int CategoriaId { get; set; }

        public string Utilizador { get; set; } = null!;

        public DateOnly Dia { get; set; }

        public TimeOnly HoraInicio { get; set; }

        public TimeOnly HoraFim { get; set; }

        public string Estado { get; set; } = null!;

        public string? Observacoes { get; set; } = null!;

        public DateTime? DataRegisto { get; set; } = null!;

        public string? ModificadoPor { get; set; } = null!;

        public DateTime? ModificadoEm { get; set; } = null!;

        public string? Motivo { get; set; } = null!;
    }
}
