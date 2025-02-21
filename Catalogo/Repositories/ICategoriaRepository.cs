using Catalogo.Models;
using Catalogo.Paginations;

namespace Catalogo.Repositories;

public interface ICategoriaRepository : IRepositoy<Categorias>
{
  
    IEnumerable<Categorias> GetCategoriasProdutos();
    public PagedList<Categorias> GetPagination(CategoriasParameters CategoriasParameters);
    public PagedList<Categorias> GetFiltroNome(CategoriasFiltroNome categoriasFiltroNome);


}
