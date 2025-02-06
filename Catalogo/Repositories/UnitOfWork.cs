using Catalogo.Data;
using Microsoft.EntityFrameworkCore;

namespace Catalogo.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private IProductRepository _productRepo;

        private ICategoriaRepository _categoriaRepo;
        public AppDbContext _context;

        public IProductRepository ProductRepository
        {
            get {
                return _productRepo = _productRepo ?? new ProductRepository(_context);
            }
        }
        public ICategoriaRepository CategoriaRepository
        {
            get
            {
                return _categoriaRepo = _categoriaRepo ?? new CategoriaRepository(_context);
            }
        }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }
        public void Commit()
        {
                
        }
    }
}
