using AutoMapper;
using Catalogo.Models;

namespace Catalogo.DTOs.Mappins;

public class ProdutosDTOMappingProfile : Profile
{
    public ProdutosDTOMappingProfile()
    {
        CreateMap<Categorias,CategoriasDTO>().ReverseMap();
        CreateMap<ProdutosDTO,Produtos>().ReverseMap();
        CreateMap<Produtos,ProdutoDTOUpdateRequest>().ReverseMap();
        CreateMap<Produtos,ProdutoDTOUpdateResponse>().ReverseMap();
    }
}
