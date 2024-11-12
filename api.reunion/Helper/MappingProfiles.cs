using AutoMapper;
using api.reunion.Dto;
using api.reunion.Models;

namespace api.reunion.Helper
{
    /// <summary>
    /// Mapeamento de objetos para objetos de transferência de dados e o inverso
    /// </summary>
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            CreateMap<Categoria, CategoriaDto>();
            CreateMap<CategoriaDto, Categoria>();
            CreateMap<AdminGroup, AdminGroupDto>();
            CreateMap<AdminGroupDto, AdminGroup>();
            CreateMap<Local, LocalDto>();
            CreateMap<LocalDto, Local>();
            CreateMap<Marcacao, MarcacaoDto>();
            CreateMap<MarcacaoDto, Marcacao>();
            CreateMap<Sala, SalaDto>();
            CreateMap<SalaDto, Sala>();
            CreateMap<Material, MaterialDto>();
            CreateMap<MaterialDto, Material>();
            CreateMap<SalaMaterial, SalaMaterialDto>();
            CreateMap<SalaMaterialDto, SalaMaterial>();
            CreateMap<MarcacaoMaterial, MarcacaoMaterialDto>();
            CreateMap<MarcacaoMaterialDto, MarcacaoMaterial>();
        }
    }
}
