using Catalogo.Models;
using Catalogo.Paginations;

namespace Catalogo.Repositories;

public interface ICategoriaRepository : IRepositoy<Categorias>
{
  
    IEnumerable<Categorias> GetCategoriasProdutos();
    public Task<PagedList<Categorias>> GetPagination(CategoriasParameters CategoriasParameters);
    public Task<PagedList<Categorias>> GetFiltroNome(CategoriasFiltroNome categoriasFiltroNome);


}
