using Azure.Core;
using Catalogo.Data;
using Catalogo.Models;

namespace Catalogo.Repositories
{
    public class ProductRepository : Repository<Produto>, IProductRepository
    {


        public ProductRepository(AppDbContext context):base(context) {
        
        }

        public IEnumerable<Produto> GetProdutosPorCategoria(int id)
        {
            return GetAll().Where(c => c.CategoriaId == id).ToList();

        }
    }



}

