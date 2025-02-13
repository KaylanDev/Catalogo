using Catalogo.Models;

namespace Catalogo.Repositories;

public interface ICategoriaRepository : IRepositoy<Categorias>
{
  
    IEnumerable<Categorias> GetCategoriasProdutos();


}
