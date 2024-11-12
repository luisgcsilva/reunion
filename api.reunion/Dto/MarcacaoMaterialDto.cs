namespace api.reunion.Dto
{
    /// <summary>
    /// Objeto de transferência de dados para MarcacaoMaterial
    /// </summary>
    public class MarcacaoMaterialDto
    {
        public int MarcacaoMateriaisId { get; set; }

        public int MarcacaoId { get; set; }

        public int MaterialId { get; set; }

        public int Quantidade { get; set; }
    }
}
