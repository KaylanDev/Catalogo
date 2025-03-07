using Catalogo.Models;
using Catalogo.Paginations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using X.PagedList;

namespace Catalogo.Repositories
{
    public interface IProductRepository : IRepositoy<Produtos>
    {

        public  Task<IEnumerable<Produtos>> GetProdutosPorCategoria(int id);
        public  Task<IPagedList<Produtos>> GetPagination(ProdutosParameters produtosParameters);
        public  Task<IPagedList<Produtos>> GetProdutosFiltro(ProdutosFiltroPrecos produtosFiltroPrecos);

        
    }
}
