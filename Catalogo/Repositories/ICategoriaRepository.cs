using Catalogo.Models;
using Catalogo.Paginations;

namespace Catalogo.Repositories;

public interface ICategoriaRepository : IRepositoy<Categorias>
{
  
   Task< IEnumerable<Categorias>> GetCategoriasProdutosAsync();
    public Task<PagedList<Categorias>> GetPaginationAsync(CategoriasParameters CategoriasParameters);
    public Task<PagedList<Categorias>> GetFiltroNomeAsync(CategoriasFiltroNome categoriasFiltroNome);


}
