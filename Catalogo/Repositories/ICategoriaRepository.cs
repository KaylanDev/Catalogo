using Catalogo.Models;

namespace Catalogo.Repositories;

public interface ICategoriaRepository : IRepositoy<Categoria>
{
  
    IEnumerable<Categoria> GetCategoriasProdutos();


}
