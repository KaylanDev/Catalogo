using Azure.Core;
using Catalogo.Data;
using Catalogo.Models;
using Catalogo.Paginations;
using Microsoft.EntityFrameworkCore;

namespace Catalogo.Repositories
{
    public class ProductRepository : Repository<Produtos>, IProductRepository
    {


        public ProductRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<PagedList<Produtos>> GetPagination(ProdutosParameters produtosParameters)
        {
            var produtos = await _context.Produtos.ToListAsync();
            var produtosOrdenados = PagedList<Produtos>.TopagedList(produtos.OrderBy(c => c.ProdutoId).AsQueryable(), produtosParameters.PageNumber, produtosParameters.PageSize);
            return produtosOrdenados;
        }

        public async Task<PagedList<Produtos>> GetProdutosFiltro(ProdutosFiltroPrecos produtosFiltroPrecos)
        {
            var produtos = await GetAllAsync();
            var produtosAll = produtos.AsQueryable();

            if (produtosFiltroPrecos.Preco.HasValue && !string.IsNullOrEmpty(produtosFiltroPrecos.PrecoCriterio))
            {
                if (produtosFiltroPrecos.PrecoCriterio.Equals("maior", StringComparison.OrdinalIgnoreCase))
                {
                    produtosAll = produtosAll.Where(p => p.Preco > produtosFiltroPrecos.Preco.Value).OrderBy(p => p.Preco);
                }
                else if (produtosFiltroPrecos.PrecoCriterio.Equals("menor", StringComparison.OrdinalIgnoreCase))
                {
                    produtosAll = produtosAll.Where(p => p.Preco < produtosFiltroPrecos.Preco.Value).OrderBy(p => p.Preco);
                }
                else if (produtosFiltroPrecos.PrecoCriterio.Equals("igual", StringComparison.OrdinalIgnoreCase))
                {
                    produtosAll = produtosAll.Where(p => p.Preco == produtosFiltroPrecos.Preco.Value).OrderBy(p => p.Preco);

                }

            }

            var produtosFiltrados = PagedList<Produtos>.TopagedList(produtosAll, produtosFiltroPrecos.PageNumber, produtosFiltroPrecos.PageSize);

            return produtosFiltrados;

        }

        public async Task<IEnumerable<Produtos>> GetProdutosPorCategoria(int id)
        {
            var produtos = await GetAllAsync();
            return produtos.Where(c => c.CategoriaId == id).ToList();

        }

       
    }



}

