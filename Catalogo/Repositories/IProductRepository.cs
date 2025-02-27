using Catalogo.Models;
using Catalogo.Paginations;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Catalogo.Repositories
{
    public interface IProductRepository : IRepositoy<Produtos>
    {

        public  Task<IEnumerable<Produtos>> GetProdutosPorCategoria(int id);
        public  Task<PagedList<Produtos>> GetPagination(ProdutosParameters produtosParameters);
        public  Task<PagedList<Produtos>> GetProdutosFiltro(ProdutosFiltroPrecos produtosFiltroPrecos);

        
    }
}
