using Catalogo.Models;
using Catalogo.Paginations;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Catalogo.Repositories
{
    public interface IProductRepository : IRepositoy<Produtos>
    {

        public IEnumerable<Produtos> GetProdutosPorCategoria(int id);
        public PagedList<Produtos> GetPagination(ProdutosParameters produtosParameters);
        public PagedList<Produtos> GetProdutosFiltro(ProdutosFiltroPrecos produtosFiltroPrecos);

        
    }
}
