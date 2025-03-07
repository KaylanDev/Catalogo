using Catalogo.Models;
using Catalogo.Paginations;
using X.PagedList;

namespace Catalogo.Repositories;

public interface ICategoriaRepository : IRepositoy<Categorias>
{
  
   Task< IEnumerable<Categorias>> GetCategoriasProdutosAsync();
    public Task<IPagedList<Categorias>> GetPaginationAsync(CategoriasParameters CategoriasParameters);
    public Task<IPagedList<Categorias>> GetFiltroNomeAsync(CategoriasFiltroNome categoriasFiltroNome);


}
