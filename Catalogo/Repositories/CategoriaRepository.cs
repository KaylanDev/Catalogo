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

        public async Task<PagedList<Categorias>> GetFiltroNome(CategoriasFiltroNome categoriasFiltroNome)
        {
            var categorias = await GetAllAsync();
            var categoriasOrdenadas = categorias.OrderBy(c => c.CategoriaId).AsQueryable();

            if (!String.IsNullOrEmpty(categoriasFiltroNome.Nome))
            {
                categoriasOrdenadas = categoriasOrdenadas.Where(c => c.Nome.ToLower().Contains(categoriasFiltroNome.Nome.ToLower()));
            }

            if (categorias is null)
            {

            }

            var categoriaFiltradas = PagedList<Categorias>.TopagedList(categoriasOrdenadas,categoriasFiltroNome.PageNumber,categoriasFiltroNome.PageSize);

            return categoriaFiltradas;

        }

        public async Task<PagedList<Categorias>> GetPagination(CategoriasParameters CategoriasParameters)
        {
            var categorias = await _context.Categorias.ToListAsync();
            var categoriasOrdenadas = categorias.OrderBy(c => c.CategoriaId).AsQueryable();
            var categoraisOrganizadas = PagedList<Categorias>.TopagedList(categoriasOrdenadas,CategoriasParameters.PageNumber,CategoriasParameters.PageSize);
            return categoraisOrganizadas;

        }
    }
}
