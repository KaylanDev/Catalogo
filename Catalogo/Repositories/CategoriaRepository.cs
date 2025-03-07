using Catalogo.Data;
using Catalogo.Models;
using Catalogo.Paginations;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Threading.Tasks;
using X.PagedList;
using X.PagedList.Extensions;

namespace Catalogo.Repositories
{
    public class CategoriaRepository : Repository<Categorias>, ICategoriaRepository
    {
       

        public CategoriaRepository(AppDbContext context):base(context) { }
      
        
        public async Task<IEnumerable<Categorias>> GetCategoriasProdutosAsync()
        {
            return await _context.Categorias.Include(p => p.Produtos).Where(p => p.CategoriaId < 10).ToListAsync();
        }

        public async Task<IPagedList<Categorias>> GetFiltroNomeAsync(CategoriasFiltroNome categoriasFiltroNome)
        {
            var categorias = await GetAllAsync();
            var categoriasOrdenadas = categorias.OrderBy(c => c.CategoriaId).AsQueryable();

            if (!String.IsNullOrEmpty(categoriasFiltroNome.Nome))
            {
                categoriasOrdenadas = categoriasOrdenadas.Where(c => c.Nome.ToLower().Contains(categoriasFiltroNome.Nome.ToLower()));
            }

             var categoriasFiltradas =   categoriasOrdenadas.ToPagedList(categoriasFiltroNome.PageNumber,categoriasFiltroNome.PageSize);
            return categoriasFiltradas;

        }

        public async Task<IPagedList<Categorias>> GetPaginationAsync(CategoriasParameters CategoriasParameters)
        {
            var categorias = await _context.Categorias.ToListAsync();
            var categoriasOrdenadas = categorias.OrderBy(c => c.CategoriaId).AsQueryable();

            return categoriasOrdenadas.ToPagedList(CategoriasParameters.PageNumber, CategoriasParameters.PageSize);


        }
    }
}
