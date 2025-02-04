using Catalogo.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Catalogo.Repositories
{
    public interface IProductRepository : IRepositoy<Produto>
    {

        public IEnumerable<Produto> GetProdutosPorCategoria(int id);
        

        
    }
}
