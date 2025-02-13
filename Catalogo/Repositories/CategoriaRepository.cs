using Catalogo.Data;
using Catalogo.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Catalogo.Repositories
{
    public class CategoriaRepository : Repository<Categorias>, ICategoriaRepository
    {
       

        public CategoriaRepository(AppDbContext context):base(context) { }
      
        
        public IEnumerable<Categorias> GetCategoriasProdutos()
        {
            return _context.Categorias.Include(p => p.Produtos).Where(p => p.CategoriaId < 10).ToList();
        }
      

    }
}
