using Azure.Core;
using Catalogo.Data;
using Catalogo.Models;

namespace Catalogo.Repositories
{
    public class ProductRepository : Repository<Produtos>, IProductRepository
    {


        public ProductRepository(AppDbContext context):base(context) {
        
        }

        public IEnumerable<Produtos> GetProdutosPorCategoria(int id)
        {
            return GetAll().Where(c => c.CategoriaId == id).ToList();

        }
    }



}

