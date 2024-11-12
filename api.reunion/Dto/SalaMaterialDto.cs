namespace api.reunion.Dto
{
    /// <summary>
    /// Objeto de transferência de dados para SalaMaterial
    /// </summary>
    public class SalaMaterialDto
    {
        public int SalaMateriaisId { get; set; }

        public int SalaId { get; set; }

        public int MaterialId { get; set; }

        public int Quantidade { get; set; }
    }
}
