using System.Linq.Expressions;

namespace Catalogo.Repositories
{
    public interface IRepositoy<T>
    {
         Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(Expression <Func<T,bool>> predicate);
        T Create(T entity);
        T Update(T entity);
        T Delete(T entity);

    }
}
