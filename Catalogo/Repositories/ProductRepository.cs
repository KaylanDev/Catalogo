using Catalogo.Data;
using Catalogo.Models;

namespace Catalogo.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }
        public IQueryable<Produto> Get()
        {
            return _context.Produtos;
        }

        public Produto GetProduto(int id)
        {
            return _context.Produtos.Find(id);
        }
        public Produto Create(Produto produto)
        {
            if (produto is not null)
            {
                _context.Produtos.Add(produto);
                _context.SaveChanges();
                return produto;
            }
            throw new ArgumentNullException();
        }

        public bool Delete(int Id)
        {
            var produto = _context.Produtos.Find(Id);
            if (produto is null)
            {
                return false;
            }

            _context.Produtos.Remove(produto); 
            _context.SaveChanges();
            return true;
        }



        public bool Update(Produto produto)
        {
            if (produto is null)
            {
                return false;
            }

            _context.Produtos.Update(produto);
            _context.SaveChanges();
            return true;
        }
    }



}

