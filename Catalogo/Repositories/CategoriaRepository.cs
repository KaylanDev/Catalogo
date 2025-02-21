using Catalogo.Data;
using Catalogo.Models;
using Catalogo.Paginations;
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

        public PagedList<Categorias> GetFiltroNome(CategoriasFiltroNome categoriasFiltroNome)
        {
            var categorias
        }

        public PagedList<Categorias> GetPagination(CategoriasParameters CategoriasParameters)
        {
            var categorias = _context.Categorias.OrderBy(c => c.CategoriaId).AsQueryable();
            var categoraisOrganizadas = PagedList<Categorias>.TopagedList(categorias,CategoriasParameters.PageNumber,CategoriasParameters.PageSize);
            return categoraisOrganizadas;

        }
    }
}
