using Catalogo.Models;
using System.Collections.ObjectModel;

namespace Catalogo.DTOs
{
    public class CategoriasProdutosDTO
    {

        public int CategoriaId { get; set; }

        public string? Nome { get; set; }

        public string? ImagemUrl { get; set; }

        public ICollection<ProdutosDTO> Produtos { get; set; }

        public CategoriasProdutosDTO()
        {
            Produtos = new Collection<ProdutosDTO>();
        }
    }
}
