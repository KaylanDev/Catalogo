using Catalogo.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Catalogo.Repositories
{
    public interface IProductRepository : IRepositoy<Produtos>
    {

        public IEnumerable<Produtos> GetProdutosPorCategoria(int id);
        

        
    }
}
