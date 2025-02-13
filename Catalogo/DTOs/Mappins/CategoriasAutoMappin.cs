using Catalogo.Models;
using System.Reflection.Metadata.Ecma335;

namespace Catalogo.DTOs.Mappins;

public static class CategoriasAutoMappin
{
    public static Categorias ToCategoria(this CategoriasDTO categoriasDTO)
    {
        if (categoriasDTO is null) return null;

        return new Categorias()
        {
            CategoriaId = categoriasDTO.CategoriaId,
            Nome = categoriasDTO.Nome,
            ImagemUrl = categoriasDTO.ImagemUrl,
        };
    }

    public static CategoriasDTO TocategoriaDTO(this Categorias categoria)
    {
        if (categoria is null) return null;

        return new CategoriasDTO()
        {
            CategoriaId = categoria.CategoriaId,
            ImagemUrl = categoria.ImagemUrl,
            Nome = categoria.Nome
        };
    }

    public static IEnumerable<CategoriasDTO> ToListCateogiraDTO(IEnumerable<Categorias> categorias)
    {
        if (categorias is null || !categorias.Any()) return Enumerable.Empty<CategoriasDTO>();

        return categorias.Select(c => new CategoriasDTO()
        {
            CategoriaId = c.CategoriaId,
            ImagemUrl = c.ImagemUrl,
            Nome = c.Nome
        }).ToList();
    }
}
