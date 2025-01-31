using Catalogo.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Catalogo.Repositories
{
    public interface IProductRepository
    {
        IQueryable<Produto> Get();
        Produto GetProduto(int id);
        Produto Create(Produto produto);
        bool Update(Produto produto);
        bool Delete(int id);
    }
}
