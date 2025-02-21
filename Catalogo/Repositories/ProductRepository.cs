using Azure.Core;
using Catalogo.Data;
using Catalogo.Models;
using Catalogo.Paginations;

namespace Catalogo.Repositories
{
    public class ProductRepository : Repository<Produtos>, IProductRepository
    {


        public ProductRepository(AppDbContext context) : base(context)
        {

        }

        public PagedList<Produtos> GetPagination(ProdutosParameters produtosParameters)
        {
            var produtos = _context.Produtos.OrderBy(c => c.ProdutoId).AsQueryable();
            var produtosOrdenados = PagedList<Produtos>.TopagedList(produtos, produtosParameters.PageNumber, produtosParameters.PageSize);
            return produtosOrdenados;
        }

        public PagedList<Produtos> GetProdutosFiltro(ProdutosFiltroPrecos produtosFiltroPrecos)
        {
            var produtos = GetAll().AsQueryable();

            if (produtosFiltroPrecos.Preco.HasValue && !string.IsNullOrEmpty(produtosFiltroPrecos.PrecoCriterio))
            {
                if (produtosFiltroPrecos.PrecoCriterio.Equals("maior", StringComparison.OrdinalIgnoreCase))
                {
                    produtos = produtos.Where(p => p.Preco > produtosFiltroPrecos.Preco.Value).OrderBy(p => p.Preco);
                }
                else if (produtosFiltroPrecos.PrecoCriterio.Equals("menor", StringComparison.OrdinalIgnoreCase))
                {
                    produtos = produtos.Where(p => p.Preco < produtosFiltroPrecos.Preco.Value).OrderBy(p => p.Preco);
                }
                else if (produtosFiltroPrecos.PrecoCriterio.Equals("igual", StringComparison.OrdinalIgnoreCase))
                {
                    produtos = produtos.Where(p => p.Preco == produtosFiltroPrecos.Preco.Value).OrderBy(p => p.Preco);

                }

            }

            var produtosFiltrados = PagedList<Produtos>.TopagedList(produtos, produtosFiltroPrecos.PageNumber, produtosFiltroPrecos.PageSize);

            return produtosFiltrados;

        }

        public IEnumerable<Produtos> GetProdutosPorCategoria(int id)
        {
            return GetAll().Where(c => c.CategoriaId == id).ToList();

        }
    }



}

