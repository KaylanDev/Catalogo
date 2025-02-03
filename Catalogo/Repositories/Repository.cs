using Catalogo.Data;
using System.Linq.Expressions;

namespace Catalogo.Repositories
{
    public class Repository<T> : IRepositoy<T> where T : class
    {
        protected readonly AppDbContext _context;
        public T Create(T entity)
        {
            throw new NotImplementedException();
        }

        public T delete(T entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public T GetById(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public T Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
